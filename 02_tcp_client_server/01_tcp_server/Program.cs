
using System.Net;
using System.Net.Sockets;

const string host = "127.0.0.1";
const int port = 8080;

TcpListener? listener = null;

try
{
    listener = new TcpListener(IPAddress.Parse(host), port);
    listener.Start();

    Console.WriteLine($"Server started at {host}:{port}");

    TcpClient client = listener.AcceptTcpClient();         // BLOCKING  // Обёртка над remoteSocket
    Console.WriteLine("client connected");

    using NetworkStream stream = client.GetStream();
    using StreamReader reader = new StreamReader(stream);
    using StreamWriter writer = new StreamWriter(stream);

    string? message = reader.ReadLine();                   // BLOCKING
    Console.WriteLine($">>> {message}");

    Thread.Sleep(3000);

    writer.WriteLine("Hello from server!");     // SEND

    stream.Flush();                             // <--- ОСВОБОЖДЕНИЕ ПОТОКА !!!
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
finally
{
    listener?.Stop();
}

Console.ReadLine();



