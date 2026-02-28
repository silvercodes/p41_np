
using UTP;
using System.Net.Sockets;
using UtpTypes;

const string host = "127.0.0.1";
const int port = 8080;

try
{
    Console.WriteLine("Press a key to connect");
    Console.ReadLine();

    using TcpClient tcpClient = new TcpClient(host, port);
    using NetworkStream netStream = tcpClient.GetStream();

    // UtpMessage<JsonPayload> utpm = new UtpMessage<JsonPayload>();

    int len = 101;
    byte[] bytes = BitConverter.GetBytes(len);
    netStream.Write(bytes, 0, 4);



    Console.WriteLine("Press a key to continue");
    Console.ReadLine();

}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
	throw;
}
