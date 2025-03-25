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
        var cocktails = await _context.Cocktails
            .Include(c => c.Drink) // Include Drink to fetch related data
            .ToListAsync();
        return Ok(cocktails);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCocktailById(string id) // Use string for IdDrink
    {
        var cocktail = await _context.Cocktails
            .Include(c => c.Drink) // Include Drink to fetch related data
            .FirstOrDefaultAsync(c => c.Drink.IdDrink == id);
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
            var cocktails = await apiService.FetchCocktailsFromApi();

            foreach (var cocktail in cocktails)
            {
                if (!_context.Cocktails.Any(c => c.Drink.IdDrink == cocktail.Drink.IdDrink)) 
                {
                    _context.Cocktails.Add(cocktail);
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Database populated with cocktails.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error populating database: {ex.Message}");
        }
    }
}