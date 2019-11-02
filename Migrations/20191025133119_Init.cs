using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.SqlServer.Types;

namespace MiniEshop.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    HierarchyId = table.Column<SqlHierarchyId>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Goods",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(38, 20)", nullable: false),
                    Qty = table.Column<int>(nullable: false, defaultValue: 0 ),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goods_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goods_CategoryId",
                table: "Goods",
                column: "CategoryId");

            Seed(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Goods");

            migrationBuilder.DropTable(
                name: "Categories");
        }

        private void Seed(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT [dbo].[Categories] ([Id], [Name], [ParentId], [HierarchyId]) VALUES (N'00000000-0000-0000-0000-000000000000', N'', N'00000000-0000-0000-0000-000000000000', N'/')
INSERT [dbo].[Categories] ([Id], [Name], [ParentId], [HierarchyId]) VALUES (N'93a42b88-12e5-4084-b26f-27982be3ff9c', N'Мужская одежда', N'2e5c1158-343c-41de-8b08-aa61abe4fed0', N'/1/2/')
INSERT [dbo].[Categories] ([Id], [Name], [ParentId], [HierarchyId]) VALUES (N'fa83a1fe-fcc1-42dd-9e41-50ee3e3cd885', N'Верхняя', N'7678a967-4e75-400b-8900-f3a9d8fdb482', N'/1/1/1/')
INSERT [dbo].[Categories] ([Id], [Name], [ParentId], [HierarchyId]) VALUES (N'14524be1-8220-43c6-b40d-6f0b05b2350d', N'Спортивная одежда', N'7678a967-4e75-400b-8900-f3a9d8fdb482', N'/1/1/2/')
INSERT [dbo].[Categories] ([Id], [Name], [ParentId], [HierarchyId]) VALUES (N'b9b728d9-630b-4a5e-967a-88620b778066', N'Верхняя', N'93a42b88-12e5-4084-b26f-27982be3ff9c', N'/1/1/1/')
INSERT [dbo].[Categories] ([Id], [Name], [ParentId], [HierarchyId]) VALUES (N'2e5c1158-343c-41de-8b08-aa61abe4fed0', N'Одежда, обувь, аксессуары', N'00000000-0000-0000-0000-000000000000', N'/1/')
INSERT [dbo].[Categories] ([Id], [Name], [ParentId], [HierarchyId]) VALUES (N'7678a967-4e75-400b-8900-f3a9d8fdb482', N'Женская одежда', N'2e5c1158-343c-41de-8b08-aa61abe4fed0', N'/1/1/')
INSERT [dbo].[Categories] ([Id], [Name], [ParentId], [HierarchyId]) VALUES (N'0df07a0d-fb78-43e8-904e-f5db9e25e34d', N'Спортивная одежда', N'93a42b88-12e5-4084-b26f-27982be3ff9c', N'/1/1/3/')
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'f35d601f-7507-4bdd-b63f-02260729a234', N'Пуховик', CAST(320.00000000000000000000 AS Decimal(38, 20)), N'7678a967-4e75-400b-8900-f3a9d8fdb482', 1)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'cb9e882c-b1d5-421a-a214-02c0c545a2f0', N'Кроссовки', CAST(60.00000000000000000000 AS Decimal(38, 20)), N'0df07a0d-fb78-43e8-904e-f5db9e25e34d', 2)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'46c30d6d-8257-4ac9-a3f2-21fe148fd862', N'Шапка', CAST(24.00000000000000000000 AS Decimal(38, 20)), N'7678a967-4e75-400b-8900-f3a9d8fdb482', 3)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'c332103f-dd27-4f66-ac6a-3bbc0767096f', N'Шорты', CAST(210.00000000000000000000 AS Decimal(38, 20)), N'14524be1-8220-43c6-b40d-6f0b05b2350d', 4)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'852cfb37-4c40-46a2-8989-40aa88ff34d0', N'Пуховик', CAST(25.00000000000000000000 AS Decimal(38, 20)), N'b9b728d9-630b-4a5e-967a-88620b778066', 6)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'334c360c-529f-477a-b88f-76746cb51344', N'Шорты', CAST(21.00000000000000000000 AS Decimal(38, 20)), N'0df07a0d-fb78-43e8-904e-f5db9e25e34d', 8)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'57b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки универсальные', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 10)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'8bbead68-ceea-422f-aaf4-bbab9011a597', N'Шапка', CAST(25.00000000000000000000 AS Decimal(38, 20)), N'b9b728d9-630b-4a5e-967a-88620b778066', 12)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'1d3f295e-55a6-4d8c-96ef-c3accdd47c91', N'Кроссовки', CAST(120.00000000000000000000 AS Decimal(38, 20)), N'14524be1-8220-43c6-b40d-6f0b05b2350d', 14)

INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'07b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки серые', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 10)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'17b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки зеленые', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 10)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'27b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки красные', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 11)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'37b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки черные', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 12)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'47b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки белые', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 13)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'67b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки желтые', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 10)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'77b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки голубые', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 18)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'87b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки фиолетовые', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 10)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'97b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки оранжевые', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 6)
INSERT [dbo].[Goods] ([Id], [Name], [Price], [CategoryId], [Qty]) VALUES (N'51b7572c-51a3-423b-9508-9ac1508310d0', N'Валенки коричневые', CAST(200.00000000000000000000 AS Decimal(38, 20)), N'2e5c1158-343c-41de-8b08-aa61abe4fed0', 10)




");
        }
    }
}
