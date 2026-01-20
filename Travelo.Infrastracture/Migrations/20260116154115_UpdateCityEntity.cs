using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travelo.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCityEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Cities");
        }
    }
}
