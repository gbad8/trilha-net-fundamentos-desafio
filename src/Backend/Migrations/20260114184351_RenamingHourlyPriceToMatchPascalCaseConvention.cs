using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trilha_net_fundamentos_desafio.Migrations
{
    /// <inheritdoc />
    public partial class RenamingHourlyPriceToMatchPascalCaseConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Hourlyprice",
                table: "Prices",
                newName: "HourlyPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HourlyPrice",
                table: "Prices",
                newName: "Hourlyprice");
        }
    }
}
