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