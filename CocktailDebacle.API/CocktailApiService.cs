using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

public class CocktailApiService
{
    private readonly HttpClient _httpClient;

    public CocktailApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Cocktail>> FetchCocktailsFromApi()
    {
        var response = await _httpClient.GetAsync("https://www.thecocktaildb.com/api/json/v1/1/search.php?s=a");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<CocktailApiResponse>(json);

        if (apiResponse?.Drinks == null)
        {
            throw new Exception("Failed to fetch cocktails from API.");
        }

        return apiResponse.Drinks.Select(drink => new Cocktail
            {
                Drink = drink
            })
            .ToList();
    }
}

public class CocktailApiResponse
{
    [JsonPropertyName("drinks")]
    public List<CocktailApiDrink>? Drinks { get; set; }
}
