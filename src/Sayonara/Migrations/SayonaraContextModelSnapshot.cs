using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Sayonara.Data;

namespace Sayonara.Migrations
{
    [DbContext(typeof(SayonaraContext))]
    partial class SayonaraContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sayonara.Models.DocumentationView", b =>
                {
                    b.Property<int>("ID");

                    b.Property<int>("FacilityID");

                    b.Property<bool>("MedicalRecordCopy");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("FacilityID");

                    b.ToTable("DocumentationView");
                });

            modelBuilder.Entity("Sayonara.Models.Extract", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CompletionDate");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("CurrentCount");

                    b.Property<int>("DocumentationViewID");

                    b.Property<DateTime>("ExtractionDate");

                    b.Property<int>("FacilityID");

                    b.Property<string>("FilePath");

                    b.Property<int>("Format");

                    b.Property<DateTime>("ReceivedDate");

                    b.Property<DateTime>("ShippedDate");

                    b.Property<string>("Status");

                    b.Property<int>("TotalCount");

                    b.HasKey("ID");

                    b.HasIndex("FacilityID");

                    b.ToTable("Extracts");
                });

            modelBuilder.Entity("Sayonara.Models.Facility", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Alias");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("Sayonara.Models.DocumentationView", b =>
                {
                    b.HasOne("Sayonara.Models.Facility", "Facility")
                        .WithMany("DocumentationViews")
                        .HasForeignKey("FacilityID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sayonara.Models.Extract", b =>
                {
                    b.HasOne("Sayonara.Models.Facility", "Facility")
                        .WithMany()
                        .HasForeignKey("FacilityID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
