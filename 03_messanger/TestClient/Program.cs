
using UTP;
using System.Net.Sockets;
using UtpTypes;
using UTP.Payload;
using UtpTypes.Payloads;

const string host = "127.0.0.1";
const int port = 8080;

try
{
    Console.WriteLine("Press a key to connect");
    Console.ReadLine();

    using TcpClient tcpClient = new TcpClient(host, port);
    using NetworkStream netStream = tcpClient.GetStream();

    UtpMessage<AuthRequestPayload, ActionTypes> utpm = new UtpMessage<AuthRequestPayload, ActionTypes>()
    {
        ActionCode = ActionTypes.Authenticate,
    };

    utpm.SetHeader("key", "thisisvalue");
    utpm.SetHeader("ses_id", "7532478348");

    utpm.SetPayload(new AuthRequestPayload("vasia@mail.com", "qwerty123"));

    while(true)
    {
        using MemoryStream memStream = new MemoryStream();
        Console.WriteLine("Press Enter to Send");
        Console.ReadLine();
        utpm.FillStream(memStream);
        memStream.CopyTo(netStream);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
	throw;
}
