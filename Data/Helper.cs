namespace net.shonx.privatenotes.frontend.Data;

using System.Text;
using net.shonx.privatenotes.backend;
using net.shonx.privatenotes.backend.requests;
using Newtonsoft.Json;

internal static class Helper
{

    private static readonly HttpClient httpClient = new HttpClient();
    internal static async Task<AKVHResponse> WriteRequest(HttpRequestMessage request, Request? data)
    {
        if (data is not null)
        {
            // Console.WriteLine(data.ToString());
            request.Content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
        }
        using HttpResponseMessage response = await httpClient.SendAsync(request);
        var statusCode = response.StatusCode;
        if ((int)statusCode != 200)
        {
            Console.WriteLine($"Helper got response {(int)statusCode}");
        }
        return await GetAKVH(response.Content.ReadAsStream());
    }

    internal static async Task<T?> WriteRequestWithReply<T>(HttpRequestMessage request, Request? data)
    {
        if (data is not null)
            request.Content = new StringContent(data.ToString(), Encoding.UTF8, "application/json"); ;
        using HttpResponseMessage response = await httpClient.SendAsync(request);
        System.Net.HttpStatusCode statusCode = response.StatusCode;
        if ((int)statusCode != 200)
        {
            Console.WriteLine($"Helper got response {(int)statusCode}");
            return default;
        }
        return await GetReply<T>(response.Content.ReadAsStream());
    }

    private static async Task<AKVHResponse> GetAKVH(Stream stream)
    {
        string response = await new StreamReader(stream).ReadToEndAsync();
        if (string.IsNullOrEmpty(response))
            return AKVHResponse.BACKEND_ERROR;
        if (!Enum.TryParse(response, out AKVHResponse akvhresponse))
            return AKVHResponse.BACKEND_ERROR;
        return akvhresponse;
    }

    private static async Task<T?> GetReply<T>(Stream stream)
    {
        string json = await new StreamReader(stream).ReadToEndAsync();
        if (String.IsNullOrEmpty(json))
        {
            return default;
        }
        return JsonConvert.DeserializeObject<T>(json);
    }
}