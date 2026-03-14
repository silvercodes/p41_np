

HttpClient client = new HttpClient();

HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, @"https://randomuser.me/api/");
req.Headers.Add("Accept", "text/html");
req.Headers.Add("User-Agent", "curl/8.6.0");

HttpResponseMessage res = await client.SendAsync(req);

Console.WriteLine($"STATUS CODE: {res.StatusCode}");

foreach(var header in res.Content.Headers)
{
    Console.Write($"{header.Key}: ");
    foreach (string val in header.Value)
        Console.Write($"{val}; ");

    Console.WriteLine();
}

Console.WriteLine("\n\n\n");

string? body = await res.Content.ReadAsStringAsync();


Console.WriteLine(body);


