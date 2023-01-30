using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class hda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_CustomFoods_CustomFoodId",
                table: "Donations");

            migrationBuilder.AlterColumn<int>(
                name: "CustomFoodId",
                table: "Donations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Donations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "CustomFoods",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_CustomFoods_CustomFoodId",
                table: "Donations",
                column: "CustomFoodId",
                principalTable: "CustomFoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_CustomFoods_CustomFoodId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Donations");

            migrationBuilder.AlterColumn<int>(
                name: "CustomFoodId",
                table: "Donations",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "CustomFoods",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_CustomFoods_CustomFoodId",
                table: "Donations",
                column: "CustomFoodId",
                principalTable: "CustomFoods",
                principalColumn: "Id");
        }
    }
}
