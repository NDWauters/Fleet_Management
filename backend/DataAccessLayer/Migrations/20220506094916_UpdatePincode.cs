using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class UpdatePincode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Pincode",
                table: "FuelCard",
                type: "nvarchar(4)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Pincode",
                table: "FuelCard",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldNullable: true);
        }
    }
}
