using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailDebacle.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCocktailSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cocktails",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Cocktails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Cocktails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GlassType",
                table: "Cocktails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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

            migrationBuilder.AddForeignKey(
                name: "FK_Cocktails_Users_CreatedByUserId",
                table: "Cocktails",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cocktails_Users_CreatedByUserId",
                table: "Cocktails");

            migrationBuilder.DropTable(
                name: "UserFavoriteCocktails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Cocktails_CreatedByUserId",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "GlassType",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cocktails");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
