using System.Net.Sockets;
using UTP.Payload;

namespace UTP;

public class UtpEngine
{
    private NetworkStream netStream;
    private MemoryStream memStream;

    public UtpEngine(NetworkStream netStream)
    {
        this.netStream = netStream;
    }

    public UtpMessage<T> Receive<T>()
        where T : IPayload
    {
        int readingSize = ConvertToInt(ReadBytes(UtpMessage<T>.MESSAGE_LEN_LABEL_SIZE));

        memStream = new MemoryStream(readingSize);

        // memStream.Write(ReadBytes(readingSize), 0, readingSize);
        // TODO: ??? DEBUG
        memStream.SetLength(readingSize);
        netStream.ReadExactly(memStream.GetBuffer(), 0, readingSize);
        memStream.Position = 0;

        UtpMessage<T> utpm = new UtpMessage<T>();

        using StreamReader sr = new StreamReader(memStream);

        ExtractActionCode(utpm);
        ExtractMetadata(utpm, sr);



        return utpm;

    }

    private void ExtractActionCode<T>(UtpMessage<T> utpm)
        where T : IPayload
    {
        short actionCode = ConvertToShort(ReadBytes(UtpMessage<T>.MESSAGE_LEN_ACTION_CODE));

        utpm.ActionCode = actionCode;
    }

    private void ExtractMetadata<T>(UtpMessage<T> utpm, StreamReader sr)
        where T : IPayload
    {
        //
    }

    private void ExtractPayloadStream<T>(UtpMessage<T> utpm)
        where T : IPayload
    {
        //
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

    private short ConvertToShort(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        return BitConverter.ToInt16(bytes, 0);
    }
}
