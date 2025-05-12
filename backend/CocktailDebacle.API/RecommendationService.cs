using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RecommendationService
{
    private readonly CocktailDbContext _context;
    private readonly CocktailApiService _apiService;

    public RecommendationService(CocktailDbContext context, CocktailApiService apiService)
    {
        _context = context;
        _apiService = apiService;
    }
	public async Task<List<string>> getFavoriteIngredients(User user)
    {
        var ingredients = new List<string>();
		if (user.FavoriteCocktails == null || user.FavoriteCocktails.Count < 3)
			return ingredients;
        var favoriteCocktails = user.FavoriteCocktails.ToList();
        int favoriteCocktailsCount = favoriteCocktails.Count;

        var allIngredients = new List<string> { "Light rum", "Lime", "Sugar", "Mint", "Soda water", "Bourbon", "Angostura bitters", "Water", "Vodka", "Gin", "Tequila", "Lemon", "Coca-Cola", "Campari", "Sweet Vermouth", "Blended whiskey", "Powdered sugar", "Cherry", "Dry Vermouth", "Olive", "Triple sec", "Lime juice", "Salt", "Ice", "Maraschino cherry", "Orange peel", "Ginger ale", "Apricot brandy", "Lemon juice", "Southern Comfort", "Amaretto", "Sloe gin", "Orange bitters", "Yellow Chartreuse", "Lemon peel", "Creme de Cacao", "Light cream", "Nutmeg", "Brandy", "Lemon vodka", "Pineapple juice", "Blackberry brandy", "Kummel", "Dark rum", "Kahlua", "Egg white", "Club soda", "White Creme de Menthe", "Tea", "Whipped cream", "Apple brandy", "Applejack", "Orange", "Wine", "Benedictine", "Champagne", "Green Creme de Menthe", "Grand Marnier", "Bitters", "Scotch", "Banana", "Carbonated water", "Coffee liqueur", "Tomato juice", "Tabasco sauce", "Celery salt", "Worcestershire sauce", "Blue Curacao", "Lemonade", "Añejo rum", "Tia maria", "Orange juice", "Maraschino liqueur", "Grenadine", "Egg", "Cachaca", "Egg yolk", "Cognac", "Cherry brandy", "Port", "Chocolate ice-cream", "Dubonnet Rouge", "Sugar syrup", "Pineapple", "Tonic water", "Orange spiral", "Strawberries", "Heavy cream", "Galliano", "Irish whiskey", "Peach brandy", "Sweet and sour", "Green Chartreuse", "Drambuie", "Orgeat syrup", "Grapefruit juice", "Red wine", "Raspberry syrup", "Sherry", "Coffee brandy", "Lime vodka", "Lemon-lime soda", "Rum", "Milk", "151 proof rum", "Lime peel", "Ricard", "Peychaud bitters", "Curacao", "Anisette", "Cointreau", "Strawberry schnapps", "Anis", "Maple syrup", "Creme de Cassis", "Grape juice", "Cream", "Apple juice", "Carrot", "Passion fruit juice", "Yoghurt", "Honey", "Chocolate syrup", "Apple", "Fruit juice", "Fruit", "Mint syrup", "Cumin seed", "Asafoetida", "Mango", "Ginger", "Cayenne pepper", "Aperol", "Cantaloupe", "Berries", "Grapes", "Kiwi", "Papaya", "Cocoa powder", "Cornstarch", "Chocolate", "Cinnamon", "Vanilla", "Butter", "Vanilla extract", "Half-and-half", "Marshmallows", "Espresso", "Absolut Citron", "Peach schnapps", "Cranberry juice", "Peach Vodka", "Sirup of roses", "Ouzo", "Baileys irish cream", "Jägermeister", "Coffee", "Grain alcohol", "Spiced rum", "Cardamom", "Cloves", "Black pepper", "Coriander", "Whipping cream", "Condensed milk", "Anise", "Licorice root", "Wormwood", "Apricot", "Almond flavoring", "Food coloring", "Glycerine", "Angelica root", "Almond", "Allspice", "Marjoram leaves", "Caramel coloring", "Cranberries", "Peppermint extract", "Coconut syrup", "Johnnie Walker", "Fennel seeds", "Brown sugar", "Guava juice", "Apple cider", "Rye whiskey", "Everclear", "Kool-Aid", "Carbonated soft drink", "Sherbet", "Fresca", "Peach nectar", "Firewater", "Absolut Peppar", "Cherry liqueur", "Lager", "Cider", "Blackcurrant cordial", "Frangelico", "Sour mix", "Whiskey", "Hot Damn", "Midori melon liqueur", "Beer", "Dr. Pepper", "White rum", "Pisco", "Egg White", "Irish cream", "Goldschlager", "Ale", "Guinness stout", "Chocolate liqueur", "Sambuca", "Erin Cream", "Advocaat", "Passion fruit syrup", "Vanilla ice-cream", "Oreo cookie", "Sprite", "Pink lemonade", "Iced tea", "Sarsaparilla", "7-Up", "Apple schnapps", "Peppermint schnapps", "Jack Daniels", "Jim Beam", "Pina colada mix", "Daiquiri mix", "Absolut Vodka", "Butterscotch schnapps", "Coconut liqueur", "Crown Royal", "Vermouth", "Chambord raspberry liqueur", "Godiva liqueur", "Absolut Kurant", "Pisang Ambon", "Bitter lemon", "Cherry Heering", "Hot chocolate", "Creme de Banane", "Malibu rum", "Dark Creme de Cacao", "Vanilla vodka", "Wild Turkey", "Grape soda", "Candy", "Banana liqueur", "Kiwi liqueur", "Peachtree schnapps", "Surge", "Jello", "Fruit punch", "Cranberry vodka", "Apfelkorn", "Schweppes Russchian", "Corona", "Bacardi Limon", "Yukon Jack", "Coconut rum", "Tropicana", "Cherries", "Root beer", "Aquavit", "Chocolate milk", "Kirschwasser", "Strawberry liqueur", "Black Sambuca", "Blackcurrant squash", "Zima", "Pepsi Cola", "Orange Curacao", "Cream of coconut", "Absinthe", "Whisky", "Tennessee whiskey", "Limeade", "Melon liqueur", "Mountain Dew", "Corn syrup", "Rumple Minze", "Lime juice cordial", "Gold tequila", "Blueberry schnapps", "Maui", "Raspberry vodka", "Triple Sec", "Cherry Grenadine", "Lemon Juice", "Agave Syrup", "demerara Sugar", "lemon juice", "maraschino liqueur", "Olive Brine", "Lime Juice", "Pineapple Syrup", "St. Germain", "Angostura Bitters", "Pepper", "Lavender", "gin", "Peach Bitters", "Apricot Brandy", "Orange Juice", "Egg Yolk", "orange juice", "Grapefruit Juice", "Maraschino Liqueur", "Coconut milk", "Pineapple Juice", "Prosecco", "Creme de Mure", "Dark Rum", "Ginger Beer", "Raspberry Liqueur", "pineapple juice", "Sugar Syrup", "Soda Water", "Agave syrup", "Tomato Juice", "Lillet Blanc", "White Rum", "Vanilla Ice-Cream", "Whipped Cream", "caramel sauce", "chocolate sauce", "Mini-snickers bars", "Cranberry Juice", "Fresh Lemon Juice", "Fresh Lime Juice", "lemon", "Chocolate Sauce", "Salted Chocolate", "Maraschino Cherry", "Blackberries", "Gold rum", "Pernod", "Ginger beer", "Tonic Water", "Rosemary", "Mezcal", "Lillet", "Orange Peel", "Orange Bitters", "Grape Soda", "Jagermeister", "Sweet and Sour", "Passoa", "Elderflower cordial", "Rosso Vermouth", "Hot Chocolate", "Irish Whiskey", "Blended Scotch", "Honey syrup", "Ginger Syrup", "Islay single malt Scotch", "Falernum", "blackstrap rum", "7-up", "Vanilla syrup", "Rye Whiskey", "Raisins", "Blueberries", "Cherry Juice", "Red Chili Flakes", "Basil", "Cucumber", "Roses sweetened lime juice", "Hpnotiq", "Banana Liqueur", "Malibu Rum", "White Wine", "Apple Brandy", "Apricot Nectar", "Pomegranate juice", "Melon Liqueur", "Coconut Liqueur", "Watermelon", "Light Rum", "Orgeat Syrup", "Rosemary Syrup", "Hot Sauce", "Worcestershire Sauce", "Soy Sauce", "Figs", "Thyme", "Amaro Montenegro", "Ruby Port", "Blood Orange", "Creme De Banane", "Rose", "Apple Schnapps", "Raspberry Vodka", "Lemon Peel" };
        var ingcount = new Dictionary<string, int>();
        foreach (var ingredient in allIngredients)
        {
            ingcount[ingredient] = 0;
        }
        foreach (var id in favoriteCocktails)
        {
            var cocktail = await _apiService.FetchCocktailsFromApiId(id);
            if (cocktail != null && cocktail.Drink != null)
            {
                var drink = cocktail.Drink;
                for (int i = 1; i <= 15; i++)
                {
                    var ingredientProperty = typeof(CocktailApiDrink).GetProperty($"StrIngredient{i}");
                    if (ingredientProperty != null)
                    {
                        var ingredient = ingredientProperty.GetValue(drink)?.ToString();
                        if (!string.IsNullOrEmpty(ingredient))
                        {
                            ingcount[ingredient]++;
                        }
                    }
                }
            }
        }
        ingcount = ingcount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        var sortedIngredients = ingcount.Keys.ToList();
        int atLeast = favoriteCocktailsCount / 2 + (favoriteCocktailsCount % 2 == 0 ? 0 : 1);
        return sortedIngredients.Where(x => ingcount[x] >= atLeast).ToList();
    }

    public List<User> getSimilarUsers(List<int> favoriteCocktails, int userId)
    {
        var users = new List<User>();
        if (favoriteCocktails == null || favoriteCocktails.Count == 0)
            return users;
        var dictionary = new Dictionary<User, int>();
        foreach (var user in _context.Users)
        {
            if (user != null && user.Id != userId && user.FavoriteCocktails != null)
            {
                var intersection = favoriteCocktails.Intersect(user.FavoriteCocktails).ToList();
                if (intersection.Count > 0)
                {
                    dictionary[user] = intersection.Count;
                }
            }
        }
        dictionary = dictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        var sortedUsers = dictionary.Keys.ToList();
        int atLeast = favoriteCocktails.Count / 2 + 1;
        return sortedUsers.Where(x => dictionary[x] >= atLeast).ToList();
    }

    public List<User> getUsersInSameZone(string country, string city, int userId)
    {
        var users = new List<User>();
        if (string.IsNullOrEmpty(country))
            return users;
        var sameCountryUsers = _context.Users.Where(x => x.Country == country).ToList();
        if (string.IsNullOrEmpty(city))
            return sameCountryUsers.Where(x => x.Id != userId).ToList();
        var sameCityUsers = sameCountryUsers.Where(x => x.City == city).ToList();
        if (sameCityUsers.Count < 5)
            return sameCountryUsers.Where(x => x.Id != userId).ToList();
        return sameCityUsers.Where(x => x.Id != userId).ToList();
    }

    public List<Cocktail> analyzeCocktails(List<int> favoriteCocktails, List<string> ingredients, List<User> similarUsers, List<User> sameZoneUsers, bool alcohol)
    {
        var cocktails = new List<Cocktail>();
        var dictionary = new Dictionary<Cocktail, float>();
        foreach (var cocktail in _context.Cocktails)
        {
            if (cocktail != null)
            {
                var drink = cocktail.Drink;
                if (drink != null && (alcohol || drink.StrAlcoholic != "Alcoholic"))
                {
                    var cocktailIngredients = new List<string>();
                    for (int i = 1; i <= 15; i++)
                    {
                        var ingredientProperty = typeof(CocktailApiDrink).GetProperty($"StrIngredient{i}");
                        if (ingredientProperty != null)
                        {
                            var ingredient = ingredientProperty.GetValue(drink)?.ToString();
                            if (!string.IsNullOrEmpty(ingredient))
                            {
                                cocktailIngredients.Add(ingredient);
                            }
                        }
                    }
                    var intersection = ingredients.Intersect(cocktailIngredients).ToList();
                    if (intersection.Count > 0)
                    {
                        if (!dictionary.ContainsKey(cocktail))
                            dictionary[cocktail] = 0;
                        dictionary[cocktail] += cocktailIngredients.Count * 50 / intersection.Count;
                    }
                }
            }
        }
        foreach (var user in similarUsers)
        {
			if (user == null || user.FavoriteCocktails == null)
				continue;
            foreach (var cocktail in user.FavoriteCocktails)
            {
				var ck = _context.Cocktails.FirstOrDefault(x => x.Id == cocktail);
                if (ck == null || favoriteCocktails.Contains(cocktail) || (!alcohol && ck.Drink.StrAlcoholic == "Alcoholic"))
                    continue;
                if (!dictionary.ContainsKey(ck))
                    dictionary[ck] = 0;
                dictionary[ck] += 40 / similarUsers.Count;
            }
        }
        foreach (var user in sameZoneUsers)
        {
			if (user == null || user.FavoriteCocktails == null)
				continue;
            foreach (var cocktail in user.FavoriteCocktails)
            {
				var ck = _context.Cocktails.FirstOrDefault(x => x.Id == cocktail);
                if (ck == null || favoriteCocktails.Contains(cocktail) || (!alcohol && ck.Drink.StrAlcoholic == "Alcoholic"))
                    continue;
                if (!dictionary.ContainsKey(ck))
                    dictionary[ck] = 0;
                dictionary[ck] += 10 / sameZoneUsers.Count;
            }
        }

        dictionary = dictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        var sortedCocktails = dictionary.Keys.ToList();
        return sortedCocktails.Take(5).ToList();
    }

    public async Task<List<Cocktail>> GetRecommendedCocktails(User user)
    {
        var favoriteCocktails = user.FavoriteCocktails?.ToList() ?? new List<int>();
        var ingredients = await getFavoriteIngredients(user);
        Console.WriteLine("Favorite Ingredients: " + string.Join(", ", ingredients));
        var usersWithSimilarPreferences = getSimilarUsers(favoriteCocktails, user.Id);
        var usersInSameZone = getUsersInSameZone(user.Country ?? string.Empty, user.City ?? string.Empty, user.Id);

        return analyzeCocktails(favoriteCocktails, ingredients, usersWithSimilarPreferences, usersInSameZone, user.CanDrinkAlcohol);
    }
}