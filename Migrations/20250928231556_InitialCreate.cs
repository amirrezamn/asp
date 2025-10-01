using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace asp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Todos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Selected",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "General" },
                    { 2, "Work" },
                    { 3, "Personal" },
                    { 4, "Shopping" }
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Category", "Completed", "Description", "Selected", "Title" },
                values: new object[,]
                {
                    { 1L, "Shopping", false, "From supermarket", false, "Buy milk" },
                    { 2L, "Work", false, "Quarterly report", false, "Finish report" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Selected",
                table: "Todos");
        }
    }
}
