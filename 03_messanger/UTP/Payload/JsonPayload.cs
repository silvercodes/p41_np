
namespace UTP.Payload;

public abstract class JsonPayload : IPayload
{
    public string PType => "json";
    public abstract MemoryStream GetStream();
    public abstract string GetJson();
}
