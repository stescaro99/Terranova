using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CocktailController : ControllerBase
{
    private readonly IDeepSeekService _deepSeekService;
    private readonly CocktailDbContext _context;

    public CocktailController(CocktailDbContext context, IDeepSeekService deepSeekService)
    {
        _context = context;
        _deepSeekService = deepSeekService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCocktails()
    {
        var cocktails = await _context.Cocktails
            .AsNoTracking()
            .Include(c => c.Drink)
            .Select(c => new
            {
                c.Id,
                c.Drink,
                CreatedByUser = c.CreatedByUser,
                FavoriteByUsers = c.FavoriteByUsers
            })
            .ToListAsync();

        return Ok(cocktails);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCocktails(string str)
    {
        if (string.IsNullOrEmpty(str))
            return BadRequest("Search string cannot be null or empty.");
        var cocktail = await _context.Cocktails
            .Include(c => c.Drink)
            .Where(c => c.Drink != null
                        && c.Drink.StrDrink != null
                        && c.Drink.StrDrink.Contains(str.ToLower())
                        && c.isPrivate == false)
            .ToListAsync();
          {
            var additionalCocktails = await _context.Cocktails
                .Include(c => c.Drink)
                .Where(c => c.Drink != null
                            && c.Drink.StrIngredient1 != null && c.Drink.StrIngredient1.Contains(str.ToLower())
                            || c.Drink.StrIngredient2 != null && c.Drink.StrIngredient2.Contains(str.ToLower())
                            || c.Drink.StrIngredient3 != null && c.Drink.StrIngredient3.Contains(str.ToLower())
                            || c.Drink.StrIngredient4 != null && c.Drink.StrIngredient4.Contains(str.ToLower())
                            || c.Drink.StrIngredient5 != null && c.Drink.StrIngredient5.Contains(str.ToLower())
                            || c.Drink.StrIngredient6 != null && c.Drink.StrIngredient6.Contains(str.ToLower())
                            || c.Drink.StrIngredient7 != null && c.Drink.StrIngredient7.Contains(str.ToLower())
                            || c.Drink.StrIngredient8 != null && c.Drink.StrIngredient8.Contains(str.ToLower())
                            || c.Drink.StrIngredient9 != null && c.Drink.StrIngredient9.Contains(str.ToLower())
                            || c.Drink.StrIngredient10 != null && c.Drink.StrIngredient10.Contains(str.ToLower())
                            || c.Drink.StrIngredient11 != null && c.Drink.StrIngredient11.Contains(str.ToLower())
                            || c.Drink.StrIngredient12 != null && c.Drink.StrIngredient12.Contains(str.ToLower())
                            || c.Drink.StrIngredient13 != null && c.Drink.StrIngredient13.Contains(str.ToLower())
                            || c.Drink.StrIngredient14 != null && c.Drink.StrIngredient14.Contains(str.ToLower())
                            || c.Drink.StrIngredient15 != null && c.Drink.StrIngredient15.Contains(str.ToLower())
                            && c.isPrivate == false)
                .ToListAsync();
            while (cocktail.Count < 5 && additionalCocktails.Count > 0)
            {
                cocktail.Add(additionalCocktails[0]);
                additionalCocktails.RemoveAt(0);
            }
        }
        if (cocktail == null || cocktail.Count == 0)
            return NotFound($"No cocktails found containing '{str}'");
        return Ok(cocktail);
    }

    [HttpGet("CheckCocktailName")]
    public async Task<IActionResult> CheckCocktailName([FromQuery]string name)
    {
        if (string.IsNullOrEmpty(name))
            return BadRequest("Cocktail name cannot be null or empty");
        string nameLower = name.ToLower();

        var CocktailExists = await _context.Cocktails
            .AnyAsync(c => c.Drink != null && c.Drink.StrDrink != null && c.Drink.StrDrink.ToLower() == nameLower);
        if (CocktailExists)
            return Ok(new { Exists = true });
        else
            return Ok(new { Exists = false });
    }

    [HttpPatch("favorite")]
    public async Task<IActionResult> SetFavorite([FromBody] FavoriteRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.CocktailId))
            return BadRequest("Invalid request data.");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
            return NotFound($"User '{request.Username}' not found.");

        var cocktail = await _context.Cocktails
            .FirstOrDefaultAsync(c => c.Drink != null && c.Drink.IdDrink == request.CocktailId);

        if (cocktail == null)
            return NotFound($"Cocktail with ID '{request.CocktailId}' not found.");

        user.FavoriteCocktails ??= new List<int>();

        _context.Attach(user);
        _context.Attach(cocktail);

        if (int.TryParse(request.CocktailId, out int cocktailId))
        {
            if (user.FavoriteCocktails.Contains(cocktailId))
            {
                user.FavoriteCocktails.Remove(cocktailId);
                cocktail.FavoriteByUsers.Remove(user.Username);
            }
            else
            {
                user.FavoriteCocktails.Add(cocktailId);
                cocktail.FavoriteByUsers.Add(user.Username);
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.Entry(cocktail).State = EntityState.Modified;
            _context.Users.Update(user);
            _context.Cocktails.Update(cocktail);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Favorite status updated successfully.", User = user });
        }
        else
        {
            return BadRequest("Invalid CocktailId format.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCocktailById(string id)
    {
        var cocktail = await _context.Cocktails
            .Include(c => c.Drink)
            .FirstOrDefaultAsync(c => c.Drink != null && c.Drink.IdDrink == id);
        if (cocktail == null)
            return NotFound($"Cocktail with ID {id} not found");

        return Ok(cocktail);
    }

    private int GenerateNewCocktailId()
    {
        int maxId = 0;

        foreach (var cocktail in _context.Cocktails)
        {
            if (cocktail.Drink != null && int.TryParse(cocktail.Drink.IdDrink, out int id))
            {
                if (id > maxId)
                    maxId = id;
            }
        }

        if (maxId < 200000)
            return 200000;
        return maxId + 1;
    }

    [HttpPost]
    public async Task<IActionResult> AddCocktail([FromBody] AddCocktailRequest request)
    {
        if (request?.Drink == null || string.IsNullOrEmpty(request.Username))
            return BadRequest("Invalid cocktail data or username.");

        int newId = GenerateNewCocktailId();
        request.Drink.IdDrink = newId.ToString();
        request.Drink.StrInstructionsZH_HANS = request.Instructions;

        var newCocktail = new Cocktail
        {
            Drink = request.Drink,
            CreatedByUser = request.Username,
            isPrivate = request.Private,
            FavoriteByUsers = new List<string>()
        };

        _context.Cocktails.Add(newCocktail);
        await _context.SaveChangesAsync();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user != null)
        {
            user.CreatedCocktails ??= new List<int>();
            user.CreatedCocktails.Add(newId);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        return CreatedAtAction(nameof(GetCocktailById), new { id = request.Drink.IdDrink }, request.Drink);
    }


    [HttpPost("Populate")]
    public async Task<IActionResult> FastPopulate([FromServices] CocktailApiService apiService)
    {
        var chars = new List<char> { '1', '2', '3', '4', '5', '6', '7', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'v', 'w', 'y', 'z' };
        var ids = new HashSet<int> { 11010, 11011, 11012, 11013, 11016, 11019, 11022, 11024, 11025, 11026, 11027, 11028, 11029, 11034, 11050, 11052, 11053, 11054, 11064, 11084, 11102, 11112, 11113, 11117, 11118, 11119, 11121, 11128, 11145, 11146, 11147, 11157, 11164, 11170, 11205, 11227, 11243, 11396, 11403, 11720, 11766, 11786, 12138, 12186, 12198, 12214, 12224, 12316, 12322, 12562, 12564, 12654, 12656, 12658, 12708, 12710, 12712, 12716, 12722, 12730, 12732, 12744, 12750, 12754, 12756, 12760, 12762, 12780, 12790, 12792, 12794, 12796, 12802, 12808, 12854, 12862, 12864, 12870, 12876, 13026, 13032, 13036, 13042, 13066, 13068, 13086, 13162, 13282, 13389, 13405, 13423, 13497, 13625, 13683, 13731, 13807, 13825, 14053, 14071, 14107, 14133, 14272, 14282, 14306, 14356, 14360, 14364, 14372, 14374, 14510, 14560, 14564, 14578, 14584, 14622, 15024, 15182, 15194, 15200, 15224, 15521, 15567, 15597, 15615, 15675, 15789, 15849, 15951, 16082, 16100, 16134, 16176, 16202, 16271, 16289, 16295, 16311, 16333, 16354, 16403, 16405, 16447, 16943, 16958, 16998, 17002, 17005, 17020, 17065, 17066, 17074, 17079, 17094, 17118, 17168, 17174, 17175, 17183, 17184, 17199, 17216, 17220, 17221, 17223, 17224, 17226, 17229, 17233, 17242, 17251, 17268, 17831, 17832, 17834, 17838, 178306, 178308, 178312, 178314, 178317, 178319, 178320, 178321, 178325, 178329, 178331, 178332, 178336, 178337, 178339, 178340, 178353, 178355, 178356, 178369 };
        int count = 0;

        try
        {
            foreach (char c in chars)
            {
                var cocktails = await apiService.FetchCocktailsFromApi(c);
                foreach (var cocktail in cocktails)
                {
                    if (cocktail?.Drink?.IdDrink != null && 
                        int.TryParse(cocktail.Drink.IdDrink, out int cocktailId) &&
                        !_context.Cocktails.Any(c => c.Drink.IdDrink == cocktail.Drink.IdDrink))
                    {
                        count++;
                        _context.Cocktails.Add(new Cocktail
                        {
                            Drink = cocktail.Drink,
                            CreatedByUser = null,
                            FavoriteByUsers = new List<string>()
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }
            foreach (int id in ids)
            {
                await Task.Delay(150);
                var cocktail = await apiService.FetchCocktailsFromApiId(id);
                if (cocktail != null && cocktail.Drink != null && 
                    int.TryParse(cocktail.Drink.IdDrink, out int cocktailId) &&
                    !_context.Cocktails.Any(c => c.Drink.IdDrink == cocktail.Drink.IdDrink))
                {
                    count++;
                    _context.Cocktails.Add(cocktail);
                }
                await _context.SaveChangesAsync();
            }

            return Ok($"Database populated with {count} cocktails.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error populating database: {ex.Message} | Inner Exception: {ex.InnerException?.Message}");
        }
    }


    [HttpGet("RandomCocktails")]
    public async Task<IActionResult> GetRandomCocktails(int number, bool alcohol)
    {
        if (number < 1)
            return BadRequest("Invalid number of cocktails requested.");
        var cocktails = await _context.Cocktails
            .Include(c => c.Drink)
            .ToListAsync();
        string? cda = alcohol ? null : "Alcoholic";
        var randomCocktails = cocktails
            .Where(c => c.Drink.StrAlcoholic != cda && c.isPrivate == false)
            .OrderBy(c => Guid.NewGuid())
            .Take(number)
            .ToList();
        if (randomCocktails.Count == 0)
            return NotFound("No cocktails found matching the criteria.");
        return Ok(randomCocktails);
    }

    [HttpGet("Price(premium)")] // funzionalità premium non implementate nel frontend (API DeepSeek a pagamento)
    public async Task<IActionResult> GetCocktailPrice(string cocktail, string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return NotFound($"User '{username}' not found.");
        string country = user.Country ?? "USA";
        string city = user.City ?? "New York";

        string prompt = $"How much does a {cocktail} cost in {city}, {country} on average?Give me just a value with the currency, no other text.";
        try
        {
            string response = await _deepSeekService.GetResponseAsync(prompt);
            return Ok(new { Cocktail = cocktail, Price = response });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving cocktail price: {ex.Message}");
        }
    }

    [HttpPost("UpdateCocktail(Premium)")] // funzionalità premium (API thecocktaildb premium)
    public async Task<IActionResult> UpdateDatabase([FromServices] CocktailApiService apiService)
    {
        int count = 0;
        var ints = await apiService.GetNewCocktailsIds();
        foreach (var id in ints)
        {
            var cocktail = await apiService.FetchCocktailsFromApiId(id);
            if (cocktail != null && !_context.Cocktails.Any(c => c.Drink.IdDrink == cocktail.Drink.IdDrink))
            {
                count++;
                _context.Cocktails.Add(cocktail);
            }
        }
        await _context.SaveChangesAsync();
        return Ok($"Database updated with {count} new cocktails.");
    }

    [HttpDelete("DeleteCocktail")]
    public async Task<IActionResult> DeleteCocktail(string id)
    {
        var cocktail = await _context.Cocktails
            .Include(c => c.Drink)
            .FirstOrDefaultAsync(c => c.Drink != null && c.Drink.IdDrink == id);

        if (cocktail == null)
            return NotFound($"Cocktail with ID {id} not found.");

        int cocktailId = int.Parse(cocktail.Drink.IdDrink);
        
        foreach (var user in _context.Users)
        {
            if (user.FavoriteCocktails != null && user.FavoriteCocktails.Contains(cocktailId))
            {
                user.FavoriteCocktails.Remove(cocktailId);
                _context.Users.Update(user);
            }
            if (user.CreatedCocktails != null && user.CreatedCocktails.Contains(cocktailId))
            {
                user.CreatedCocktails.Remove(cocktailId);
                _context.Users.Update(user);
            }
            _context.Users.Update(user);
        }
        _context.Cocktails.Remove(cocktail);
        await _context.SaveChangesAsync();
        return Ok($"Cocktail with ID {cocktailId} deleted successfully.");
    }
}