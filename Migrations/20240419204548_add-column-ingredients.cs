using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kuchenne_rewolucje.Migrations
{
    /// <inheritdoc />
    public partial class addcolumningredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ingredients",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ingredients",
                table: "Articles");
        }
    }
}
