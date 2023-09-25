
using MassTransit;

namespace RootAPI
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
			builder.Services.AddMassTransit(config =>
			{
				//x.AddConsumer<MyConsumer>();

				config.UsingRabbitMq((busContext, cfg) =>
				{
					var uri = new Uri(builder.Configuration["ServiceBus:Uri"]);
					cfg.Host(uri, h =>
					{
						h.Username(builder.Configuration["ServiceBus:Username"]);
						h.Password(builder.Configuration["ServiceBus:Password"]);
					});
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
