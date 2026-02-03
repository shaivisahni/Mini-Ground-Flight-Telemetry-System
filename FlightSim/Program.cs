using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.IO;

const int Port = 6000;

var listener = new TcpListener(IPAddress.Loopback, Port);
listener.Start();
Console.WriteLine("[FlightSim] Listening...");

while (true)
{
    using TcpClient client = await listener.AcceptTcpClientAsync();
    Console.WriteLine("[FlightSim] GroundStation connected");

    using NetworkStream stream = client.GetStream();
    using var reader = new StreamReader(stream, Encoding.UTF8);
    using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

    var telemetry = new Telemetry();

    // Listen for commands in the background (line-by-line)
    _ = Task.Run(async () =>
    {
        while (true)
        {
            string? line = await reader.ReadLineAsync();
            if (line == null) break;

            try
            {
                var cmd = JsonSerializer.Deserialize<Command>(line);
                if (cmd == null) continue;

                Console.WriteLine($"[FlightSim] Received command: {cmd.Name} {cmd.Argument}");

var ack = CommandHandler.Handle(cmd, telemetry);

await writer.WriteLineAsync(JsonSerializer.Serialize(ack));
Console.WriteLine($"[FlightSim] ACK sent: {ack.Status}");

            }
            catch
            {
                Console.WriteLine("[FlightSim] Could not parse command (ignored).");
            }
        }
    });

    // Telemetry loop (line-by-line)
    while (client.Connected)
    {
        await writer.WriteLineAsync(JsonSerializer.Serialize(telemetry));
        await Task.Delay(1000);
    }
}
