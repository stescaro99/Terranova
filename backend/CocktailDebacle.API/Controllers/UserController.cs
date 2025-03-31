using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly CocktailDbContext _context;

    public UserController(CocktailDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _context.Users
                .Include(u => u.FavoriteCocktails)
                .ThenInclude(c => c.Drink)
                .Include(u => u.CreatedCocktails)
                .ThenInclude(c => c.Drink)
                .ToListAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.Users
            .Include(u => u.FavoriteCocktails)
            .ThenInclude(c => c.Drink)
            .Include(u => u.CreatedCocktails)
            .ThenInclude(c => c.Drink)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound($"User with ID {id} not found");

        return Ok(user);
    }

    [HttpGet("{Username}")]
    public async Task<IActionResult> GetUserByName(string username)
    {
         Console.WriteLine($"Ricevuto parametro username: {username}");
        var user = await _context.Users
            .Include(u => u.FavoriteCocktails)
            .ThenInclude(c => c.Drink)
            .Include(u => u.CreatedCocktails)
            .ThenInclude(c => c.Drink)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return NotFound($"User with name {username} not found");

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User newUser)
    {
        if (newUser == null)
            return BadRequest("Invalid user data");
        if (newUser.BirthDate == default)
        return BadRequest("Invalid BirthDate format. Use 'YYYY-MM-DD'.");

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

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}