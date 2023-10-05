using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksAPI.Migrations
{
    /// <inheritdoc />
    public partial class ManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_ToDos_ToDoId",
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
                    ToDosToDoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeToDo", x => new { x.EmployeesEmployeeId, x.ToDosToDoId });
                    table.ForeignKey(
                        name: "FK_EmployeeToDo_Employees_EmployeesEmployeeId",
                        column: x => x.EmployeesEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeToDo_ToDos_ToDosToDoId",
                        column: x => x.ToDosToDoId,
                        principalTable: "ToDos",
                        principalColumn: "ToDoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeToDo_ToDosToDoId",
                table: "EmployeeToDo",
                column: "ToDosToDoId");
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
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ToDoId",
                table: "Employees",
                column: "ToDoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_ToDos_ToDoId",
                table: "Employees",
                column: "ToDoId",
                principalTable: "ToDos",
                principalColumn: "ToDoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
