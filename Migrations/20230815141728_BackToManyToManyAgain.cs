using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTrackerAPI.Migrations
{
    public partial class BackToManyToManyAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_CategoryId",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_CategoryId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "transactions");

            migrationBuilder.CreateTable(
                name: "CategoryTransaction",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    TransactionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTransaction", x => new { x.CategoriesId, x.TransactionsId });
                    table.ForeignKey(
                        name: "FK_CategoryTransaction_categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTransaction_transactions_TransactionsId",
                        column: x => x.TransactionsId,
                        principalTable: "transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTransaction_TransactionsId",
                table: "CategoryTransaction",
                column: "TransactionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTransaction");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_CategoryId",
                table: "transactions",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_CategoryId",
                table: "transactions",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
