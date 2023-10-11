using MassTransit;
using Microsoft.EntityFrameworkCore;
using TasksAPI.Data;
using TasksAPI.Services;
namespace TasksAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			//Utilities.GenerateGuids(20);
			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			bool envIsDev = builder.Environment.IsDevelopment();

			//Custom services
			string dbString = envIsDev ? "TaskDb-dev" : "TaskDb";
			builder.Services.AddDbContext<TaskContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(dbString)));
			builder.Services.AddScoped<ITaskService, TaskService>();

			builder.Services.AddMassTransit(x =>
			{

				x.SetKebabCaseEndpointNameFormatter();

				x.UsingRabbitMq((context, cfg) =>
				{
					//if (IsRunningInContainer)
					//	cfg.Host("rabbitmq");
					cfg.Host("localhost", "/", h =>
					{
						h.Username("guest");
						h.Password("guest");
					});

					cfg.UseDelayedMessageScheduler();

					cfg.ConfigureEndpoints(context);
				});
			});

			var app = builder.Build();

			app.UseCors(option => option.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

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
