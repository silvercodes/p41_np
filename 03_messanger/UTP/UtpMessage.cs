using System.Buffers.Binary;
using System.Text.Json;
using UTP.Payload;

namespace UTP;

public class UtpMessage<TPayload, TActionEnum>
    where TPayload : IPayload
    where TActionEnum : System.Enum
{
    private const char HEADER_SEPARATOR = ':';
    private const string HEADER_PAYLOAD_TYPE_KEY = "ptype";
    private const string HEADER_PAYLOAD_LEN_KEY = "plen";

    internal const byte MESSAGE_LEN_LABEL_SIZE = 4;
    internal const byte MESSAGE_LEN_ACTION_CODE = 2;

    public TActionEnum ActionCode { get; set; }
    internal Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    public TPayload? Payload { get; private set; }
    public MemoryStream? PayloadStream { get; internal set; }


    public void SetHeader(string key, string value)
    {
        Headers[key] = value;
    }

    public void SetHeader(string? headerLine)
    {
        if (headerLine is null)
            return;

        string[] chunks = headerLine.Split(HEADER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        SetHeader(chunks[0], chunks[1]);
    }

    public void SetPayload(TPayload payload)
    {
        Payload = payload;

        PayloadStream = payload.GetStream();

        Headers[HEADER_PAYLOAD_LEN_KEY] = PayloadStream.Length.ToString();
        Headers[HEADER_PAYLOAD_TYPE_KEY] = Payload.PType;
    }

    public void FillStream(MemoryStream memStream)
    {
        memStream.Write(new byte[MESSAGE_LEN_LABEL_SIZE], 0, MESSAGE_LEN_LABEL_SIZE);

        memStream.Write(ConvertActionToBytes(ActionCode));      // TODO: CHECK !!!!

        StreamWriter writer = new StreamWriter(memStream);

        foreach (KeyValuePair<string, string> h in Headers)
        {
            writer.WriteLine($"{h.Key}{HEADER_SEPARATOR}{h.Value}");
        }
        writer.WriteLine();
        writer.Flush();

        if (Payload is not null && PayloadStream is not null)
        {
            PayloadStream.Position = 0;
            PayloadStream.CopyTo(memStream);
        }

        memStream.Position = 0;

        int len = (int)memStream.Length - MESSAGE_LEN_LABEL_SIZE;
        memStream.Write(BitConverter.GetBytes(len));

        memStream.Position = 0;
    }

    byte[] ConvertActionToBytes(TActionEnum action)
    {
        return JsonSerializer.SerializeToUtf8Bytes<TActionEnum>(action);
    }
}
