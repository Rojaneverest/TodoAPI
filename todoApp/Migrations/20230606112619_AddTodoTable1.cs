using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTodoTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Task",
                table: "Todos",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Todos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Todos",
                newName: "Task");
        }
    }
}
