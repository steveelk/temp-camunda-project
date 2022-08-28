using Camunda.Worker;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAFBalApp.Bpmn
{
    [HandlerTopics("Topic_CreateInvoice", LockDuration = 10_000)]
    public class UpdateStatusTaskHandler : ExternalTaskHandler
    {
        private readonly IMediator bus;

        public UpdateStatusTaskHandler(IMediator bus)
        {
            this.bus = bus;
        }

        public override async Task<IExecutionResult> Process(ExternalTask externalTask)
        {

            await bus.Send(null);

            return new CompleteResult { };
        }
    }
}
