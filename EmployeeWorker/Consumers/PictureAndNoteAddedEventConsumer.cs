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
	public class PictureAndNoteAddedEventConsumerDefinition :
		ConsumerDefinition<PictureAndNoteAddedEventConsumer>
	{
		public PictureAndNoteAddedEventConsumerDefinition()
		{
			EndpointName = "employee.picture-note-added";
		}
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<PictureAndNoteAddedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
	public class PictureAndNoteAddedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<PictureAndNoteAddedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;

		public async Task Consume(ConsumeContext<PictureAndNoteAddedEvent> context)
		{
			PictureAndNoteAddedEvent message = context.Message;

			Employee employee = await _employeeContext.Employees
				.Where(e => e.EmployeeId == message.EmployeeId)
				.FirstOrDefaultAsync();

			if (employee is null) return;

			SubTask subTask = await _employeeContext.SubTasks.FirstOrDefaultAsync(s => s.SubTaskId == message.SubTaskId);

			List<Picture> pictures = message.Pictures
				.Select(p => new Picture()
				{
					PictureId = p.PictureId,
					Path = p.Path,
					SubTask = subTask,
					EmployeeId = employee.EmployeeId
					
				})
				.ToList();

			Note note = new()
			{
				NoteId = message.Note.NoteId,
				Title = message.Note.Title,
				SubTask = subTask,
				EmployeeId = employee.EmployeeId
			};

			_employeeContext.Add(note);
			_employeeContext.AddRange(pictures);
			
			await _employeeContext.SaveChangesAsync();
		}
	}
}

