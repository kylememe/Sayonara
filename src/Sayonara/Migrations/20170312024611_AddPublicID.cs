using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sayonara.Migrations
{
    public partial class AddPublicID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentationView_Facilities_FacilityID",
                table: "DocumentationView");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentationView",
                table: "DocumentationView");

            migrationBuilder.RenameTable(
                name: "DocumentationView",
                newName: "DocumentationViews");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentationView_FacilityID",
                table: "DocumentationViews",
                newName: "IX_DocumentationViews_FacilityID");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentationViewID",
                table: "Extracts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PublicID",
                table: "Extracts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentationViews",
                table: "DocumentationViews",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Extracts_DocumentationViewID",
                table: "Extracts",
                column: "DocumentationViewID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentationViews_Facilities_FacilityID",
                table: "DocumentationViews",
                column: "FacilityID",
                principalTable: "Facilities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Extracts_DocumentationViews_DocumentationViewID",
                table: "Extracts",
                column: "DocumentationViewID",
                principalTable: "DocumentationViews",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentationViews_Facilities_FacilityID",
                table: "DocumentationViews");

            migrationBuilder.DropForeignKey(
                name: "FK_Extracts_DocumentationViews_DocumentationViewID",
                table: "Extracts");

            migrationBuilder.DropIndex(
                name: "IX_Extracts_DocumentationViewID",
                table: "Extracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentationViews",
                table: "DocumentationViews");

            migrationBuilder.DropColumn(
                name: "PublicID",
                table: "Extracts");

            migrationBuilder.RenameTable(
                name: "DocumentationViews",
                newName: "DocumentationView");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentationViews_FacilityID",
                table: "DocumentationView",
                newName: "IX_DocumentationView_FacilityID");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentationViewID",
                table: "Extracts",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentationView",
                table: "DocumentationView",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentationView_Facilities_FacilityID",
                table: "DocumentationView",
                column: "FacilityID",
                principalTable: "Facilities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
