public class FavoriteRequest
{
    public required string Username { get; set; }
    public required string CocktailId { get; set; }
}

public class UpdateUserRequest
{
    public required int Id;
    public required string Field;
    public required string Value;
}

public class ImageUploadRequest
{
    public required string FileName { get; set; }
    public required IFormFile Image { get; set; }
}

public class AddCocktailRequest
{
    public required CocktailApiDrink Drink { get; set; }
    public required string Username { get; set; }
    public required bool Private { get; set; }
}