using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartacfe.Migrations
{
    public partial class notifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACDeviceAlerts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ACDeviceID = table.Column<int>(nullable: false),
                    AlertType = table.Column<int>(nullable: false),
                    ReadingDateTime = table.Column<DateTime>(nullable: false),
                    AlertDateTime = table.Column<DateTime>(nullable: false),
                    Cleared = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACDeviceAlerts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ACDeviceAlerts_ACDevices_ACDeviceID",
                        column: x => x.ACDeviceID,
                        principalTable: "ACDevices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACDeviceAlerts_ACDeviceID",
                table: "ACDeviceAlerts",
                column: "ACDeviceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACDeviceAlerts");
        }
    }
}
