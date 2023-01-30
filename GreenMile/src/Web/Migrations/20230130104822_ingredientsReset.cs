using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class ingredientsReset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Donations");

            migrationBuilder.RenameColumn(
                name: "ExpiredDate",
                table: "CustomFoods",
                newName: "ExpiryDate");

            migrationBuilder.AlterColumn<string>(
                name: "difficulty",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)");

            migrationBuilder.AddColumn<double>(
                name: "CarbonFootprint",
                table: "FoodItems",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustom",
                table: "FoodItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Donations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DonationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DonationId = table.Column<int>(type: "INTEGER", nullable: true),
                    DonorId = table.Column<string>(type: "TEXT", nullable: true),
                    RecipientId = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonationRequests_AspNetUsers_DonorId",
                        column: x => x.DonorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonationRequests_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonationRequests_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroceryFood",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ExtraNote = table.Column<string>(type: "TEXT", nullable: true),
                    InBasket = table.Column<bool>(type: "INTEGER", nullable: false),
                    HouseholdId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CarbonFootprint = table.Column<double>(type: "REAL", nullable: true),
                    ImageFilePath = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroceryFood", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroceryFood_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroceryFood_Household_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Household",
                        principalColumn: "HouseholdId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_DonationId",
                table: "DonationRequests",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_DonorId",
                table: "DonationRequests",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequests_RecipientId",
                table: "DonationRequests",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryFood_CategoryId",
                table: "GroceryFood",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryFood_HouseholdId",
                table: "GroceryFood",
                column: "HouseholdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonationRequests");

            migrationBuilder.DropTable(
                name: "GroceryFood");

            migrationBuilder.DropColumn(
                name: "CarbonFootprint",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "IsCustom",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "CustomFoods",
                newName: "ExpiredDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "difficulty",
                table: "Recipes",
                type: "decimal(2,1)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Donations",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Donations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
