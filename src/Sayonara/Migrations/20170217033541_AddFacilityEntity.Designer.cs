﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Sayonara.Data;

namespace Sayonara.Migrations
{
    [DbContext(typeof(SayonaraContext))]
    [Migration("20170217033541_AddFacilityEntity")]
    partial class AddFacilityEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("Extracts");
                });

            modelBuilder.Entity("Sayonara.Models.Facility", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Facilities");
                });
        }
    }
}
