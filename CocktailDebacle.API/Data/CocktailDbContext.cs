using Microsoft.EntityFrameworkCore;

public class CocktailDbContext : DbContext
{
    public CocktailDbContext(DbContextOptions<CocktailDbContext> options) : base(options) { }

    public DbSet<Cocktail> Cocktails { get; set; }
}
