using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.IO;

public class CocktailApiService
{
    private readonly HttpClient _httpClient;

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
            return new List<Cocktail>();
        }

        return apiResponse.Drinks.Select(drink => new Cocktail
        {
            Drink = drink
        })
        .ToList();
    }

    public async Task<Cocktail?> FetchCocktailsFromApiId(int id)
    {
        string url = $"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={id}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<CocktailApiResponse>(json);

        if (apiResponse?.Drinks == null || apiResponse.Drinks.Count != 1)
        {
            return null;
        }

        return new Cocktail
        {
            Drink = apiResponse.Drinks.First()
        };
    }

    public async Task<List<int>> GetNewCocktailsIds() // premium API only
    {
        string url = "https://www.thecocktaildb.com/api/json/v1/1/latest.php";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var ids = new List<int>();
        
        using (JsonDocument document = JsonDocument.Parse(json))
        {
            var root = document.RootElement;
            if (root.TryGetProperty("drinks", out JsonElement drinksElement))
            {
                if (drinksElement.ValueKind == JsonValueKind.Object)
                {
                    if (drinksElement.TryGetProperty("idDrink", out JsonElement idDrinkElement) &&
                        int.TryParse(idDrinkElement.GetString(), out int id))
                    {
                        ids.Add(id);
                    }
                }
            }
        }

        return ids;
    }
}

public class CocktailApiResponse
{
    [JsonPropertyName("drinks")]
    public List<CocktailApiDrink>? Drinks { get; set; }
}
