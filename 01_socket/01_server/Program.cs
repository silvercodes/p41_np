using System.Net;
using System.Net.Sockets;
using System.Text;



// const string serverIp = "127.0.0.1";
const string serverIp = "192.168.2.150";
const int port = 80;

// Создание TCP/IPv4 сокета
using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(serverIp), port);

try
{
	socket.Bind(endpoint);
	socket.Listen();

    Console.WriteLine($"Server started at {serverIp}:{port}");

	while(true)
	{

        // BLOCKING (до тех пор, пока не установиться успекшное соединение)
        // Ожидание нового подключения
        // 1. Абстакция над установленным соединением с конкретным клиентом
        // 2. Реальный системный сокет для общения с конкретным клиентом
        Socket remoteSocket = socket.Accept();

        Console.WriteLine("Connection established...");

        byte[] buffer = new byte[1024];
        int bytesCount = 0;
        StringBuilder sb = new StringBuilder();
        do
        {
            bytesCount = remoteSocket.Receive(buffer);
            sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesCount));
        } while (remoteSocket.Available > 0);

        Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {sb.ToString()}");

        Thread.Sleep(2000);

        string response = "Hello from server";
//        string response = @"HTTP/1.1 200 OK
//Content-Type: text/html; charset=utf-8
//Connection: close

//<h1 style='color:red;'>Vasia</h1>";

        remoteSocket.Send(Encoding.UTF8.GetBytes(response));


        // Закрытие соединения и удаление remoteSocket
        remoteSocket.Shutdown(SocketShutdown.Both);     // Выполнение протокола закрытия соединения
        remoteSocket.Close();                           // Освобождение ресурсов из-под remoteSocket

        Console.WriteLine("Connection closed...");
    }

}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}




