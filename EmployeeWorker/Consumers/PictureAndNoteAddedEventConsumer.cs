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
			SubTask subTask = _employeeContext.SubTasks.FirstOrDefault(s => s.SubTaskId == message.SubTaskId);
			bool isSubTaskFound = subTask != null;

			List<Employee> queryList = [.. _employeeContext.Employees.Where(e => e.EmployeeId == message.EmployeeId).Include(e => e.SubTasks.Where(s => s.SubTaskId == message.SubTaskId))];
			if (queryList.Count == 0) return;

			Employee employee = queryList.First();
			bool hasEmployeeAddedToSubtask = employee.SubTasks.Count > 0;
			SubTask subTaskТоEdit = hasEmployeeAddedToSubtask ? employee.SubTasks.First() : new SubTask();

			List<Picture> picturesCasted = [.. message.Pictures.Select(p => new Picture()
			{
				PictureId = p.PictureId,
				Path = p.Path,
				SubTaskId = message.SubTaskId,
			})];
			Note note = new()
			{
				NoteId = message.Note.NoteId,
				Title = message.Note.Title,
				SubTaskId = message.SubTaskId,
			};
			List<Note> notesCasted = [note];

			if (!isSubTaskFound && !hasEmployeeAddedToSubtask)
			{
				SubTask newSubTask = new()
				{
					SubTaskId = message.SubTaskId,
					Title = message.SubTaskTitle,
					Pictures = picturesCasted,
					Notes = notesCasted,
					Employees = [employee]
				};
				_employeeContext.SubTasks.Add(newSubTask);
			}

			note.SubTaskId = subTask.SubTaskId;
			note.SubTask = subTask;
			picturesCasted.ForEach(p =>
			{
				p.SubTaskId = subTask.SubTaskId;
				p.SubTask = subTask;
			});

			if (isSubTaskFound && !hasEmployeeAddedToSubtask)
			{
				_employeeContext.Add(note);
				_employeeContext.AddRange(picturesCasted);
				subTask.Employees.Add(employee);
			}
			else if (isSubTaskFound && hasEmployeeAddedToSubtask)
			{
				_employeeContext.Add(note);
				_employeeContext.AddRange(picturesCasted);
			}
			int rowChanged = await _employeeContext.SaveChangesAsync();

			await Console.Out.WriteLineAsync(rowChanged.ToString());
		}
	}
}

