using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddToDoEmployeeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Tasks_ToDoId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ToDoId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ToDoId",
                table: "Employees");

            migrationBuilder.CreateTable(
                name: "EmployeeToDo",
                columns: table => new
                {
                    EmployeesEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TasksToDoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeToDo", x => new { x.EmployeesEmployeeId, x.TasksToDoId });
                    table.ForeignKey(
                        name: "FK_EmployeeToDo_Employees_EmployeesEmployeeId",
                        column: x => x.EmployeesEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeToDo_Tasks_TasksToDoId",
                        column: x => x.TasksToDoId,
                        principalTable: "Tasks",
                        principalColumn: "ToDoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeToDo_TasksToDoId",
                table: "EmployeeToDo",
                column: "TasksToDoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeToDo");

            migrationBuilder.AddColumn<Guid>(
                name: "ToDoId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ToDoId",
                table: "Employees",
                column: "ToDoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Tasks_ToDoId",
                table: "Employees",
                column: "ToDoId",
                principalTable: "Tasks",
                principalColumn: "ToDoId");
        }
    }
}
