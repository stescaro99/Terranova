
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public int[] BirthDate { get; set; } = new int[3];
	public string Country { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public bool CanDrinkAlcohol { get; set; } = false;
	public bool AppPermissions { get; set; } = false;
	public string ImageUrl { get; set; } = string.Empty;
	public Cocktail[] FavoriteCocktails { get; set; } = new Cocktail[0];
	public Cocktail[] CreatedCocktails { get; set; } = new Cocktail[0];
}