using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
public class FavoriteRequest
{
    public required string Username { get; set; }
    public required string CocktailId { get; set; }
}

public class UpdateUserRequest
{
    public required string Username { get; set; }
    public required string Field { get; set; }
    public required string Value { get; set; }
}

public class UploadRequest
{
    [FromForm]
    public string Nome { get; set; }

    [FromForm]
    public IFormFile Image { get; set; }
}

public class AddCocktailRequest
{
    public required CocktailApiDrink Drink { get; set; }
    public required string Username { get; set; }
    public required bool Private { get; set; }
    public required string Instructions { get; set; };
}