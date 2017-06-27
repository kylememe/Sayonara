using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sayonara.Migrations
{
    public partial class AddAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Facilities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Facilities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Facilities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Facilities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Facilities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Facilities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Extracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Extracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Extracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Extracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Extracts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Extracts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Extracts");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Extracts");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Extracts");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Extracts");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Extracts");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Extracts");
        }
    }
}
