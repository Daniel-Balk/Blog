using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.App.Database.Migrations
{
    /// <inheritdoc />
    public partial class SimplifiedAModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "FTPFileOwners");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "FTPFileOwners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
