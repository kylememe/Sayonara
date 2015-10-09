using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace Sayonara.Migrations
{
    public partial class Init : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateSequence(
                name: "DefaultSequence",
                type: "bigint",
                startWith: 1L,
                incrementBy: 10);
            migration.CreateTable(
                name: "Extract",
                columns: table => new
                {
                    ID = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    CompletionDate = table.Column(type: "datetime2", nullable: false),
                    CreatedBy = table.Column(type: "nvarchar(max)", nullable: true),
                    CurrentCount = table.Column(type: "int", nullable: false),
                    DocumentationViewID = table.Column(type: "int", nullable: false),
                    ExtractionDate = table.Column(type: "datetime2", nullable: false),
                    FacilityID = table.Column(type: "int", nullable: false),
                    FilePath = table.Column(type: "nvarchar(max)", nullable: true),
                    Format = table.Column(type: "int", nullable: false),
                    ReceivedDate = table.Column(type: "datetime2", nullable: false),
                    ShippedDate = table.Column(type: "datetime2", nullable: false),
                    Status = table.Column(type: "nvarchar(max)", nullable: true),
                    TotalCount = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extract", x => x.ID);
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropSequence("DefaultSequence");
            migration.DropTable("Extract");
        }
    }
}
