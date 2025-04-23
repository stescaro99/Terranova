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
        var cocktail = await _context.Cocktails
            .Include(c => c.Drink)
            .Where(c => c.Drink != null && c.Drink.StrDrink != null && c.Drink.StrDrink.StartsWith(str) && c.isPrivate == false)
            .ToListAsync();
        if (cocktail == null || cocktail.Count == 0)
            return NotFound($"No cocktails found starting with '{str}'");
        return Ok(cocktail);
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

    [HttpPost]
    public async Task<IActionResult> AddCocktail([FromBody] Cocktail newCocktail)
    {
        if (newCocktail == null)
            return BadRequest("Invalid cocktail data");

        _context.Cocktails.Add(newCocktail);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCocktailById), new { id = newCocktail.Drink.IdDrink }, newCocktail);
    }

    [HttpPost("populate")]
    public async Task<IActionResult> PopulateCocktails([FromServices] CocktailApiService apiService)
    {
        try
        {
            int count = 0;
            char letter = 'a';

            while (letter <= 'z')
            {
                var cocktails = await apiService.FetchCocktailsFromApi(letter);

                foreach (var cocktail in cocktails)
                {
                    if (cocktail?.Drink?.IdDrink != null && 
                        int.TryParse(cocktail.Drink.IdDrink, out int cocktailId) &&
                        !_context.Cocktails.Any(c => c.Drink.IdDrink == cocktail.Drink.IdDrink))
                    {
                        count++;
                        _context.Cocktails.Add(new Cocktail
                        {
                            Id = cocktailId,
                            Drink = cocktail.Drink,
                            CreatedByUser = null,
                            FavoriteByUsers = new List<string>()
                        });
                    }
                }
                letter++;
                await _context.SaveChangesAsync();
            }

            return Ok($"Database populated with {count} cocktails.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error populating database: {ex.Message}");
        }
    }

    [HttpPost("populatebyid")] //solo se non ci sono altri modi
    public async Task<IActionResult> PopulateCocktailsById([FromServices] CocktailApiService apiService)
    {
        try
        {
            int count = 0;
            int i = 0;
            var ints = new HashSet<int> {  11000, 11001, 11002, 11003, 11004, 11005, 11006, 11007, 11008, 11009, 11010, 11011, 11012, 11013, 11014, 11016, 11019, 11020, 11021, 11022, 11023, 11024, 11025, 11026, 11027, 11028, 11029, 11034, 11046, 11050, 11052, 11053, 11054, 11055, 11060, 11064, 11084, 11102, 11106, 11112, 11113, 11117, 11118, 11119, 11120, 11121, 11124, 11128, 11129, 11145, 11146, 11147, 11149, 11157, 11164, 11170, 11202, 11205, 11222, 11224, 11227, 11239, 11242, 11243, 11251, 11255, 11288, 11320, 11324, 11326, 11338, 11339, 11368, 11369, 11375, 11382, 11387, 11390, 11391, 11396, 11403, 11407, 11408, 11410, 11415, 11416, 11417, 11418, 11419, 11420, 11422, 11423, 11424, 11433, 11462, 11470, 11472, 11476, 11524, 11528, 11542, 11558, 11566, 11580, 11600, 11602, 11604, 11634, 11658, 11662, 11664, 11666, 11670, 11690, 11720, 11728, 11766, 11786, 11798, 11844, 11870, 11872, 11936, 11938, 11959, 11961, 11963, 11965, 11983, 11985, 11987, 11989, 11991, 11993, 12055, 12057, 12067, 12071, 12087, 12089, 12091, 12093, 12097, 12101, 12107, 12127, 12130, 12138, 12158, 12162, 12186, 12188, 12190, 12196, 12198, 12214, 12224, 12256, 12308, 12316, 12322, 12362, 12370, 12388, 12402, 12418, 12420, 12434, 12436, 12442, 12444, 12446, 12450, 12452, 12460, 12474, 12518, 12528, 12560, 12562, 12564, 12572, 12618, 12630, 12654, 12656, 12658, 12668, 12670, 12672, 12674, 12688, 12690, 12692, 12694, 12696, 12698, 12702, 12704, 12706, 12708, 12710, 12712, 12714, 12716, 12718, 12720, 12722, 12724, 12726, 12728, 12730, 12732, 12734, 12736, 12738, 12744, 12746, 12748, 12750, 12752, 12754, 12756, 12758, 12760, 12762, 12764, 12766, 12768, 12770, 12772, 12774, 12776, 12780, 12782, 12784, 12786, 12790, 12792, 12794, 12796, 12798, 12800, 12802, 12808, 12820, 12854, 12856, 12862, 12864, 12870, 12876, 12890, 12910, 12914, 12916, 12944, 12954, 12988, 13020, 13024, 13026, 13032, 13036, 13042, 13056, 13058, 13066, 13068, 13070, 13072, 13086, 13128, 13162, 13190, 13192, 13194, 13196, 13198, 13200, 13202, 13204, 13206, 13214, 13222, 13282, 13328, 13332, 13377, 13389, 13395, 13405, 13423, 13497, 13499, 13501, 13535, 13539, 13581, 13621, 13625, 13675, 13683, 13731, 13751, 13775, 13807, 13825, 13837, 13847, 13861, 13899, 13936, 13938, 13940, 13971, 14029, 14053, 14065, 14071, 14087, 14095, 14107, 14133, 14157, 14167, 14181, 14195, 14209, 14229, 14272, 14282, 14306, 14356, 14360, 14364, 14366, 14372, 14374, 14378, 14446, 14456, 14466, 14482, 14510, 14538, 14560, 14564, 14578, 14584, 14586, 14588, 14594, 14598, 14602, 14608, 14610, 14622, 14642, 14688, 14730, 14752, 14782, 14842, 14860, 14888, 14956, 14978, 15006, 15024, 15026, 15082, 15086, 15092, 15106, 15178, 15182, 15184, 15194, 15200, 15224, 15226, 15254, 15266, 15288, 15300, 15328, 15330, 15346, 15403, 15409, 15423, 15427, 15511, 15515, 15521, 15567, 15597, 15615, 15639, 15675, 15691, 15743, 15761, 15789, 15801, 15813, 15825, 15841, 15849, 15853, 15933, 15941, 15951, 15997, 16031, 16041, 16047, 16082, 16100, 16108, 16134, 16158, 16176, 16178, 16196, 16202, 16250, 16262, 16271, 16273, 16275, 16289, 16295, 16311, 16333, 16354, 16403, 16405, 16419, 16447, 16485, 16942, 16943, 16951, 16958, 16963, 16967, 16984, 16985, 16986, 16987, 16990, 16991, 16992, 16995, 16998, 17002, 17005, 17006, 17015, 17020, 17027, 17035, 17044, 17060, 17065, 17066, 17074, 17079, 17094, 17105, 17108, 17114, 17118, 17120, 17122, 17135, 17141, 17167, 17168, 17174, 17175, 17176, 17177, 17178, 17180, 17181, 17182, 17183, 17184, 17185, 17186, 17187, 17188, 17189, 17190, 17191, 17192, 17193, 17194, 17195, 17196, 17197, 17198, 17199, 17200, 17201, 17202, 17203, 17204, 17205, 17206, 17207, 17208, 17209, 17210, 17211, 17212, 17213, 17214, 17215, 17216, 17217, 17218, 17219, 17220, 17221, 17222, 17223, 17224, 17225, 17226, 17227, 17228, 17229, 17230, 17233, 17239, 17241, 17242, 17245, 17246, 17247, 17248, 17249, 17250, 17251, 17252, 17253, 17254, 17255, 17256, 17266, 17267, 17268, 17824, 17825, 17826, 17827, 17828, 17829, 17830, 17831, 17832, 17833, 17834, 17835, 17836, 17837, 17838, 17839, 17840, 178306, 178307, 178308, 178309, 178310, 178311, 178312, 178313, 178314, 178315, 178316, 178317, 178318, 178319, 178320, 178321, 178322, 178323, 178325, 178326, 178327, 178328, 178329, 178330, 178331, 178332, 178333, 178334, 178335, 178336, 178337, 178338, 178339, 178340, 178341, 178342, 178343, 178344, 178345, 178346, 178347, 178348, 178349, 178350, 178352, 178353, 178354, 178355, 178356, 178357, 178358, 178359, 178360, 178362, 178363, 178364, 178365, 178366, 178367, 178368, 178369, 178370, 178371 };

            while (i < ints.Count)
            {
                var cocktail = await apiService.FetchCocktailsFromApiId(ints.ElementAt(i));

                if (cocktail != null && cocktail.Drink != null && 
                    int.TryParse(cocktail.Drink.IdDrink, out int cocktailId) &&
                    !_context.Cocktails.Any(c => c.Drink.IdDrink == cocktail.Drink.IdDrink))
                {
                    count++;
                    _context.Cocktails.Add(cocktail);
                }
                i++;
                await Task.Delay(200);
                await _context.SaveChangesAsync();
            }
            return Ok($"Database populated with {count} cocktails.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error populating database: {ex.Message}");
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

    [HttpGet("Price")]
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

    [HttpPost("UpdateCocktail (Premium)")]
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

    [HttpDelete("reset")]
    public async Task<IActionResult> ResetDatabase()
    {
        try
        {
            _context.Cocktails.RemoveRange(_context.Cocktails);
            await _context.SaveChangesAsync();
            return Ok("Database reset successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error resetting database: {ex.Message}");
        }
    }
}