﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SampleAppsAsMicroService.Efs;

namespace SampleAppsAsMicroService.Migrations
{
    [DbContext(typeof(EfContext))]
    [Migration("20190424104614_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SampleAppsAsMicroService.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("SampleAppsAsMicroService.Models.JobRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Level");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("JobRoles");

                    b.HasData(
                        new { Id = 1, Level = "I1", Title = "Intern" },
                        new { Id = 2, Level = "I2", Title = "Trainee" },
                        new { Id = 3, Level = "I3", Title = "Analyst" }
                    );
                });

            modelBuilder.Entity("SampleAppsAsMicroService.Models.Employee", b =>
                {
                    b.HasOne("SampleAppsAsMicroService.Models.JobRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}
