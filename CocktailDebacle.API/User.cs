using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public int[] BirthDate { get; set; } = new int[3];

    [MaxLength(30)]
    public string Country { get; set; } = string.Empty;

    [MaxLength(58)]
    public string City { get; set; } = string.Empty;

    public bool CanDrinkAlcohol { get; set; } = false;
    public bool AppPermissions { get; set; } = false;

    [Url]
    public string ImageUrl { get; set; } = string.Empty;

    [InverseProperty("FavoriteByUsers")]
    public ICollection<Cocktail> FavoriteCocktails { get; set; } = new List<Cocktail>();

    [InverseProperty("CreatedByUser")]
    public ICollection<Cocktail> CreatedCocktails { get; set; } = new List<Cocktail>();
}