using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameTodoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeToDo_ToDo_ToDosToDoId",
                table: "EmployeeToDo");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_SubTask_SubTaskId",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_Picture_SubTask_SubTaskId",
                table: "Picture");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTask_Employees_EmployeeId",
                table: "SubTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToDo",
                table: "ToDo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTask",
                table: "SubTask");

            migrationBuilder.RenameTable(
                name: "ToDo",
                newName: "ToDos");

            migrationBuilder.RenameTable(
                name: "SubTask",
                newName: "SubTasks");

            migrationBuilder.RenameIndex(
                name: "IX_SubTask_EmployeeId",
                table: "SubTasks",
                newName: "IX_SubTasks_EmployeeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "SubTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDos",
                table: "ToDos",
                column: "ToDoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTasks",
                table: "SubTasks",
                column: "SubTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeToDo_ToDos_ToDosToDoId",
                table: "EmployeeToDo",
                column: "ToDosToDoId",
                principalTable: "ToDos",
                principalColumn: "ToDoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_SubTasks_SubTaskId",
                table: "Note",
                column: "SubTaskId",
                principalTable: "SubTasks",
                principalColumn: "SubTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_SubTasks_SubTaskId",
                table: "Picture",
                column: "SubTaskId",
                principalTable: "SubTasks",
                principalColumn: "SubTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTasks_Employees_EmployeeId",
                table: "SubTasks",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeToDo_ToDos_ToDosToDoId",
                table: "EmployeeToDo");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_SubTasks_SubTaskId",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_Picture_SubTasks_SubTaskId",
                table: "Picture");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Employees_EmployeeId",
                table: "SubTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToDos",
                table: "ToDos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTasks",
                table: "SubTasks");

            migrationBuilder.RenameTable(
                name: "ToDos",
                newName: "ToDo");

            migrationBuilder.RenameTable(
                name: "SubTasks",
                newName: "SubTask");

            migrationBuilder.RenameIndex(
                name: "IX_SubTasks_EmployeeId",
                table: "SubTask",
                newName: "IX_SubTask_EmployeeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "SubTask",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDo",
                table: "ToDo",
                column: "ToDoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTask",
                table: "SubTask",
                column: "SubTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeToDo_ToDo_ToDosToDoId",
                table: "EmployeeToDo",
                column: "ToDosToDoId",
                principalTable: "ToDo",
                principalColumn: "ToDoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_SubTask_SubTaskId",
                table: "Note",
                column: "SubTaskId",
                principalTable: "SubTask",
                principalColumn: "SubTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_SubTask_SubTaskId",
                table: "Picture",
                column: "SubTaskId",
                principalTable: "SubTask",
                principalColumn: "SubTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTask_Employees_EmployeeId",
                table: "SubTask",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }
    }
}
