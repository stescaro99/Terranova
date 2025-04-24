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