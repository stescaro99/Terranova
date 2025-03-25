using Microsoft.EntityFrameworkCore;

public class CocktailDbContext : DbContext
{
    public CocktailDbContext(DbContextOptions<CocktailDbContext> options) : base(options) { }

    public DbSet<Cocktail> Cocktails { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cocktail>()
            .OwnsOne(c => c.Drink, drink =>
            {
                drink.Property(d => d.IdDrink).HasColumnName("Drink_IdDrink");
                drink.Property(d => d.StrDrink).HasColumnName("Drink_StrDrink");
                drink.Property(d => d.StrGlass).HasColumnName("Drink_StrGlass");
                drink.Property(d => d.StrCategory).HasColumnName("Drink_StrCategory");
                drink.Property(d => d.StrDrinkThumb).HasColumnName("Drink_StrDrinkThumb");
                drink.Property(d => d.StrInstructions).HasColumnName("Drink_StrInstructions");
                drink.Property(d => d.StrIngredient1).HasColumnName("Drink_StrIngredient1");
                drink.Property(d => d.StrIngredient2).HasColumnName("Drink_StrIngredient2");
                drink.Property(d => d.StrIngredient3).HasColumnName("Drink_StrIngredient3");
                drink.Property(d => d.StrIngredient4).HasColumnName("Drink_StrIngredient4");
                drink.Property(d => d.StrIngredient5).HasColumnName("Drink_StrIngredient5");
                drink.Property(d => d.StrIngredient6).HasColumnName("Drink_StrIngredient6");
                drink.Property(d => d.StrMeasure1).HasColumnName("Drink_StrMeasure1");
                drink.Property(d => d.StrMeasure2).HasColumnName("Drink_StrMeasure2");
                drink.Property(d => d.StrMeasure3).HasColumnName("Drink_StrMeasure3");
                drink.Property(d => d.StrMeasure4).HasColumnName("Drink_StrMeasure4");
                drink.Property(d => d.StrMeasure5).HasColumnName("Drink_StrMeasure5");
                drink.Property(d => d.StrMeasure6).HasColumnName("Drink_StrMeasure6");
            });

        modelBuilder.Entity<Cocktail>()
            .HasOne(c => c.CreatedByUser)
            .WithMany(u => u.CreatedCocktails)
            .HasForeignKey(c => c.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Cocktail>()
            .HasMany(c => c.FavoriteByUsers)
            .WithMany(u => u.FavoriteCocktails)
            .UsingEntity(j => j.ToTable("UserFavoriteCocktails"));
    }
}