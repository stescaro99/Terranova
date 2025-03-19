using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CocktailController : ControllerBase
{
    private static readonly List<Cocktail> cocktails = new()
    {
        new Cocktail { Id = 1, Name = "Margarita", Ingredients = "Tequila, Lime, Triple Sec", Category = "Alcoholic" },
        new Cocktail { Id = 2, Name = "Mojito", Ingredients = "Rum, Mint, Sugar, Lime, Soda", Category = "Alcoholic" },
        new Cocktail { Id = 3, Name = "Virgin Colada", Ingredients = "Pineapple, Coconut Milk", Category = "Non-Alcoholic" }
    };

    [HttpGet]
    public IActionResult GetCocktails()
    {
        return Ok(cocktails);
    }

    [HttpGet("{id}")]
    public IActionResult GetCocktailById(int id)
    {
        var cocktail = cocktails.FirstOrDefault(c => c.Id == id);
        if (cocktail == null)
            return NotFound($"Cocktail with ID {id} not found");

        return Ok(cocktail);
    }

    [HttpPost]
    public IActionResult AddCocktail([FromBody] Cocktail newCocktail)
    {
        if (newCocktail == null)
            return BadRequest("Invalid cocktail data");

        newCocktail.Id = cocktails.Count + 1;
        cocktails.Add(newCocktail);
        return CreatedAtAction(nameof(GetCocktailById), new { id = newCocktail.Id }, newCocktail);
    }
}

// Modello per il cocktail
public class Cocktail
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Ingredients { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsAlcoholic => Category.Equals("Alcoholic", StringComparison.OrdinalIgnoreCase);
    public string Creator { get; set; } = string.Empty;
    // prezzo medio da cercare in base al paese/citta' (non presente nell'API, collegare un'AI?)
    // public string Price { get; set; } = string.Empty;
}
