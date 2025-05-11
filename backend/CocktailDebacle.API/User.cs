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

	[InverseProperty("FavoriteByUsers")]
	public ICollection<int>? FavoriteCocktails { get; set; } = new List<int>();

	[InverseProperty("CreatedByUser")]
	public ICollection<int>? CreatedCocktails { get; set; } = new List<int>();

	public string Language { get; set; } = "en";
}





/*
appsettings.json without dockerfiles

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CocktailDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=true"
  },
  "DeepSeek": {
    "ApiKey": "AIzaSyCN7C0eyh9psWs7iOgVuDLxV415ly39Xxw",
    "Endpoint": "https://api.deepseek.com/chat/completions"
  }
}*/