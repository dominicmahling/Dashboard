using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddScriptOutput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScriptOutput",
                table: "ArticleFields",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScriptOutput",
                table: "ArticleFields");
        }
    }
}
