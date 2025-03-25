using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailDebacle.API.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureDrinkMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CocktailIngredient");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "GlassType",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cocktails");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Cocktails",
                newName: "Drink_StrMeasure6");

            migrationBuilder.AddColumn<string>(
                name: "Drink_IdDrink",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrCategory",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrDrink",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrDrinkThumb",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrGlass",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrIngredient1",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrIngredient2",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrIngredient3",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrIngredient4",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrIngredient5",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrIngredient6",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrInstructions",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrMeasure1",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrMeasure2",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrMeasure3",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrMeasure4",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Drink_StrMeasure5",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Drink_IdDrink",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrCategory",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrDrink",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrDrinkThumb",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrGlass",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrIngredient1",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrIngredient2",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrIngredient3",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrIngredient4",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrIngredient5",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrIngredient6",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrInstructions",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrMeasure1",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrMeasure2",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrMeasure3",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrMeasure4",
                table: "Cocktails");

            migrationBuilder.DropColumn(
                name: "Drink_StrMeasure5",
                table: "Cocktails");

            migrationBuilder.RenameColumn(
                name: "Drink_StrMeasure6",
                table: "Cocktails",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Cocktails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GlassType",
                table: "Cocktails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cocktails",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CocktailIngredient",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Measure = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CocktailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CocktailIngredient", x => new { x.Name, x.Measure });
                    table.ForeignKey(
                        name: "FK_CocktailIngredient_Cocktails_CocktailId",
                        column: x => x.CocktailId,
                        principalTable: "Cocktails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CocktailIngredient_CocktailId",
                table: "CocktailIngredient",
                column: "CocktailId");
        }
    }
}
