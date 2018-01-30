using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace nsb_scheduling_msmq
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var endpointConfig = new EndpointConfiguration("Test");
            endpointConfig.UseTransport<MsmqTransport>();
            endpointConfig.UsePersistence<InMemoryPersistence>();
            endpointConfig.SendFailedMessagesTo("Test.error");
            
            endpointConfiguration.AssemblyScanner().ExcludeAssemblies("NServiceBus.Azure.Transports.WindowsAzureStorageQueues.dll");

            var msmqEndpoint = await Endpoint.Start(endpointConfig).ConfigureAwait(false);

            await msmqEndpoint.ScheduleEvery(
                TimeSpan.FromSeconds(5),
            context =>
            {
                Console.WriteLine("Task executed");
                return Task.CompletedTask;
            }).ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await msmqEndpoint.Stop()
            .ConfigureAwait(false);
        }
    }
}
