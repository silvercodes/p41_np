using System.Net;
using System.Net.Sockets;
using System.Text;

const string localhost = "192.168.2.150";
const string remoteHost = "192.168.2.150";

Console.Write("Enter a local port: ");
int localPort = Int32.Parse(Console.ReadLine());
Console.Write("Enter a remote port: ");
int remotePort = Int32.Parse(Console.ReadLine());


using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

_ = Task.Run(() =>
{
	try
	{
		socket.Bind(new IPEndPoint(IPAddress.Parse(localhost), localPort));

		while(true)
		{
			byte[] buffer = new byte[65507];        // Размер буфера МАКСИМАЛЬНЫЙ!!!
			int byteCount = 0;

			EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

			byteCount = socket.ReceiveFrom(buffer, ref remoteEP);           // BLOCKING
			string message = Encoding.UTF8.GetString(buffer, 0, byteCount);

			if (remoteEP is IPEndPoint remoteEPWIthInfo)
                Console.Write($"from: {remoteEPWIthInfo.Address}:{remoteEPWIthInfo.Port} --> ");
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {message}");
		}
	}
	catch (Exception ex)
	{
        Console.WriteLine($"ERROR: {ex.Message}");
	}
	finally
	{
		socket.Close();
	}
});

try
{
    while(true)
	{
		string? message = Console.ReadLine();

		if (message is null)
			continue;

		byte[] data = Encoding.UTF8.GetBytes(message);
		socket.SendTo(data, new IPEndPoint(IPAddress.Parse(remoteHost), remotePort));
	}
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
finally
{
    socket.Close();
}

