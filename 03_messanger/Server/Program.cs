using Server;

ServerCore server = new ServerCore("127.0.0.1", 8080);
await server.StartAsync();
