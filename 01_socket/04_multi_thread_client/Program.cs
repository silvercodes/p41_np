using System.Net;
using System.Net.Sockets;
using System.Text;

const string serverIp = "127.0.0.1";
// const string serverIp = "192.168.2.150";
const int serverPort = 8080;

// Создание TCP/IPv4 сокета
using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

try
{
    Console.WriteLine("Press Enter for connect to server");
    Console.ReadLine();

    socket.Connect(serverEndpoint);

    while(true)
    {
        Console.Write("> ");
        string? message = Console.ReadLine();

        if (message is null)
            continue;

        if (message == "exit")
            break;

        socket.Send(Encoding.UTF8.GetBytes(message));

        byte[] buffer = new byte[1024];
        int bytesCount = 0;
        StringBuilder sb = new StringBuilder();
        do
        {
            bytesCount = socket.Receive(buffer);
            sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesCount));
        } while (socket.Available > 0);

        Console.WriteLine($"Response: {sb.ToString()}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
finally
{
    Disconnect();
}

void Disconnect()
{
    if (socket.Connected)
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }

    Console.WriteLine("Connection closed");
}