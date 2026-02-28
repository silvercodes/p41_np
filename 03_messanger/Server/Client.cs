using System.Net.Sockets;
using UTP;
using UtpTypes;

namespace Server;

internal class Client
{
    private TcpClient tcpClient;
    private NetworkStream netStream = null!;

    public Client(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient; 
    }

    public void Processing()
    {
        try
        {
            netStream = tcpClient.GetStream();

            UtpEngine engine = new UtpEngine(netStream);

            while(true)
            {
                UtpMessage<JsonPayload> message = engine.Receive<JsonPayload>();

                // ...
            }

            // ...

        }
        catch (Exception ex)
        {

            // ...
            throw;
        }
    }
}
