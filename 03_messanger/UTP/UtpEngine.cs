using System.Net.Sockets;

namespace UTP;

public class UtpEngine
{
    private NetworkStream netStream;

    public UtpEngine(NetworkStream netStream)
    {
        this.netStream = netStream;
    }

    public UtpMessage Receive()
    {
        int readingSize = ConvertToInt(ReadBytes(UtpMessage.MESSAGE_LEN_LABEL_SIZE));



    }

    private byte[] ReadBytes(int count)
    {
        byte[] bytes = new byte[count];
        netStream.ReadExactly(bytes, 0, count);

        return bytes;
    }

    private int ConvertToInt(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        return BitConverter.ToInt32(bytes, 0);
    }
}
