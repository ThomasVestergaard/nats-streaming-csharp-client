# NATS .NET C# Streaming Client
This repository has the same functionality as the official nats-streaming C# client repo here: https://github.com/nats-io/csharp-nats-streaming as per commit id #b33cc19651e945ae6a6757dbeebcc411e05d3859.

The official repo has been very quite for some time and I needed to add some connection functionality. If the official repo is resurrected, I'd be happy to contribute.

[![License Apache 2.0](https://img.shields.io/badge/License-Apache2-blue.svg)](https://www.apache.org/licenses/LICENSE-2.0)

## Add callback handler on lost connection to server
A callback can be added when heartbeats from the server is no longer detected. This requires the server to be started with heartbeats (look for the -hbi command in the documentation: https://github.com/nats-io/nats-streaming-server#command-line-arguments)

```csharp
var natsConnectionOptions = StanOptions.GetDefaultOptions();
natsConnectionOptions.NatsURL = "nats://localhost:4222";
natsConnectionOptions.ServerHeartbeatTimeoutCallback = () =>
{
    // Handle connection issues here
};
natsConnectionOptions.ServerHeartbeatTimeoutMillis = 10000; // Invoke callback after 10 seconds of silence

var cf = new StanConnectionFactory();
using (var c = cf.CreateConnection("test-cluster", "appname"))
{
    using (c.Subscribe("foo", (obj, args) =>
    {
        Console.WriteLine(
            System.Text.Encoding.UTF8.GetString(args.Message.Data));
    }))
    {
        c.Publish("foo", System.Text.Encoding.UTF8.GetBytes("hello"));
    }
}
```
