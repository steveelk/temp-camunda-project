using Camunda.Api.Client;
using Camunda.Api.Client.Deployment;
using Camunda.Api.Client.Message;
using Camunda.Api.Client.ProcessDefinition;
using Camunda.Api.Client.ProcessInstance;
using Camunda.Api.Client.UserTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAFBalApp.Bpmn
{
    public class BpmnService
    {
        private readonly CamundaClient camunda;

        public BpmnService(string camundaRestApiUri)
        {
            this.camunda = CamundaClient.Create(camundaRestApiUri);
        }

        #region DEPLOYMENT

        public async Task DeployProcessDefinition()
        {
            var bpmnResourceStream = this.GetType()
                .Assembly
                .GetManifestResourceStream("BpmnHrPoc.Bpmn.Resources.Process_HR_NET.bpmn");

            try
            {

                await camunda.Deployments.Create(
                    "HR POC Deployment",
                    true,
                    true,
                    null,
                    null,
                    new ResourceDataContent(bpmnResourceStream, "Process_HR_NET.bpmn"));
            }
            catch (Exception e)
            {
                throw new ApplicationException("Failed to deploy process definition", e);
            }
        }

        #endregion

        #region PROCESSES AND INSTANCES
        public async Task<int> StartProcessInstance()
        {
            //this.processStartupVariables = psVars;
            //var processParams = new StartProcessInstance()
            //    .SetVariable("avbAnnualLeaves", VariableValue.FromObject(psVars.avbAnnualLeaves))
            //    .SetVariable("employee_id", VariableValue.FromObject(psVars.employee_id))
            //    .SetVariable("employee_manager", VariableValue.FromObject(psVars.employee_manager))
            //    .SetVariable("leaveType", VariableValue.FromObject(psVars.leaveType));

            //processParams.BusinessKey = psVars.getBusinessKey();
            //Task<ProcessInstanceWithVariables> processStartResult = null;
            //try
            //{
            //    processStartResult =
            //        camunda.ProcessDefinitions.ByKey(psVars.processName).StartProcessInstance(processParams);
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}


            //return processStartResult == null ? -1 : processStartResult.Id;

            return 0;
        }

        public async Task<List<ProcessInstanceInfo>> GetAllProcesseInstances()
        {
            var processStartResult = await
                camunda.ProcessInstances.Query().List();

            return processStartResult;
        }

        #endregion

        #region TASKS

        public async Task<UserTaskInfo> GetProcessTaskByBusinessKey(string businessKey)
        {
            List<UserTaskInfo> tasks = await camunda.UserTasks.Query(new TaskQuery
            {
                ProcessInstanceBusinessKey = businessKey
            }).List();

            if (tasks != null && tasks.Count > 0)
            {
                return tasks[0];
            }

            return null;
        }

        public async Task<UserTaskInfo> ClaimTask(string taskId, string user)
        {
            await camunda.UserTasks[taskId].Claim(user);
            var task = await camunda.UserTasks[taskId].Get();
            return task;
        }

        #endregion

        public async Task<List<UserTaskInfo>> GetTasksForCandidateGroup(string group, string user)
        {
            var groupTaskQuery = new TaskQuery
            {
                ProcessDefinitionKeys = { "BKEY" },
                CandidateGroup = group
            };
            var groupTasks = await camunda.UserTasks.Query(groupTaskQuery).List();

            if (user != null)
            {
                var userTaskQuery = new TaskQuery
                {
                    ProcessDefinitionKeys = { "BKEY" },
                    Assignee = user
                };
                var userTasks = await camunda.UserTasks.Query(userTaskQuery).List();

                groupTasks.AddRange(userTasks);
            }

            return groupTasks;
        }

        public async Task<UserTaskInfo> CompleteTask(string taskId, Dictionary<string, string> variables = null)
        {
            var task = await camunda.UserTasks[taskId].Get();
            var completeTask = new CompleteTask();

            if (variables != null)
            {
                foreach (string key in variables.Keys)
                {
                    completeTask.SetVariable(key, VariableValue.FromObject(variables[key]));
                }
            }


            await camunda.UserTasks[taskId].Complete(completeTask);
            return task;
        }

        public async Task SendMessageInvoicePaid(string messageName)
        {
            await camunda.Messages.DeliverMessage(new CorrelationMessage
            {
                BusinessKey = "BKEY",
                MessageName = messageName
            });
        }

        public async Task CleanupProcessInstances()
        {
            var instances = await camunda.ProcessInstances
                .Query(new ProcessInstanceQuery
                {
                    ProcessDefinitionKey = "BKEY"
                })
                .List();

            if (instances.Count > 0)
            {
                await camunda.ProcessInstances.Delete(new DeleteProcessInstances
                {
                    ProcessInstanceIds = instances.Select(i => i.Id).ToList()
                });
            }
        }
    }
}
