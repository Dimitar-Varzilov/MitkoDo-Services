using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class TableRename2 : Migration
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Picture",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Note",
                table: "Note");

            migrationBuilder.RenameTable(
                name: "ToDo",
                newName: "ToDos");

            migrationBuilder.RenameTable(
                name: "SubTask",
                newName: "SubTasks");

            migrationBuilder.RenameTable(
                name: "Picture",
                newName: "Pictures");

            migrationBuilder.RenameTable(
                name: "Note",
                newName: "Notes");

            migrationBuilder.RenameIndex(
                name: "IX_SubTask_EmployeeId",
                table: "SubTasks",
                newName: "IX_SubTasks_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Picture_SubTaskId",
                table: "Pictures",
                newName: "IX_Pictures_SubTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Note_SubTaskId",
                table: "Notes",
                newName: "IX_Notes_SubTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDos",
                table: "ToDos",
                column: "ToDoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTasks",
                table: "SubTasks",
                column: "SubTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures",
                column: "PictureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notes",
                table: "Notes",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeToDo_ToDos_ToDosToDoId",
                table: "EmployeeToDo",
                column: "ToDosToDoId",
                principalTable: "ToDos",
                principalColumn: "ToDoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_SubTasks_SubTaskId",
                table: "Notes",
                column: "SubTaskId",
                principalTable: "SubTasks",
                principalColumn: "SubTaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_SubTasks_SubTaskId",
                table: "Pictures",
                column: "SubTaskId",
                principalTable: "SubTasks",
                principalColumn: "SubTaskId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Notes_SubTasks_SubTaskId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_SubTasks_SubTaskId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Employees_EmployeeId",
                table: "SubTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToDos",
                table: "ToDos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTasks",
                table: "SubTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notes",
                table: "Notes");

            migrationBuilder.RenameTable(
                name: "ToDos",
                newName: "ToDo");

            migrationBuilder.RenameTable(
                name: "SubTasks",
                newName: "SubTask");

            migrationBuilder.RenameTable(
                name: "Pictures",
                newName: "Picture");

            migrationBuilder.RenameTable(
                name: "Notes",
                newName: "Note");

            migrationBuilder.RenameIndex(
                name: "IX_SubTasks_EmployeeId",
                table: "SubTask",
                newName: "IX_SubTask_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_SubTaskId",
                table: "Picture",
                newName: "IX_Picture_SubTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_SubTaskId",
                table: "Note",
                newName: "IX_Note_SubTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDo",
                table: "ToDo",
                column: "ToDoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTask",
                table: "SubTask",
                column: "SubTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picture",
                table: "Picture",
                column: "PictureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Note",
                table: "Note",
                column: "NoteId");

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
                principalColumn: "SubTaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_SubTask_SubTaskId",
                table: "Picture",
                column: "SubTaskId",
                principalTable: "SubTask",
                principalColumn: "SubTaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTask_Employees_EmployeeId",
                table: "SubTask",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
