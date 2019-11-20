using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeRentalAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bike",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(maxLength: 25, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    Notes = table.Column<string>(maxLength: 1000, nullable: true),
                    LastService = table.Column<DateTime>(type: "date", nullable: false),
                    PriceFirstHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceAdditionalHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BikeCategory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bike", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gender = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 75, nullable: false),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false),
                    Street = table.Column<string>(maxLength: 75, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 10, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: false),
                    Town = table.Column<string>(maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rental",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(nullable: false),
                    BikeID = table.Column<int>(nullable: false),
                    Begin = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    TotalCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Paid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rental", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rental_Bike_BikeID",
                        column: x => x.BikeID,
                        principalTable: "Bike",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rental_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rental_BikeID",
                table: "Rental",
                column: "BikeID");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_CustomerID",
                table: "Rental",
                column: "CustomerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rental");

            migrationBuilder.DropTable(
                name: "Bike");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
