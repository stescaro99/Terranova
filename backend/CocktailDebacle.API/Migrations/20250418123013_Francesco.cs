using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailDebacle.API.Migrations
{
    /// <inheritdoc />
    public partial class Francesco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cocktails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Drink_IdDrink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrDrink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrDrinkAlternate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIBA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrAlcoholic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrGlass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrInstructionsES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrInstructionsDE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrInstructionsFR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrInstructionsIT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrInstructionsZH_HANS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrInstructionsZH_HANT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrDrinkThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient15 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure15 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrImageSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrImageAttribution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrCreativeCommonsConfirmed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_DateModified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cocktails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    City = table.Column<string>(type: "nvarchar(58)", maxLength: 58, nullable: true),
                    CanDrinkAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    AppPermissions = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FavoriteCocktails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedCocktails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cocktails");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
