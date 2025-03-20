using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CocktailController : ControllerBase
{
	private readonly CocktailDbContext _context;

	public CocktailController(CocktailDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<IActionResult> GetCocktails()
	{
		var cocktails = await _context.Cocktails.ToListAsync();
		return Ok(cocktails);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetCocktailById(int id)
	{
		var cocktail = await _context.Cocktails.FirstOrDefaultAsync(c => c.Id == id);
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

		return CreatedAtAction(nameof(GetCocktailById), new { id = newCocktail.Id }, newCocktail);
	}

	[HttpPost("populate")]
	public async Task<IActionResult> PopulateCocktails([FromServices] CocktailApiService apiService, [FromServices] CocktailDbContext dbContext)
	{
		try
		{
			var cocktails = await apiService.FetchCocktailsFromApi();

			foreach (var cocktail in cocktails)
			{
				if (!dbContext.Cocktails.Any(c => c.Name == cocktail.Name))
				{
					dbContext.Cocktails.Add(cocktail);
				}
			}

			await dbContext.SaveChangesAsync();
			return Ok("Database populated with cocktails.");
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Error populating database: {ex.Message}");
		}
}
}
