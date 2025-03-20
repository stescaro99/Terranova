using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Cocktail
{
	public int Id { get; set; }

	[Required]
	[MaxLength(30)]
	public string Name { get; set; } = string.Empty;

	[Required]
	public string Ingredients { get; set; } = string.Empty;

	[Required]
	[MaxLength(20)]
	public string GlassType { get; set; } = string.Empty;

	[Required]
	[MaxLength(20)]
	public string Category { get; set; } = string.Empty;

	[Url]
	public string? ImageUrl { get; set; } = string.Empty;
	//usare array di byte nel database per immagini al posto dell'url
	//public byte[]? ImageData { get; set; };
	//oppure usare un path locale e salvare le immagini in directory sul server
	//public string ImagePath { get; set; } = string.Empty;

	[ForeignKey("CreatedByUserId")]
	public User? CreatedByUser { get; set; }
	public int? CreatedByUserId { get; set; }

	[InverseProperty("FavoriteCocktails")]
	public ICollection<User> FavoriteByUsers { get; set; } = new List<User>();
	// prezzo medio da cercare in base al paese/citta' (non presente nell'API, collegare un'AI?)
	// public string Price { get; set; } = string.Empty;
}