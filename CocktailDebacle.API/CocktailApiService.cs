using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class CocktailApiService
{
	private readonly HttpClient _httpClient;

	public CocktailApiService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<List<Cocktail>> FetchCocktailsFromApi()
	{
		var response = await _httpClient.GetAsync("https://www.thecocktaildb.com/api/json/v1/1/search.php?s=mojito");
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var apiResponse = JsonSerializer.Deserialize<CocktailApiResponse>(json);
		if (apiResponse?.Drinks == null)
		{
			throw new Exception("Failed to fetch cocktails from API.");
		}

		return apiResponse?.Drinks.Select(drink => new Cocktail
		{
			Name = drink.StrDrink ?? "Unknown drink",
			Ingredients = string.Join(", ", drink.Ingredients),
			GlassType = drink.StrGlass ?? "Unknown glass",
			Category = drink.StrCategory ?? "Unknown category",
			ImageUrl = drink.StrDrinkThumb ?? string.Empty
		}).ToList() ?? new List<Cocktail>();
	}
}

public class CocktailApiResponse
{
	public List<CocktailApiDrink> Drinks { get; set; } = new();
}

public class CocktailApiDrink
{
	public string? StrDrink { get; set; }
	public string? StrGlass { get; set; }
	public string? StrCategory { get; set; }
	public string? StrDrinkThumb { get; set; }
	public List<string> Ingredients => new List<string>
	{
		StrIngredient1 ?? string.Empty,
		StrIngredient2 ?? string.Empty,
		StrIngredient3 ?? string.Empty,
		StrIngredient4 ?? string.Empty,
		StrIngredient5 ?? string.Empty,
		StrIngredient6 ?? string.Empty,
		StrIngredient7 ?? string.Empty,
		StrIngredient8 ?? string.Empty,
		StrIngredient9 ?? string.Empty,
		StrIngredient10 ?? string.Empty,
		StrIngredient11 ?? string.Empty,
		StrIngredient12 ?? string.Empty,
		StrIngredient13 ?? string.Empty,
		StrIngredient14 ?? string.Empty,
		StrIngredient15 ?? string.Empty
	}.Where(i => !string.IsNullOrEmpty(i)).ToList();

	public string? StrIngredient1 { get; set; }
	public string? StrIngredient2 { get; set; }
	public string? StrIngredient3 { get; set; }
	public string? StrIngredient4 { get; set; }
	public string? StrIngredient5 { get; set; }
	public string? StrIngredient6 { get; set; }
	public string? StrIngredient7 { get; set; }
	public string? StrIngredient8 { get; set; }
	public string? StrIngredient9 { get; set; }
	public string? StrIngredient10 { get; set; }
	public string? StrIngredient11 { get; set; }
	public string? StrIngredient12 { get; set; }
	public string? StrIngredient13 { get; set; }
	public string? StrIngredient14 { get; set; }
	public string? StrIngredient15 { get; set; }
}