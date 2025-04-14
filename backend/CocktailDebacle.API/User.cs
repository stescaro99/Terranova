using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
	public int Id { get; set; }

	[Required]
	[MaxLength(50)]
	public string Name { get; set; } = string.Empty;

	[Required]
	[MaxLength(20)]
	public string Username { get; set; } = string.Empty;

	[Required]
	public string Email { get; set; } = string.Empty;

	[Required]
	public string Password { get; set; } = string.Empty;

	[Required]
	public string BirthDate { get; set; } = string.Empty;

	[MaxLength(30)]
	public string? Country { get; set; } = string.Empty;

	[MaxLength(58)]
	public string? City { get; set; } = string.Empty;

	public bool CanDrinkAlcohol { get; set; } = false;
	public bool AppPermissions { get; set; } = false;

	public string? ImageUrl { get; set; } = string.Empty;

	[InverseProperty("FavoriteByUsers")] //id dei cocktail preferiti
	public ICollection<int>? FavoriteCocktails { get; set; } = null;

	[InverseProperty("CreatedByUser")] //id dei cocktail creati
	public ICollection<int>? CreatedCocktails { get; set; } = null;
}