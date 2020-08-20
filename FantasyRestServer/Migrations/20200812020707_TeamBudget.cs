using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyRestServer.Migrations
{
    public partial class TeamBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableBudget",
                table: "Teams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableBudget",
                table: "Teams");
        }
    }
}
