using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTrackerAPI.Migrations
{
    public partial class TransactionMerge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryExpenseItem");

            migrationBuilder.DropTable(
                name: "CategoryIncomeItem");

            migrationBuilder.DropTable(
                name: "expenseItems");

            migrationBuilder.DropTable(
                name: "incomeItems");

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                });

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

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.CreateTable(
                name: "expenseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenseItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "incomeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incomeItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryExpenseItem",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    ExpenseItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryExpenseItem", x => new { x.CategoriesId, x.ExpenseItemsId });
                    table.ForeignKey(
                        name: "FK_CategoryExpenseItem_categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryExpenseItem_expenseItems_ExpenseItemsId",
                        column: x => x.ExpenseItemsId,
                        principalTable: "expenseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryIncomeItem",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    IncomeItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryIncomeItem", x => new { x.CategoriesId, x.IncomeItemsId });
                    table.ForeignKey(
                        name: "FK_CategoryIncomeItem_categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryIncomeItem_incomeItems_IncomeItemsId",
                        column: x => x.IncomeItemsId,
                        principalTable: "incomeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryExpenseItem_ExpenseItemsId",
                table: "CategoryExpenseItem",
                column: "ExpenseItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryIncomeItem_IncomeItemsId",
                table: "CategoryIncomeItem",
                column: "IncomeItemsId");
        }
    }
}
