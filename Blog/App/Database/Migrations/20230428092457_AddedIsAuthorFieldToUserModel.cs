using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.App.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsAuthorFieldToUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAuthor",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAuthor",
                table: "Users");
        }
    }
}
