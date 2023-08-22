using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTrackerAPI.Migrations
{
    public partial class ManyToManyTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_expenseItems_ExpenseItemId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_categories_incomeItems_IncomeItemId",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_ExpenseItemId",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_IncomeItemId",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "ExpenseItemId",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "IncomeItemId",
                table: "categories");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryExpenseItem");

            migrationBuilder.DropTable(
                name: "CategoryIncomeItem");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseItemId",
                table: "categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomeItemId",
                table: "categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_ExpenseItemId",
                table: "categories",
                column: "ExpenseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_IncomeItemId",
                table: "categories",
                column: "IncomeItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_expenseItems_ExpenseItemId",
                table: "categories",
                column: "ExpenseItemId",
                principalTable: "expenseItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_incomeItems_IncomeItemId",
                table: "categories",
                column: "IncomeItemId",
                principalTable: "incomeItems",
                principalColumn: "Id");
        }
    }
}
