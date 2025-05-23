using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


public class Cocktail
{
    [Key]
    public int Id { get; set; }

    [Required]
    public CocktailApiDrink Drink { get; set; } = new();

    [ForeignKey("CreatedByUser")]
    public string? CreatedByUser { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<string> FavoriteByUsers { get; set; } = new List<string>();

    public bool isPrivate { get; set; } = false;
}

public class CocktailApiDrink
{
    [JsonPropertyName("idDrink")]
    public string? IdDrink { get; set; }

    [JsonPropertyName("strDrink")]
    public string? StrDrink { get; set; }

    [JsonPropertyName("strDrinkAlternate")]
    public string? StrDrinkAlternate { get; set; }

    [JsonPropertyName("strTags")]
    public string? StrTags { get; set; }

    [JsonPropertyName("strVideo")]
    public string? StrVideo { get; set; }

    [JsonPropertyName("strCategory")]
    public string? StrCategory { get; set; }

    [JsonPropertyName("strIBA")]
    public string? StrIBA { get; set; }

    [JsonPropertyName("strAlcoholic")]
    public string? StrAlcoholic { get; set; }

    [JsonPropertyName("strGlass")]
    public string? StrGlass { get; set; }

    [JsonPropertyName("strInstructions")]
    public string? StrInstructions { get; set; }

    [JsonPropertyName("strInstructionsES")]
    public string? StrInstructionsES { get; set; }

    [JsonPropertyName("strInstructionsDE")]
    public string? StrInstructionsDE { get; set; }

    [JsonPropertyName("strInstructionsFR")]
    public string? StrInstructionsFR { get; set; }

    [JsonPropertyName("strInstructionsIT")]
    public string? StrInstructionsIT { get; set; }

    [JsonPropertyName("strInstructionsZH-HANS")]
    public string? StrInstructionsZH_HANS { get; set; }

    [JsonPropertyName("strInstructionsZH-HANT")]
    public string? StrInstructionsZH_HANT { get; set; }

    [JsonPropertyName("strDrinkThumb")]
    public string? StrDrinkThumb { get; set; }

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

    [JsonPropertyName("strIngredient7")]
    public string? StrIngredient7 { get; set; }

    [JsonPropertyName("strIngredient8")]
    public string? StrIngredient8 { get; set; }

    [JsonPropertyName("strIngredient9")]
    public string? StrIngredient9 { get; set; }

    [JsonPropertyName("strIngredient10")]
    public string? StrIngredient10 { get; set; }

    [JsonPropertyName("strIngredient11")]
    public string? StrIngredient11 { get; set; }

    [JsonPropertyName("strIngredient12")]
    public string? StrIngredient12 { get; set; }

    [JsonPropertyName("strIngredient13")]
    public string? StrIngredient13 { get; set; }

    [JsonPropertyName("strIngredient14")]
    public string? StrIngredient14 { get; set; }

    [JsonPropertyName("strIngredient15")]
    public string? StrIngredient15 { get; set; }

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

    [JsonPropertyName("strMeasure7")]
    public string? StrMeasure7 { get; set; }

    [JsonPropertyName("strMeasure8")]
    public string? StrMeasure8 { get; set; }

    [JsonPropertyName("strMeasure9")]
    public string? StrMeasure9 { get; set; }

    [JsonPropertyName("strMeasure10")]
    public string? StrMeasure10 { get; set; }

    [JsonPropertyName("strMeasure11")]
    public string? StrMeasure11 { get; set; }

    [JsonPropertyName("strMeasure12")]
    public string? StrMeasure12 { get; set; }

    [JsonPropertyName("strMeasure13")]
    public string? StrMeasure13 { get; set; }

    [JsonPropertyName("strMeasure14")]
    public string? StrMeasure14 { get; set; }

    [JsonPropertyName("strMeasure15")]
    public string? StrMeasure15 { get; set; }

    [JsonPropertyName("strImageSource")]
    public string? StrImageSource { get; set; }

    [JsonPropertyName("strImageAttribution")]
    public string? StrImageAttribution { get; set; }

    [JsonPropertyName("strCreativeCommonsConfirmed")]
    public string? StrCreativeCommonsConfirmed { get; set; }

    [JsonPropertyName("dateModified")]
    public string? DateModified { get; set; }
}