using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class ConnectPictureNotesToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSubTask");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Pictures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.AddColumn<Guid>(
            //    name: "EmployeeId",
            //    table: "Notes",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_EmployeeId",
                table: "Pictures",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_EmployeeId",
                table: "Notes",
                column: "EmployeeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Notes_Employees_EmployeeId",
            //    table: "Notes",
            //    column: "EmployeeId",
            //    principalTable: "Employees",
            //    principalColumn: "EmployeeId",
            //    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Employees_EmployeeId",
                table: "Pictures",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Employees_EmployeeId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Employees_EmployeeId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_EmployeeId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Notes_EmployeeId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Notes");

            migrationBuilder.CreateTable(
                name: "EmployeeSubTask",
                columns: table => new
                {
                    EmployeesEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubTasksSubTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSubTask", x => new { x.EmployeesEmployeeId, x.SubTasksSubTaskId });
                    table.ForeignKey(
                        name: "FK_EmployeeSubTask_Employees_EmployeesEmployeeId",
                        column: x => x.EmployeesEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSubTask_SubTasks_SubTasksSubTaskId",
                        column: x => x.SubTasksSubTaskId,
                        principalTable: "SubTasks",
                        principalColumn: "SubTaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSubTask_SubTasksSubTaskId",
                table: "EmployeeSubTask",
                column: "SubTasksSubTaskId");
        }
    }
}
