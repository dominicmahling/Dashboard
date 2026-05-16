using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddStepScriptSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "WorkflowSteps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScriptOutput",
                table: "WorkflowSteps",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Script",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "ScriptOutput",
                table: "WorkflowSteps");
        }
    }
}
