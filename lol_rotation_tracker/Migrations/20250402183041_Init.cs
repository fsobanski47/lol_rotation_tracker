using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lol_rotation_tracker.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Versions",
                columns: table => new
                {
                    Version = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versions", x => x.Version);
                });

            migrationBuilder.CreateTable(
                name: "Champions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    GameVersionVersion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Champions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Champions_Versions_GameVersionVersion",
                        column: x => x.GameVersionVersion,
                        principalTable: "Versions",
                        principalColumn: "Version");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Champions_GameVersionVersion",
                table: "Champions",
                column: "GameVersionVersion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Champions");

            migrationBuilder.DropTable(
                name: "Versions");
        }
    }
}
