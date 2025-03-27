using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailDebacle.API.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    City = table.Column<string>(type: "nvarchar(58)", maxLength: 58, nullable: false),
                    CanDrinkAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    AppPermissions = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cocktails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Drink_IdDrink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrDrink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrGlass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrDrinkThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrIngredient6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drink_StrMeasure6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cocktails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cocktails_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserFavoriteCocktails",
                columns: table => new
                {
                    FavoriteByUsersId = table.Column<int>(type: "int", nullable: false),
                    FavoriteCocktailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteCocktails", x => new { x.FavoriteByUsersId, x.FavoriteCocktailsId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteCocktails_Cocktails_FavoriteCocktailsId",
                        column: x => x.FavoriteCocktailsId,
                        principalTable: "Cocktails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteCocktails_Users_FavoriteByUsersId",
                        column: x => x.FavoriteByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cocktails_CreatedByUserId",
                table: "Cocktails",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteCocktails_FavoriteCocktailsId",
                table: "UserFavoriteCocktails",
                column: "FavoriteCocktailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteCocktails");

            migrationBuilder.DropTable(
                name: "Cocktails");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
