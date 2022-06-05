using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Consumer.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    AmountAvailable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Code", "AmountAvailable", "Description", "Name", "Price" },
                values: new object[] { 1234, 25, "White t shirt", "T Shirt", 5.0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Code", "AmountAvailable", "Description", "Name", "Price" },
                values: new object[] { 5678, 30, "Blue Jeans", "Jeans", 10.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
