using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace srvProduct.Migrations
{
    /// <inheritdoc />
    public partial class CategoryV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tblSubCategoryDetail",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tblProperty",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tblCategoryDetail",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tblSubCategoryDetail");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tblProperty");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tblCategoryDetail");
        }
    }
}
