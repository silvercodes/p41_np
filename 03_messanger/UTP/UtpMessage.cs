using UTP.Payload;

namespace UTP;

public class UtpMessage<T>
    where T : IPayload
{
    internal const byte MESSAGE_LEN_LABEL_SIZE = 4;
    internal const byte MESSAGE_LEN_ACTION_CODE = 2;
    public short ActionCode { get; internal set; }
    internal Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    public T? Payload { get; private set; }
    public MemoryStream? PayloadStream { get; internal set; }

}
