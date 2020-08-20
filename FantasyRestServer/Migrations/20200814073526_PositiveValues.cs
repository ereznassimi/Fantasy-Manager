using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyRestServer.Migrations
{
    public partial class PositiveValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "AskingPrice",
                table: "Transfers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "TotalValue",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "AvailableBudget",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "AmountInTeam",
                table: "Positions",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "MarketValue",
                table: "Players",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "Age",
                table: "Players",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 1,
                column: "AmountInTeam",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 2,
                column: "AmountInTeam",
                value: 6L);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 3,
                column: "AmountInTeam",
                value: 6L);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 4,
                column: "AmountInTeam",
                value: 5L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AskingPrice",
                table: "Transfers",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "TotalValue",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "AvailableBudget",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "AmountInTeam",
                table: "Positions",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "MarketValue",
                table: "Players",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Players",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 1,
                column: "AmountInTeam",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 2,
                column: "AmountInTeam",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 3,
                column: "AmountInTeam",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 4,
                column: "AmountInTeam",
                value: 5);
        }
    }
}
