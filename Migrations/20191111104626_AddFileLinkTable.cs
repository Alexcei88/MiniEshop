using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniEshop.Migrations
{
    public partial class AddFileLinkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Goods_ImageUrl",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Goods");

            migrationBuilder.AddColumn<Guid>(
                name: "FileLinkId",
                table: "Goods",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageUrlId",
                table: "Goods",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateTable(
                name: "FileLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileLinks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goods_FileLinkId",
                table: "Goods",
                column: "FileLinkId",
                unique: false,
                filter: "[FileLinkId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FileLinks_FileUrl",
                table: "FileLinks",
                column: "FileUrl");

            migrationBuilder.AddForeignKey(
                name: "FK_Goods_FileLinks_FileLinkId",
                table: "Goods",
                column: "FileLinkId",
                principalTable: "FileLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_FileLinks_FileLinkId",
                table: "Goods");

            migrationBuilder.DropTable(
                name: "FileLinks");

            migrationBuilder.DropIndex(
                name: "IX_Goods_FileLinkId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "FileLinkId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "ImageUrlId",
                table: "Goods");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Goods",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_ImageUrl",
                table: "Goods",
                column: "ImageUrl");
        }
    }
}
