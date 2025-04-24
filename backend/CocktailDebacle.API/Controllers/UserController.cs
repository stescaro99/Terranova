using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly CocktailDbContext _context;
    private readonly IDeepSeekService _deepSeekService;

    public UserController(CocktailDbContext context, IDeepSeekService deepSeekService)
    {
        _context = context;
        _deepSeekService = deepSeekService;
    }

    [HttpGet("CheckUsername")]
    public async Task<IActionResult> CheckUsername([FromQuery]string username)
    {
        if (string.IsNullOrEmpty(username))
            return BadRequest("Username cannot be null or empty");

        var userExists = await _context.Users.AnyAsync(u => u.Username == username);
        return Ok(!userExists);
    }

    [HttpGet("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromQuery] UpdateUserRequest request)
    {
        if (request == null)
            return BadRequest("Invalid request data");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return NotFound($"User with username {username} not found");

        _context.Attach(user);
        var field = request.Field;
        var value = request.Value;

        switch (field.ToLower())
        {
            case "name":
                user.Name = value;
                break;
            case "username":
                user.Username = value;
                break;
            case "password":
                user.Password = value;
                break;
            case "email":
                user.Email = value;
                break;
            case "birthdate":
                user.BirthDate = value;
                break;
            case "country":
                user.Country = value;
                user.City = null;
                break;
            case "city":
                user.City = value;
                break;
            case "canalcohol":
                user.CanDrinkAlcohol = bool.Parse(value);
                break;
            case "apppermissions":
                user.AppPermissions = bool.Parse(value);
                break;
            case "imageurl":
                user.ImageUrl = value;
                break;
            case "lenguage":
                user.Lenguage = value;
                break;
            default:
                return BadRequest($"Field {field} is not valid");
        }

        _context.Entry(user).State = EntityState.Modified;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery]string username, [FromQuery]string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == username);
        if (user == null)
            return NotFound($"User with username/email {username} not found");
        if (user.Password != password)
            return Unauthorized("Invalid password");
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Username,
                    u.Email,
                    u.Country,
                    u.City,
                    u.CanDrinkAlcohol,
                    u.AppPermissions,
                    u.ImageUrl,
                    FavoriteCocktails = u.FavoriteCocktails,
                    CreatedCocktails = u.CreatedCocktails
                })
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("GetUserFavorites")]
    public async Task<IActionResult> ListFavourite([FromQuery] string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return NotFound($"User with ID {username} not found");

        if (user.FavoriteCocktails == null || !user.FavoriteCocktails.Any())
            return NotFound($"User with ID {username} has no favorite cocktails");

        ICollection<Cocktail> ListCocktails = new List<Cocktail>();
        foreach (var id in user.FavoriteCocktails)
        {
            var cocktail = await _context.Cocktails.FirstOrDefaultAsync(c => c.Drink.IdDrink == id.ToString());
            if (cocktail != null)
            {
                ListCocktails.Add(cocktail);
            }
        }
        return Ok(ListCocktails);
    }

    [HttpGet("byusername")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _context.Users
            .Where(u => u.Username == username)
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.Username,
                u.Email,
                u.Country,
                u.City,
                u.CanDrinkAlcohol,
                u.AppPermissions,
                u.ImageUrl,
                FavoriteCocktails = u.FavoriteCocktails,
                CreatedCocktails = u.CreatedCocktails
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound($"User with Username {username} not found");

        return Ok(user);
    }

    /*[HttpGet("RecommendedCocktails")]
    public async Task<IActionResult> GetRecommendedCocktails([FromQuery] string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return NotFound($"User with ID {username} not found");

        var recommendedCocktails = await apiService.RecommendedCocktails(user);
        return Ok(recommendedCocktails);
    }*/

    [HttpGet("TopCocktails")]
    public async Task<IActionResult> GetTopCocktails([FromQuery] string username, int top, int ret)
    {
        if (ret > top)
            return BadRequest("The number of cocktails to return cannot be greater than the total number of cocktails requested.");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return NotFound($"User with username {username} not found");

        var cocksdictionary = new Dictionary<string, int>();
        var cocktails = await _context.Cocktails.ToListAsync();

        foreach (var cocktail in cocktails)
        {
            if (!string.IsNullOrEmpty(cocktail.Drink?.IdDrink) &&
                user.FavoriteCocktails != null &&
                !user.FavoriteCocktails.Contains(int.Parse(cocktail.Drink.IdDrink)) &&
                (user.CanDrinkAlcohol == true || cocktail.Drink.StrCategory != "Alcoholic"))
            {
                cocksdictionary.Add(cocktail.Drink.IdDrink, cocktail.FavoriteByUsers.Count);
            }
        }

        var topCocktails = cocksdictionary
            .OrderByDescending(x => x.Value)
            .Take(top)
            .ToList();

        var returns = topCocktails
            .OrderBy(c => Guid.NewGuid())
            .Take(ret)
            .ToList();

        var cocksret = new List<Cocktail>();
        foreach (var cocktail in cocktails)
        {
            if (returns.Any(x => x.Key == cocktail.Drink.IdDrink))
            {
                cocksret.Add(cocktail);
            }
        }

        return Ok(cocksret);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.Users
            .Where(u => u.Id == id)
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.Username,
                u.Email,
                u.Country,
                u.City,
                u.CanDrinkAlcohol,
                u.AppPermissions,
                u.ImageUrl,
                FavoriteCocktails = u.FavoriteCocktails,
                CreatedCocktails = u.CreatedCocktails
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound($"User with ID {id} not found");

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User newUser)
    {
        Console.WriteLine($"Dati ricevuti: {System.Text.Json.JsonSerializer.Serialize(newUser)}");
        if (newUser == null)
            return BadRequest("Invalid user data");

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return NotFound($"User with ID {id} not found");

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Country = updatedUser.Country;
        user.City = updatedUser.City;
        user.CanDrinkAlcohol = updatedUser.CanDrinkAlcohol;
        user.AppPermissions = updatedUser.AppPermissions;
        user.ImageUrl = updatedUser.ImageUrl;
        user.FavoriteCocktails = updatedUser.FavoriteCocktails;
        user.CreatedCocktails = updatedUser.CreatedCocktails;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return NotFound($"User with ID {id} not found");

        foreach (var cocktailId in user.CreatedCocktails ?? new List<int>())
        {
            var cocktail = await _context.Cocktails.FirstOrDefaultAsync(c => c.Drink.IdDrink == cocktailId.ToString());
            if (cocktail == null)
                continue;
            if (cocktail.isPrivate == true)
            {
                _context.Cocktails.Remove(cocktail);
            }
            else
            {
                cocktail.CreatedByUser = "Unvailable";
                _context.Cocktails.Update(cocktail);
            }
        }
    
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}