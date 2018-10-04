using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Rates.Functions.BackEnd
{
    public static class FetchRates
    {
        [FunctionName("FetchRates")]
        public static async Task Run(
            [TimerTrigger("0 0 * * * *")]TimerInfo myTimer,
            [Queue(Lookups.RatesAddedQueue)] ICollector<string> destinationQueue,
            ILogger log)
        {
            var mediator = ContainerFactory.Container.Resolve<IMediator>();

            var events = await mediator.Send(new Domain.WriteModel.FetchRates.Command());
            
            log.LogInformation($"Sending {events.Count()} events to {Lookups.RatesAddedQueue} queue");
            foreach (var @event in events)
            {
                var json = JsonConvert.SerializeObject(@event, Lookups.JsonSettings);
                destinationQueue.Add(json);
            }
        }
    }
}
