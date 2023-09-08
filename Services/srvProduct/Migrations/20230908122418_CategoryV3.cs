using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace srvProduct.Migrations
{
    /// <inheritdoc />
    public partial class CategoryV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "tblSubCategoryDetail",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDt",
                table: "tblSubCategoryDetail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "tblCategoryDetail",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDt",
                table: "tblCategoryDetail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tblSubCategoryDetail");

            migrationBuilder.DropColumn(
                name: "CreatedDt",
                table: "tblSubCategoryDetail");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tblCategoryDetail");

            migrationBuilder.DropColumn(
                name: "CreatedDt",
                table: "tblCategoryDetail");
        }
    }
}
