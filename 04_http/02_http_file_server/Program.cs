
using _02_http_file_server;

Server server = new Server(host: "192.168.2.150", port: 80)
{
    RootDirectory = @"C:\Users\ThinkPad\Desktop\storage"
};
await server.StartAsync();


