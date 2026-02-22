using System.Net;
using System.Net.Sockets;

namespace Server;

internal class ServerCore
{
    private string host;
    private int port;
    private TcpListener listener;
    private List<Client> clients = new List<Client>();

    public ServerCore(string host, int port = 8080)
    {
        this.host = host;
        this.port = port;

        listener = new TcpListener(IPAddress.Parse(host), port);
    }

    public async Task StartAsync()
    {
        try
        {
            listener.Start();
            await Console.Out.WriteLineAsync($"Server started at {host}:{port}");

            while(true)
            {
                TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                await Console.Out.WriteLine("Client connected...");

                Client client = new Client(tcpClient);

                // TODO: Add: client events handlers from Server

                clients.Add(client);

                _ = Task.Run(() => client.Processing());
            }
        }
        catch (Exception ex)
        {
            // TODO: This is mock. Add ERRORHANDLER
            await Console.Out.WriteLineAsync($"ERROR: {ex.Message}");
        }
    }
}
