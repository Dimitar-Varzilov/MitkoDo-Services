﻿// <auto-generated />
using System;
using EmployeeAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmployeeAPI.Migrations
{
    [DbContext(typeof(EmployeeContext))]
    [Migration("20230929141556_Initial-Migration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmployeeAPI.Models.Employee", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EmployeeAPI.Models.Note", b =>
                {
                    b.Property<Guid>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SubTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NoteId");

                    b.HasIndex("SubTaskId");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("EmployeeAPI.Models.Picture", b =>
                {
                    b.Property<Guid>("PictureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SubTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PictureId");

                    b.HasIndex("SubTaskId");

                    b.ToTable("Picture");
                });

            modelBuilder.Entity("EmployeeAPI.Models.SubTask", b =>
                {
                    b.Property<Guid>("SubTaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubTaskId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("SubTask");
                });

            modelBuilder.Entity("EmployeeAPI.Models.ToDo", b =>
                {
                    b.Property<Guid>("ToDoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ToDoId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("ToDo");
                });

            modelBuilder.Entity("EmployeeAPI.Models.Note", b =>
                {
                    b.HasOne("EmployeeAPI.Models.SubTask", null)
                        .WithMany("Notes")
                        .HasForeignKey("SubTaskId");
                });

            modelBuilder.Entity("EmployeeAPI.Models.Picture", b =>
                {
                    b.HasOne("EmployeeAPI.Models.SubTask", null)
                        .WithMany("Pictures")
                        .HasForeignKey("SubTaskId");
                });

            modelBuilder.Entity("EmployeeAPI.Models.SubTask", b =>
                {
                    b.HasOne("EmployeeAPI.Models.Employee", null)
                        .WithMany("SubTasks")
                        .HasForeignKey("EmployeeId");
                });

            modelBuilder.Entity("EmployeeAPI.Models.ToDo", b =>
                {
                    b.HasOne("EmployeeAPI.Models.Employee", null)
                        .WithMany("ToDos")
                        .HasForeignKey("EmployeeId");
                });

            modelBuilder.Entity("EmployeeAPI.Models.Employee", b =>
                {
                    b.Navigation("SubTasks");

                    b.Navigation("ToDos");
                });

            modelBuilder.Entity("EmployeeAPI.Models.SubTask", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("Pictures");
                });
#pragma warning restore 612, 618
        }
    }
}