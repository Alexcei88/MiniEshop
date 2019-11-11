using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniEshop.Migrations
{
    public partial class FixLinkToFileInGoodTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_FileLinks_FileLinkId",
                table: "Goods");

            migrationBuilder.DropIndex(
                name: "IX_Goods_FileLinkId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "FileLinkId",
                table: "Goods");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_ImageUrlId",
                table: "Goods",
                column: "ImageUrlId",
                unique: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Goods_FileLinks_ImageUrlId",
                table: "Goods",
                column: "ImageUrlId",
                principalTable: "FileLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_FileLinks_ImageUrlId",
                table: "Goods");

            migrationBuilder.DropIndex(
                name: "IX_Goods_ImageUrlId",
                table: "Goods");

            migrationBuilder.AddColumn<Guid>(
                name: "FileLinkId",
                table: "Goods",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_FileLinkId",
                table: "Goods",
                column: "FileLinkId",
                unique: true,
                filter: "[FileLinkId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Goods_FileLinks_FileLinkId",
                table: "Goods",
                column: "FileLinkId",
                principalTable: "FileLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
