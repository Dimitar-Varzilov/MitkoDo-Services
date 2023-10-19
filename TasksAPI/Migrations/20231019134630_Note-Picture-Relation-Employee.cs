using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksAPI.Migrations
{
    /// <inheritdoc />
    public partial class NotePictureRelationEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UploadedBy",
                table: "Pictures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UploadedBy",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UploadedBy",
                table: "Pictures",
                column: "UploadedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UploadedBy",
                table: "Notes",
                column: "UploadedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Employees_UploadedBy",
                table: "Notes",
                column: "UploadedBy",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Employees_UploadedBy",
                table: "Pictures",
                column: "UploadedBy",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Employees_UploadedBy",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Employees_UploadedBy",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_UploadedBy",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UploadedBy",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "UploadedBy",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "UploadedBy",
                table: "Notes");
        }
    }
}
