using System.Text;
using System.Text.Json;
using UTP.Payload;

namespace UtpTypes.Payloads;

public class AuthRequestPayload : JsonPayload
{
    public string Email { get; set; }
    public string Password { get; set; }

    public AuthRequestPayload(string email, string password)
    {
        Email = email; 
        Password = password; 
    }

    public override string GetJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public override MemoryStream GetStream()
    {
        string json = GetJson();

        byte[] bytes = Encoding.UTF8.GetBytes(json);

        MemoryStream memStream = new MemoryStream(bytes.Length);
        memStream.Write(bytes, 0, bytes.Length);

        return memStream;
    }
}
