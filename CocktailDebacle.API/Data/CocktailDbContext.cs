using Microsoft.EntityFrameworkCore;

public class CocktailDbContext : DbContext
{
    public CocktailDbContext(DbContextOptions<CocktailDbContext> options) : base(options) { }

    public DbSet<Cocktail> Cocktails { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relazione: un cocktail è creato da un utente
        modelBuilder.Entity<Cocktail>()
            .HasOne(c => c.CreatedByUser)
            .WithMany(u => u.CreatedCocktails)
            .HasForeignKey(c => c.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relazione: un cocktail può essere preferito da molti utenti
        modelBuilder.Entity<Cocktail>()
            .HasMany(c => c.FavoriteByUsers)
            .WithMany(u => u.FavoriteCocktails)
            .UsingEntity(j => j.ToTable("UserFavoriteCocktails"));
    }
}