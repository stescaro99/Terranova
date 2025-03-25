using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


public class Cocktail
{
    [Key]
    public int Id { get; set; }

    [Required]
    public CocktailApiDrink Drink { get; set; } = new();

    [ForeignKey("CreatedByUserId")]
    public User? CreatedByUser { get; set; }
    public int? CreatedByUserId { get; set; }

    [InverseProperty("FavoriteCocktails")]
    public ICollection<User> FavoriteByUsers { get; set; } = new List<User>();
}

public class CocktailApiDrink
{
    [JsonPropertyName("IdDrink")]
    public string? IdDrink { get; set; }

    [JsonPropertyName("strDrink")]
    public string? StrDrink { get; set; }

    [JsonPropertyName("strGlass")]
    public string? StrGlass { get; set; }

    [JsonPropertyName("strCategory")]
    public string? StrCategory { get; set; }

    [JsonPropertyName("strDrinkThumb")]
    public string? StrDrinkThumb { get; set; }

    [JsonPropertyName("strInstructions")]
    public string? StrInstructions { get; set; }

    [JsonPropertyName("strIngredient1")]
    public string? StrIngredient1 { get; set; }

    [JsonPropertyName("strIngredient2")]
    public string? StrIngredient2 { get; set; }

    [JsonPropertyName("strIngredient3")]
    public string? StrIngredient3 { get; set; }

    [JsonPropertyName("strIngredient4")]
    public string? StrIngredient4 { get; set; }

    [JsonPropertyName("strIngredient5")]
    public string? StrIngredient5 { get; set; }

    [JsonPropertyName("strIngredient6")]
    public string? StrIngredient6 { get; set; }

    [JsonPropertyName("strMeasure1")]
    public string? StrMeasure1 { get; set; }

    [JsonPropertyName("strMeasure2")]
    public string? StrMeasure2 { get; set; }

    [JsonPropertyName("strMeasure3")]
    public string? StrMeasure3 { get; set; }

    [JsonPropertyName("strMeasure4")]
    public string? StrMeasure4 { get; set; }

    [JsonPropertyName("strMeasure5")]
    public string? StrMeasure5 { get; set; }

    [JsonPropertyName("strMeasure6")]
    public string? StrMeasure6 { get; set; }
}