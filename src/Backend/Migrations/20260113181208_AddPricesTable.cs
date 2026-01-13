using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace trilha_net_fundamentos_desafio.Migrations
{
    /// <inheritdoc />
    public partial class AddPricesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EffectiveHourlyPrice",
                table: "Veiculos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Type = table.Column<int>(type: "int", nullable: false),
                    Hourlyprice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Type);
                });

            migrationBuilder.InsertData(
                table: "Prices",
                columns: new[] { "Type", "Hourlyprice" },
                values: new object[,]
                {
                    { 0, 10.00m },
                    { 1, 5.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Type",
                table: "Veiculos",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_Prices_Type",
                table: "Veiculos",
                column: "Type",
                principalTable: "Prices",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_Prices_Type",
                table: "Veiculos");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Type",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "EffectiveHourlyPrice",
                table: "Veiculos");
        }
    }
}
