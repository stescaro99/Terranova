using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.IO;

public class CocktailApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _logFilePath = "cocktail_api_log.txt";

    public CocktailApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Cocktail>> FetchCocktailsFromApi(char letter)
    {
        string url = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?f={letter}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<CocktailApiResponse>(json);

        if (apiResponse?.Drinks == null)
        {
            int asciiValue = (int)letter;
            await LogToFile($"No drinks found for the letter: {letter} in int {asciiValue}\n");
            return new List<Cocktail>();
        }

        return apiResponse.Drinks.Select(drink => new Cocktail
            {
                Drink = drink
            })
            .ToList();
    }

    private async Task LogToFile(string message)
    {
        using (var writer = new StreamWriter(_logFilePath, append: true))
        {
            await writer.WriteLineAsync($"{DateTime.Now}: {message}");
        }
    }
}

public class CocktailApiResponse
{
    [JsonPropertyName("drinks")]
    public List<CocktailApiDrink>? Drinks { get; set; }
}
