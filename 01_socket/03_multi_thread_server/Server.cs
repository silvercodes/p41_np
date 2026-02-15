using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _03_multi_thread_server;

internal class Server: IAsyncDisposable
{
    private readonly int backlog;
    public string Host { get; }
    public int Port { get; }
    public IPEndPoint IPEndPoint { get; set; } = null!;
    public Socket ServerSocket { get; set; } = null!;

    public Server(string host, int port, int backlog = 10)
    {
        Host = host;
        Port = port;
        this.backlog = backlog;

        Init();
    }

    private void Init()
    {
        IPEndPoint = new IPEndPoint(IPAddress.Parse(Host), Port);

        ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerSocket.Bind(IPEndPoint);
    }

    public async Task StartAsync()
    {
        try
        {
            ServerSocket.Listen(backlog);
            Console.WriteLine($"Server started at {Host}:{Port}");

            await HandleConnectionsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"START ERROR: {ex.Message}");
        }
    }

    private async Task HandleConnectionsAsync()
    {
        while(true)
        {
            Socket remoteSocket = await ServerSocket.AcceptAsync();

            if (remoteSocket.RemoteEndPoint is IPEndPoint remoteEP)
                await Console.Out.WriteAsync($"Connection opened for remote --> {remoteEP.Address}:{remoteEP.Port}");

            // Как альтернатива: Thread.Start()...
            _ = Task.Run(() => HandleRemoteSocket(remoteSocket));
        }
    }

    private void HandleRemoteSocket(Socket remoteSocket)
    {
        try
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesCount = 0;
                StringBuilder sb = new StringBuilder();
                do
                {
                    bytesCount = remoteSocket.Receive(buffer);
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesCount));
                } while (remoteSocket.Available > 0);

                string response = sb.ToString() switch
                {
                    "time" => DateTime.Now.ToShortTimeString(),
                    "date" => DateTime.Now.ToShortDateString(),
                    _ => "Invalid request message"
                };

                remoteSocket.Send(Encoding.UTF8.GetBytes(response));
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"ERROR WITH CLIENT: {ex.Message}");
        }
        finally
        {
            if (remoteSocket.Connected)
            {
                remoteSocket.Shutdown(SocketShutdown.Both);
                remoteSocket.Close();
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Run(() =>
        {
            if (ServerSocket is not null)
                ServerSocket.Dispose();
        });
    }
}


