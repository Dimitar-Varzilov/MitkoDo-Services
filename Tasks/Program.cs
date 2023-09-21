using Tasks.Models;
namespace Tasks
{
	public class Program
	{
		public static List<CustomTask> Tasks =new()
		{
			new CustomTask {Id = Guid.NewGuid(), Title = "Task 1", Description = "Description 1", Status = TaskStatus.Running},
			new CustomTask {Id = Guid.NewGuid(), Title = "Task 2", Description = "Description 2", Status = TaskStatus.WaitingToRun},
			new CustomTask {Id = Guid.NewGuid(), Title = "Task 3", Description = "Description 3", Status = TaskStatus.Created},
		};
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
