using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "ToDos",
                columns: table => new
                {
                    ToDoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDos", x => x.ToDoId);
                });

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

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    SubTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PicturesCountToBeCompleted = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NotesCountToBeCompleted = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ToDoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => x.SubTaskId);
                    table.ForeignKey(
                        name: "FK_SubTasks_ToDos_ToDoId",
                        column: x => x.ToDoId,
                        principalTable: "ToDos",
                        principalColumn: "ToDoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Notes_Employees_UploadedBy",
                        column: x => x.UploadedBy,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_SubTasks_SubTaskId",
                        column: x => x.SubTaskId,
                        principalTable: "SubTasks",
                        principalColumn: "SubTaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    PictureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.PictureId);
                    table.ForeignKey(
                        name: "FK_Pictures_Employees_UploadedBy",
                        column: x => x.UploadedBy,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pictures_SubTasks_SubTaskId",
                        column: x => x.SubTaskId,
                        principalTable: "SubTasks",
                        principalColumn: "SubTaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeToDo_ToDosToDoId",
                table: "EmployeeToDo",
                column: "ToDosToDoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_SubTaskId",
                table: "Notes",
                column: "SubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UploadedBy",
                table: "Notes",
                column: "UploadedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_SubTaskId",
                table: "Pictures",
                column: "SubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UploadedBy",
                table: "Pictures",
                column: "UploadedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_ToDoId",
                table: "SubTasks",
                column: "ToDoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeToDo");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.DropTable(
                name: "ToDos");
        }
    }
}
