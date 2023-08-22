using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTrackerAPI.Migrations
{
    public partial class DescAndListCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_expenseItems_categories_categoryId",
                table: "expenseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_incomeItems_categories_categoryId",
                table: "incomeItems");

            migrationBuilder.DropIndex(
                name: "IX_incomeItems_categoryId",
                table: "incomeItems");

            migrationBuilder.DropIndex(
                name: "IX_expenseItems_categoryId",
                table: "expenseItems");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "incomeItems");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "expenseItems");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "incomeItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "expenseItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Description",
                table: "incomeItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "expenseItems");

            migrationBuilder.DropColumn(
                name: "ExpenseItemId",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "IncomeItemId",
                table: "categories");

            migrationBuilder.AddColumn<int>(
                name: "categoryId",
                table: "incomeItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "categoryId",
                table: "expenseItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_incomeItems_categoryId",
                table: "incomeItems",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_expenseItems_categoryId",
                table: "expenseItems",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_expenseItems_categories_categoryId",
                table: "expenseItems",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_incomeItems_categories_categoryId",
                table: "incomeItems",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
