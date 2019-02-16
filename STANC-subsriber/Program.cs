using System;
using STANC;

namespace STANC_subsriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting subsriber app...");

            var natsConnectionOptions = StanOptions.GetDefaultOptions();
            natsConnectionOptions.NatsURL = "nats://localhost:4222";
            natsConnectionOptions.ServerHeartbeatTimeoutCallback = () =>
            {
                Console.WriteLine("Hurrr... connection problem!");
            };
            natsConnectionOptions.ServerHeartbeatTimeoutMillis = 10000;

            var stanConnection = new StanConnectionFactory().CreateConnection("test-cluster", $"subscirber-name", natsConnectionOptions);

            var options = StanSubscriptionOptions.GetDefaultOptions();
            

            stanConnection.Subscribe($"some-channel", options, (sender, e) =>
            {
                Console.WriteLine("Message received...");
            });

            Console.WriteLine("Started... Hit any key to quit.");
            Console.ReadKey();

        }
    }
}
