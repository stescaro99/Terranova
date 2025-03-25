using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailDebacle.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCocktailIngredientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ingredients",
                table: "Cocktails");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CocktailIngredient");

            migrationBuilder.AddColumn<string>(
                name: "Ingredients",
                table: "Cocktails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
