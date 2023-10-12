using EmployeeAPI.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksAPI.Events;

namespace EmployeeWorker.Consumers
{
	public class PictureAndNoteAddedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<PictureAndNoteAddedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;

		public async Task Consume(ConsumeContext<PictureAndNoteAddedEvent> context)
		{
			PictureAndNoteAddedEvent message = context.Message;
			List<Employee> queryList = [.. _employeeContext.Employees.Where(e => e.EmployeeId == message.EmployeeId).Include(e => e.SubTasks.Where(s => s.SubTaskId == message.SubTaskId))];
			if (queryList.Count == 0) return;

			Employee employee = queryList.First();
			bool isSubTaskFound = employee.SubTasks.Count > 0;
			SubTask subTaskТоEdit = isSubTaskFound ? employee.SubTasks.First() : new SubTask();
			List<Picture> picturesCasted = [.. message.Pictures.Select(p => new Picture()
			{
				PictureId = p.PictureId,
				Path = p.Path,
				SubTaskId = subTaskТоEdit.SubTaskId,
				SubTask = subTaskТоEdit
			})];
			Note note = new()
			{
				NoteId = message.Note.NoteId,
				Title = message.Note.Title,
				SubTaskId = subTaskТоEdit.SubTaskId,
				SubTask = subTaskТоEdit
			};
			List<Note> notesCasted = [note];
			SubTask newSubTask = new()
			{
				Title = message.SubTaskTitle,
				Pictures = picturesCasted,
				Notes = notesCasted,
				SubTaskId = message.SubTaskId,
				EmployeeId = employee.EmployeeId,
				Employee = employee
			};

			if (!isSubTaskFound)
			{
				_employeeContext.SubTasks.Add(newSubTask);
			}
			else
			{
				_employeeContext.Pictures.AddRange(picturesCasted);
				_employeeContext.Notes.AddRange(notesCasted);
			}
			int rowChanged = await _employeeContext.SaveChangesAsync();
		}
	}
}

