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
/*
    public async Task<List<string>> getFavoriteIngredients(User user)
    {
        var favoriteCocktails = user.FavoriteCocktails;
        var ingredients = new List<string>();
        int favoriteCocktailsCount = favoriteCocktails.Count;
        if (favoriteCocktailsCount < 3)
            return ingredients;

        var allIngredients = new List<string> { "Light rum", "Lime", "Sugar", "Mint", "Soda water", "Bourbon", "Angostura bitters", "Water", "Vodka", "Gin", "Tequila", "Lemon", "Coca-Cola", "Campari", "Sweet Vermouth", "Blended whiskey", "Powdered sugar", "Cherry", "Dry Vermouth", "Olive", "Triple sec", "Lime juice", "Salt", "Ice", "Maraschino cherry", "Orange peel", "Ginger ale", "Apricot brandy", "Lemon juice", "Southern Comfort", "Amaretto", "Sloe gin", "Orange bitters", "Yellow Chartreuse", "Lemon peel", "Creme de Cacao", "Light cream", "Nutmeg", "Brandy", "Lemon vodka", "Pineapple juice", "Blackberry brandy", "Kummel", "Dark rum", "Kahlua", "Egg white", "Club soda", "White Creme de Menthe", "Tea", "Whipped cream", "Apple brandy", "Applejack", "Orange", "Wine", "Benedictine", "Champagne", "Green Creme de Menthe", "Grand Marnier", "Bitters", "Scotch", "Banana", "Carbonated water", "Coffee liqueur", "Tomato juice", "Tabasco sauce", "Celery salt", "Worcestershire sauce", "Blue Curacao", "Lemonade", "Añejo rum", "Tia maria", "Orange juice", "Maraschino liqueur", "Grenadine", "Egg", "Cachaca", "Egg yolk", "Cognac", "Cherry brandy", "Port", "Chocolate ice-cream", "Dubonnet Rouge", "Sugar syrup", "Pineapple", "Tonic water", "Orange spiral", "Strawberries", "Heavy cream", "Galliano", "Irish whiskey", "Peach brandy", "Sweet and sour", "Green Chartreuse", "Drambuie", "Orgeat syrup", "Grapefruit juice", "Red wine", "Raspberry syrup", "Sherry", "Coffee brandy", "Lime vodka", "Lemon-lime soda", "Rum", "Milk", "151 proof rum", "Lime peel", "Ricard", "Peychaud bitters", "Curacao", "Anisette", "Cointreau", "Strawberry schnapps", "Anis", "Maple syrup", "Creme de Cassis", "Grape juice", "Cream", "Apple juice", "Carrot", "Passion fruit juice", "Yoghurt", "Honey", "Chocolate syrup", "Apple", "Fruit juice", "Fruit", "Mint syrup", "Cumin seed", "Asafoetida", "Mango", "Ginger", "Cayenne pepper", "Aperol", "Cantaloupe", "Berries", "Grapes", "Kiwi", "Papaya", "Cocoa powder", "Cornstarch", "Chocolate", "Cinnamon", "Vanilla", "Butter", "Vanilla extract", "Half-and-half", "Marshmallows", "Espresso", "Absolut Citron", "Peach schnapps", "Cranberry juice", "Peach Vodka", "Sirup of roses", "Ouzo", "Baileys irish cream", "Jägermeister", "Coffee", "Grain alcohol", "Spiced rum", "Cardamom", "Cloves", "Black pepper", "Coriander", "Whipping cream", "Condensed milk", "Anise", "Licorice root", "Wormwood", "Apricot", "Almond flavoring", "Food coloring", "Glycerine", "Angelica root", "Almond", "Allspice", "Marjoram leaves", "Caramel coloring", "Cranberries", "Peppermint extract", "Coconut syrup", "Johnnie Walker", "Fennel seeds", "Brown sugar", "Guava juice", "Apple cider", "Rye whiskey", "Everclear", "Kool-Aid", "Carbonated soft drink", "Sherbet", "Fresca", "Peach nectar", "Firewater", "Absolut Peppar", "Cherry liqueur", "Lager", "Cider", "Blackcurrant cordial", "Frangelico", "Sour mix", "Whiskey", "Hot Damn", "Midori melon liqueur", "Beer", "Dr. Pepper", "White rum", "Pisco", "Egg White", "Irish cream", "Goldschlager", "Ale", "Guinness stout", "Chocolate liqueur", "Sambuca", "Erin Cream", "Advocaat", "Passion fruit syrup", "Vanilla ice-cream", "Oreo cookie", "Sprite", "Pink lemonade", "Iced tea", "Sarsaparilla", "7-Up", "Apple schnapps", "Peppermint schnapps", "Jack Daniels", "Jim Beam", "Pina colada mix", "Daiquiri mix", "Absolut Vodka", "Butterscotch schnapps", "Coconut liqueur", "Crown Royal", "Vermouth", "Chambord raspberry liqueur", "Godiva liqueur", "Absolut Kurant", "Pisang Ambon", "Bitter lemon", "Cherry Heering", "Hot chocolate", "Creme de Banane", "Malibu rum", "Dark Creme de Cacao", "Vanilla vodka", "Wild Turkey", "Grape soda", "Candy", "Banana liqueur", "Kiwi liqueur", "Peachtree schnapps", "Surge", "Jello", "Fruit punch", "Cranberry vodka", "Apfelkorn", "Schweppes Russchian", "Corona", "Bacardi Limon", "Yukon Jack", "Coconut rum", "Tropicana", "Cherries", "Root beer", "Aquavit", "Chocolate milk", "Kirschwasser", "Strawberry liqueur", "Black Sambuca", "Blackcurrant squash", "Zima", "Pepsi Cola", "Orange Curacao", "Cream of coconut", "Absinthe", "Whisky", "Tennessee whiskey", "Limeade", "Melon liqueur", "Mountain Dew", "Corn syrup", "Rumple Minze", "Lime juice cordial", "Gold tequila", "Blueberry schnapps", "Maui", "Raspberry vodka", "Triple Sec", "Cherry Grenadine", "Lemon Juice", "Agave Syrup", "demerara Sugar", "lemon juice", "maraschino liqueur", "Olive Brine", "Lime Juice", "Pineapple Syrup", "St. Germain", "Angostura Bitters", "Pepper", "Lavender", "gin", "Peach Bitters", "Apricot Brandy", "Orange Juice", "Egg Yolk", "orange juice", "Grapefruit Juice", "Maraschino Liqueur", "Coconut milk", "Pineapple Juice", "Prosecco", "Creme de Mure", "Dark Rum", "Ginger Beer", "Raspberry Liqueur", "pineapple juice", "Sugar Syrup", "Soda Water", "Agave syrup", "Tomato Juice", "Lillet Blanc", "White Rum", "Vanilla Ice-Cream", "Whipped Cream", "caramel sauce", "chocolate sauce", "Mini-snickers bars", "Cranberry Juice", "Fresh Lemon Juice", "Fresh Lime Juice", "lemon", "Chocolate Sauce", "Salted Chocolate", "Maraschino Cherry", "Blackberries", "Gold rum", "Pernod", "Ginger beer", "Tonic Water", "Rosemary", "Mezcal", "Lillet", "Orange Peel", "Orange Bitters", "Grape Soda", "Jagermeister", "Sweet and Sour", "Passoa", "Elderflower cordial", "Rosso Vermouth", "Hot Chocolate", "Irish Whiskey", "Blended Scotch", "Honey syrup", "Ginger Syrup", "Islay single malt Scotch", "Falernum", "blackstrap rum", "7-up", "Vanilla syrup", "Rye Whiskey", "Raisins", "Blueberries", "Cherry Juice", "Red Chili Flakes", "Basil", "Cucumber", "Roses sweetened lime juice", "Hpnotiq", "Banana Liqueur", "Malibu Rum", "White Wine", "Apple Brandy", "Apricot Nectar", "Pomegranate juice", "Melon Liqueur", "Coconut Liqueur", "Watermelon", "Light Rum", "Orgeat Syrup", "Rosemary Syrup", "Hot Sauce", "Worcestershire Sauce", "Soy Sauce", "Figs", "Thyme", "Amaro Montenegro", "Ruby Port", "Blood Orange", "Creme De Banane", "Rose", "Apple Schnapps", "Raspberry Vodka", "Lemon Peel" };
        var ingcount = new Dictionary<string, int>();
        foreach (var ingredient in allIngredients)
        {
            ingcount[ingredient] = 0;
        }
        foreach (var id in favoriteCocktails)
        {
            var cocktail = await FetchCocktailsFromApiId(id);
            if (cocktail != null)
            {
                 
            }
        }

        return ingredients;
    }*/

    /*public async Task<List<Cocktail>> RecommendedCocktails(User user)
    {
        var favoriteCocktails = user.FavoriteCocktails;
        var recommendedCocktails = new List<Cocktail>();

        var ingredients = getFavoriteIngredients(user);
        
    }*/
}

public class CocktailApiResponse
{
    [JsonPropertyName("drinks")]
    public List<CocktailApiDrink>? Drinks { get; set; }
}
