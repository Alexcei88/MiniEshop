using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniEshop.Migrations
{
    public partial class AddIndexToGoodTableImageUrlField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Goods",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_ImageUrl",
                table: "Goods",
                column: "ImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Goods_ImageUrl",
                table: "Goods");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Goods",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
