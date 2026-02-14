using System.Net;
using System.Net.Sockets;
using System.Text;

// const string serverIp = "127.0.0.1";
const string serverIp = "192.168.2.150";
const int serverPort = 80;

// Создание TCP/IPv4 сокета
using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

try
{
    Console.Write("> ");
    string? message = Console.ReadLine();

    if (message is null)
        throw new ArgumentException("Message is empty");

    socket.Connect(serverEndpoint);                 // BLOCKING
    socket.Send(Encoding.UTF8.GetBytes(message));   // BLOCKING

    byte[] buffer = new byte[1024];
    int bytesCount = 0;
    StringBuilder sb = new StringBuilder();
    do
    {
        bytesCount = socket.Receive(buffer);        // BLOCKING
        sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesCount));
    } while (socket.Available > 0);

    Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: Response: {sb.ToString()}");

    socket.Shutdown(SocketShutdown.Both);
    socket.Close();

    Console.WriteLine("Connection closed");
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
