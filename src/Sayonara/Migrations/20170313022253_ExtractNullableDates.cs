using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sayonara.Migrations
{
    public partial class ExtractNullableDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippedDate",
                table: "Extracts",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceivedDate",
                table: "Extracts",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletionDate",
                table: "Extracts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippedDate",
                table: "Extracts",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceivedDate",
                table: "Extracts",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletionDate",
                table: "Extracts",
                nullable: false);
        }
    }
}
