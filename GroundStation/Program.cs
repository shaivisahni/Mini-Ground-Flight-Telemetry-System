using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Text.Json;

const string Host = "127.0.0.1";
const int Port = 6000;

Console.WriteLine("[GroundStation] Connecting...");

using TcpClient client = new TcpClient();
await client.ConnectAsync(Host, Port);
Console.WriteLine("[GroundStation] Connected");

using NetworkStream stream = client.GetStream();
using var reader = new StreamReader(stream, Encoding.UTF8);
using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

// Send ONE command after connecting
var command = new { Name = "SET_MODE", Argument = "SAFE" };
string cmdJson = JsonSerializer.Serialize(command);
await writer.WriteLineAsync(cmdJson);
Console.WriteLine("[GroundStation] Sent command: SET_MODE SAFE");

// Read messages line-by-line forever
while (true)
{
    string? line = await reader.ReadLineAsync();
    if (line == null) break;

    Console.WriteLine($"[Received] {line}");
}
