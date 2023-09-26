using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Tasks.Data;
using Tasks.Models;
using Tasks.Services;
namespace Tasks
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

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
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new AutoMapperProfile());
			});
			IMapper mapper = mapperConfig.CreateMapper();
			builder.Services.AddSingleton(mapper);

			//builder.Services.AddMassTransit(config =>
			//{
			//	//x.AddConsumer<MyConsumer>();

			//	config.UsingRabbitMq((busContext, cfg) =>
			//	{
			//		var uri = new Uri(builder.Configuration["ServiceBus:Uri"]);
			//		cfg.Host(uri, h =>
			//		{
			//			h.Username(builder.Configuration["ServiceBus:Username"]);
			//			h.Password(builder.Configuration["ServiceBus:Password"]);
			//		});
			//	});
			//});

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
