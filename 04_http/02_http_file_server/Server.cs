using System.Net;
using HeyRed.Mime;

namespace _02_http_file_server;

internal class Server
{
    private HttpListener listener = null!;
    private string host;
    private int port;
    private string[] indexFiles = 
        [
            "index.html"
        ];
    public required string RootDirectory { get; set; }

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

    private async Task HandleRequest(HttpListenerContext ctx)
    {
        string? path = ctx.Request.Url?.AbsolutePath;

        Console.WriteLine($"Path requested: {path}");

        path = path.Trim('/');

        if (string.IsNullOrEmpty(path))
        {
            foreach(string indexFile in indexFiles)
            {
                if (File.Exists(Path.Combine(RootDirectory, indexFile)))
                {
                    path = indexFile;

                    break;
                }
            }
        }

        string filePath = Path.Combine(RootDirectory, path);

        if (File.Exists(filePath))
        {
            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            ctx.Response.ContentLength64 = fs.Length;
            ctx.Response.ContentType = MimeTypesMap.GetMimeType(filePath);

            DateTime lm = File.GetLastWriteTimeUtc(filePath);
            ctx.Response.AddHeader("Last-Modified", $"{lm.ToShortDateString()} {lm.ToShortTimeString()}");

            // string originalFileName = Path.GetFileName(filePath);
            // ctx.Response.AddHeader("Content-Disposition", $"attachment; filename=\"{originalFileName}\"");

            ctx.Response.AddHeader("X-Custom-Header", "Test header");

            fs.CopyTo(ctx.Response.OutputStream);

            ctx.Response.StatusCode = (int)HttpStatusCode.OK;

            fs.Flush();

        }
        else
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }

        ctx.Response.Close();
    }
}
