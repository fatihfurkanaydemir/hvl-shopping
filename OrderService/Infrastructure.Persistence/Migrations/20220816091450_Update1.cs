using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Infrastructure.Persistence.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Orders",
                newName: "TotalProductPrice");

            migrationBuilder.AddColumn<string>(
                name: "OrderGroupId",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ShipmentPrice",
                table: "Orders",
                type: "numeric(18,6)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderGroupId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipmentPrice",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "TotalProductPrice",
                table: "Orders",
                newName: "TotalPrice");
        }
    }
}
