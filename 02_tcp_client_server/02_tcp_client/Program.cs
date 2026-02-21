
using System.Net.Sockets;

const string serverHost = "127.0.0.1";
const int serverPort = 8080;

Console.WriteLine("Enter a key for connect");
Console.ReadLine();

try
{
    TcpClient client = new TcpClient();
    client.Connect(serverHost, serverPort);

    using NetworkStream stream = client.GetStream();
    using StreamReader reader = new StreamReader(stream);
    using StreamWriter writer = new StreamWriter(stream);

    writer.WriteLine("Hello from client");
    stream.Flush();

    string? response = reader.ReadLine();
    Console.WriteLine($">>> {response}");

    client.Close();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
