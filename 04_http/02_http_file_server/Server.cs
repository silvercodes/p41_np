using System.Net;

namespace _02_http_file_server;

internal class Server
{
    private HttpListener listener = null!;
    private string host;
    private int port;

    public Server(string host = "127.0.0.1", int port = 80)
    {
        this.host = host;
        this.port = port;

        Initialize();
    }

    private void Initialize()
    {
        listener = new HttpListener();
        listener.Prefixes.Add($@"http://{host}:{port}/");       // <-- LAST SLASH !!!
    }

    public async Task StartAsync()
    {
        listener.Start();

        Console.WriteLine($"Server started at {host}:{port}");

        while(true)
        {
            try
            {
                HttpListenerContext ctx = await listener.GetContextAsync();

                _ = Task.Run(() => HandleRequest(ctx));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }
    }

    private void HandleRequest(HttpListenerContext ctx)
    {
        Console.WriteLine("REQUEST RECEIVED!!!");
    }
}
