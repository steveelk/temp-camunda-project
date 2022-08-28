using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAFBalApp.Bpmn
{
    public class BpmnProcessDeployService : IHostedService
    {
        private readonly BpmnService service;

        public BpmnProcessDeployService(BpmnService bpmnService)
        {
            this.service = bpmnService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await service.DeployProcessDefinition();

            await service.CleanupProcessInstances();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
