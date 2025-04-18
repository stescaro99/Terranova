using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
                drink.Property(d => d.StrDrinkAlternate).HasColumnName("Drink_StrDrinkAlternate");
                drink.Property(d => d.StrTags).HasColumnName("Drink_StrTags");
                drink.Property(d => d.StrVideo).HasColumnName("Drink_StrVideo");
                drink.Property(d => d.StrGlass).HasColumnName("Drink_StrGlass");
                drink.Property(d => d.StrCategory).HasColumnName("Drink_StrCategory");
                drink.Property(d => d.StrIBA).HasColumnName("Drink_StrIBA");
                drink.Property(d => d.StrAlcoholic).HasColumnName("Drink_StrAlcoholic");
                drink.Property(d => d.StrInstructions).HasColumnName("Drink_StrInstructions");
                drink.Property(d => d.StrInstructionsES).HasColumnName("Drink_StrInstructionsES");
                drink.Property(d => d.StrInstructionsDE).HasColumnName("Drink_StrInstructionsDE");
                drink.Property(d => d.StrInstructionsFR).HasColumnName("Drink_StrInstructionsFR");
                drink.Property(d => d.StrInstructionsIT).HasColumnName("Drink_StrInstructionsIT");
                drink.Property(d => d.StrInstructionsZH_HANS).HasColumnName("Drink_StrInstructionsZH_HANS");
                drink.Property(d => d.StrInstructionsZH_HANT).HasColumnName("Drink_StrInstructionsZH_HANT");
                drink.Property(d => d.StrDrinkThumb).HasColumnName("Drink_StrDrinkThumb");
                drink.Property(d => d.StrIngredient1).HasColumnName("Drink_StrIngredient1");
                drink.Property(d => d.StrIngredient2).HasColumnName("Drink_StrIngredient2");
                drink.Property(d => d.StrIngredient3).HasColumnName("Drink_StrIngredient3");
                drink.Property(d => d.StrIngredient4).HasColumnName("Drink_StrIngredient4");
                drink.Property(d => d.StrIngredient5).HasColumnName("Drink_StrIngredient5");
                drink.Property(d => d.StrIngredient6).HasColumnName("Drink_StrIngredient6");
                drink.Property(d => d.StrIngredient7).HasColumnName("Drink_StrIngredient7");
                drink.Property(d => d.StrIngredient8).HasColumnName("Drink_StrIngredient8");
                drink.Property(d => d.StrIngredient9).HasColumnName("Drink_StrIngredient9");
                drink.Property(d => d.StrIngredient10).HasColumnName("Drink_StrIngredient10");
                drink.Property(d => d.StrIngredient11).HasColumnName("Drink_StrIngredient11");
                drink.Property(d => d.StrIngredient12).HasColumnName("Drink_StrIngredient12");
                drink.Property(d => d.StrIngredient13).HasColumnName("Drink_StrIngredient13");
                drink.Property(d => d.StrIngredient14).HasColumnName("Drink_StrIngredient14");
                drink.Property(d => d.StrIngredient15).HasColumnName("Drink_StrIngredient15");
                drink.Property(d => d.StrMeasure1).HasColumnName("Drink_StrMeasure1");
                drink.Property(d => d.StrMeasure2).HasColumnName("Drink_StrMeasure2");
                drink.Property(d => d.StrMeasure3).HasColumnName("Drink_StrMeasure3");
                drink.Property(d => d.StrMeasure4).HasColumnName("Drink_StrMeasure4");
                drink.Property(d => d.StrMeasure5).HasColumnName("Drink_StrMeasure5");
                drink.Property(d => d.StrMeasure6).HasColumnName("Drink_StrMeasure6");
                drink.Property(d => d.StrMeasure7).HasColumnName("Drink_StrMeasure7");
                drink.Property(d => d.StrMeasure8).HasColumnName("Drink_StrMeasure8");
                drink.Property(d => d.StrMeasure9).HasColumnName("Drink_StrMeasure9");
                drink.Property(d => d.StrMeasure10).HasColumnName("Drink_StrMeasure10");
                drink.Property(d => d.StrMeasure11).HasColumnName("Drink_StrMeasure11");
                drink.Property(d => d.StrMeasure12).HasColumnName("Drink_StrMeasure12");
                drink.Property(d => d.StrMeasure13).HasColumnName("Drink_StrMeasure13");
                drink.Property(d => d.StrMeasure14).HasColumnName("Drink_StrMeasure14");
                drink.Property(d => d.StrMeasure15).HasColumnName("Drink_StrMeasure15");
                drink.Property(d => d.StrImageSource).HasColumnName("Drink_StrImageSource");
                drink.Property(d => d.StrImageAttribution).HasColumnName("Drink_StrImageAttribution");
                drink.Property(d => d.StrCreativeCommonsConfirmed).HasColumnName("Drink_StrCreativeCommonsConfirmed");
                drink.Property(d => d.DateModified).HasColumnName("Drink_DateModified");
            });

        modelBuilder.Entity<Cocktail>()
            .Property(c => c.CreatedByUser)
            .HasColumnName("CreatedByUser");

        modelBuilder.Entity<Cocktail>()
            .Ignore(c => c.FavoriteByUsers);

        var intListComparer = new ValueComparer<ICollection<int>>(
            (c1, c2) => (c1 ?? Enumerable.Empty<int>()).SequenceEqual(c2 ?? Enumerable.Empty<int>()),
            c => (c ?? Enumerable.Empty<int>()).Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => (c ?? Enumerable.Empty<int>()).ToList()
        );

        modelBuilder.Entity<User>()
            .Property(u => u.FavoriteCocktails)
            .HasConversion(
                v => string.Join(',', v ?? Enumerable.Empty<int>()),
                v => (v ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            )
            .Metadata.SetValueComparer(intListComparer);

        base.OnModelCreating(modelBuilder);
    }
}