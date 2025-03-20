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
			var users = await _context.Users.ToListAsync();
			return Ok(users);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet("{id}")]
	public IActionResult GetUserById(int id)
	{
		var user = _context.Users.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound($"User with ID {id} not found");

		return Ok(user);
	}

	[HttpPost]
	public IActionResult AddUser([FromBody] User newUser)
	{
		if (newUser == null)
			return BadRequest("Invalid user data");

		_context.Users.Add(newUser);
		_context.SaveChanges();
		return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
	}

	[HttpPut("{id}")]
	public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
	{
		var user = _context.Users.FirstOrDefault(u => u.Id == id);
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

		_context.SaveChanges();
		return NoContent();
	}

	[HttpDelete("{id}")]
	public IActionResult DeleteUser(int id)
	{
		var user = _context.Users.FirstOrDefault(u => u.Id == id);
		if (user == null)
			return NotFound($"User with ID {id} not found");

		_context.Users.Remove(user);
		_context.SaveChanges();
		return NoContent();
	}
}