using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    BrandID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.BrandID);
                });

            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    DriverID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    NationalInsuranceNr = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    VehicleID = table.Column<int>(nullable: true),
                    FuelCardID = table.Column<int>(nullable: true),
                    AddressID = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.DriverID);
                });

            migrationBuilder.CreateTable(
                name: "DriverLicenseType",
                columns: table => new
                {
                    DriverLicenseTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLicenseType", x => x.DriverLicenseTypeID);
                });

            migrationBuilder.CreateTable(
                name: "FuelType",
                columns: table => new
                {
                    FuelTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelType", x => x.FuelTypeID);
                });

            migrationBuilder.CreateTable(
                name: "VehicleType",
                columns: table => new
                {
                    VehicleTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleType", x => x.VehicleTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressID = table.Column<int>(nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Place = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Zipcode = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Address_Driver_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Driver",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelCard",
                columns: table => new
                {
                    FuelCardID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    Pincode = table.Column<int>(nullable: true),
                    DriverID = table.Column<int>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelCard", x => x.FuelCardID);
                    table.ForeignKey(
                        name: "FK_FuelCard_Driver_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Driver",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverDriverLicenseType",
                columns: table => new
                {
                    DriverID = table.Column<int>(nullable: false),
                    DriverLicenseTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverDriverLicenseType", x => new { x.DriverID, x.DriverLicenseTypeID });
                    table.ForeignKey(
                        name: "FK_DriverDriverLicenseType_Driver_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Driver",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverDriverLicenseType_DriverLicenseType_DriverLicenseTypeID",
                        column: x => x.DriverLicenseTypeID,
                        principalTable: "DriverLicenseType",
                        principalColumn: "DriverLicenseTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    VehicleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Vin = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    BrandID = table.Column<int>(nullable: false),
                    VehicleTypeID = table.Column<int>(nullable: false),
                    FuelTypeID = table.Column<int>(nullable: false),
                    Color = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AmountDoors = table.Column<int>(nullable: true),
                    DriverID = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.VehicleID);
                    table.ForeignKey(
                        name: "FK_Vehicle_Brand_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brand",
                        principalColumn: "BrandID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicle_Driver_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Driver",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_FuelType_FuelTypeID",
                        column: x => x.FuelTypeID,
                        principalTable: "FuelType",
                        principalColumn: "FuelTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleType_VehicleTypeID",
                        column: x => x.VehicleTypeID,
                        principalTable: "VehicleType",
                        principalColumn: "VehicleTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelCardFuelType",
                columns: table => new
                {
                    FuelCardID = table.Column<int>(nullable: false),
                    FuelTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelCardFuelType", x => new { x.FuelCardID, x.FuelTypeID });
                    table.ForeignKey(
                        name: "FK_FuelCardFuelType_FuelCard_FuelCardID",
                        column: x => x.FuelCardID,
                        principalTable: "FuelCard",
                        principalColumn: "FuelCardID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuelCardFuelType_FuelType_FuelTypeID",
                        column: x => x.FuelTypeID,
                        principalTable: "FuelType",
                        principalColumn: "FuelTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverDriverLicenseType_DriverLicenseTypeID",
                table: "DriverDriverLicenseType",
                column: "DriverLicenseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FuelCard_DriverID",
                table: "FuelCard",
                column: "DriverID",
                unique: true,
                filter: "[DriverID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FuelCardFuelType_FuelTypeID",
                table: "FuelCardFuelType",
                column: "FuelTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_BrandID",
                table: "Vehicle",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_DriverID",
                table: "Vehicle",
                column: "DriverID",
                unique: true,
                filter: "[DriverID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_FuelTypeID",
                table: "Vehicle",
                column: "FuelTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_VehicleTypeID",
                table: "Vehicle",
                column: "VehicleTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "DriverDriverLicenseType");

            migrationBuilder.DropTable(
                name: "FuelCardFuelType");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "DriverLicenseType");

            migrationBuilder.DropTable(
                name: "FuelCard");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "FuelType");

            migrationBuilder.DropTable(
                name: "VehicleType");

            migrationBuilder.DropTable(
                name: "Driver");
        }
    }
}
