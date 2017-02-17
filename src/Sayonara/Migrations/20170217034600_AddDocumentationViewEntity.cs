using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sayonara.Migrations
{
    public partial class AddDocumentationViewEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentationView",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FacilityID = table.Column<int>(nullable: false),
                    MedicalRecordCopy = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentationView", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DocumentationView_Facilities_FacilityID",
                        column: x => x.FacilityID,
                        principalTable: "Facilities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Extracts_FacilityID",
                table: "Extracts",
                column: "FacilityID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentationView_FacilityID",
                table: "DocumentationView",
                column: "FacilityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Extracts_Facilities_FacilityID",
                table: "Extracts",
                column: "FacilityID",
                principalTable: "Facilities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Extracts_Facilities_FacilityID",
                table: "Extracts");

            migrationBuilder.DropTable(
                name: "DocumentationView");

            migrationBuilder.DropIndex(
                name: "IX_Extracts_FacilityID",
                table: "Extracts");
        }
    }
}
