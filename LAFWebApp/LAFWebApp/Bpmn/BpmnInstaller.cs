using Camunda.Worker;
//using Camunda.Worker.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAFBalApp;


namespace LAFWebApp.Bpmn
{
    public static class BpmnInstaller
    {
        public static IServiceCollection AddCamunda(this IServiceCollection services, string camundaRestApiUri)
        {
            services.AddSingleton(_ => new LAFBalApp.Bpmn.BpmnService(camundaRestApiUri));
            services.AddHostedService<LAFBalApp.Bpmn.BpmnProcessDeployService>();

            services.AddCamundaWorker(options =>
            {
                options.BaseUri = new Uri(camundaRestApiUri);
                options.WorkerCount = 1;
            });
            //.AddHandler<UpdateStatusTaskHandler>();

            return services;
        }
    }
}
