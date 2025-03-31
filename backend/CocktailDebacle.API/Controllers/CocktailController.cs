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
            int count = 0;
            char letter = '1';
            var ignoredLetters = new HashSet<char> { '8', ':', ';', '<', '=', '>', '?', '@', 'U', 'X', '[', ']', '^', '_', '`' };
            //con letter == % stampa tutti i cocktail ma probabilmente sono divisi in pagine e prendo solo i primi 25           
            while (letter < 'a')
            {
                if (ignoredLetters.Contains(letter))
                {
                    letter++;
                    continue;
                }
                var cocktails = await apiService.FetchCocktailsFromApi(letter);

                foreach (var cocktail in cocktails)
                {
                    if (!_context.Cocktails.Any(c => c.Drink.IdDrink == cocktail.Drink.IdDrink))
                    {
                        count++;
                        _context.Cocktails.Add(cocktail);
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

    private async Task LogToFile(string message)
    {
        string logFilePath = "cocktail_log.txt"; // Nome del file di log
        using (var writer = new StreamWriter(logFilePath, append: true))
        {
            await writer.WriteLineAsync($"{DateTime.Now}: {message}");
        }
    }

    [HttpDelete("reset")]
    public async Task<IActionResult> ResetDatabase()
    {
        try
        {
            _context.Cocktails.RemoveRange(_context.Cocktails); // Rimuove tutti i record dalla tabella Cocktails
            await _context.SaveChangesAsync(); // Salva le modifiche nel database
            return Ok("Database reset successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error resetting database: {ex.Message}");
        }
    }
}