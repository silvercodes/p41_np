namespace UTP.Payload;

public interface IPayload
{
    public MemoryStream GetStream();
    public string PType { get; }
}
