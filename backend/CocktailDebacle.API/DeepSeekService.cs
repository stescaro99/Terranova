using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public interface IDeepSeekService
{
    Task<string> GetResponseAsync(string prompt);
}

public class DeepSeekService : IDeepSeekService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _endpoint;

    public DeepSeekService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["DeepSeek:ApiKey"] ?? throw new ArgumentNullException("DeepSeek:ApiKey is not configured.");
        _endpoint = configuration["DeepSeek:Endpoint"] ?? throw new ArgumentNullException("DeepSeek:Endpoint is not configured.");
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        var requestBody = new
        {
            model = "deepseek-chat",
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = prompt }
            },
            stream = false
        };

        var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
        {
            Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")
        };
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error retrieving response: {response.StatusCode} - {response.ReasonPhrase}. Details: {errorContent}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        dynamic? result = JsonConvert.DeserializeObject(responseBody);
        return result?.choices?[0]?.message?.content?.Trim() ?? "No response from API";
    }
}