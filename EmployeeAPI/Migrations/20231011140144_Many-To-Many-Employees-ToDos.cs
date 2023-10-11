using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
	/// <inheritdoc />
	public partial class ManyToManyEmployeesToDos : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ToDo_Employees_EmployeeId",
				table: "ToDo");

			migrationBuilder.DropIndex(
				name: "IX_ToDo_EmployeeId",
				table: "ToDo");

			migrationBuilder.DropColumn(
				name: "EmployeeId",
				table: "ToDo");

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
						name: "FK_EmployeeToDo_ToDo_ToDosToDoId",
						column: x => x.ToDosToDoId,
						principalTable: "ToDo",
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
				name: "EmployeeId",
				table: "ToDo",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

			migrationBuilder.CreateIndex(
				name: "IX_ToDo_EmployeeId",
				table: "ToDo",
				column: "EmployeeId");

			migrationBuilder.AddForeignKey(
				name: "FK_ToDo_Employees_EmployeeId",
				table: "ToDo",
				column: "EmployeeId",
				principalTable: "Employees",
				principalColumn: "EmployeeId",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
