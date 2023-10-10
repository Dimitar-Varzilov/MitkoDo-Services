using Gateway.EventBusConfig;
using MassTransit;

namespace Gateway
{
	public class Program
	{
		static bool? _isRunningInContainer;

		static bool IsRunningInContainer =>
			_isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddMassTransit(x =>
			{

				x.SetKebabCaseEndpointNameFormatter();

				x.UsingRabbitMq((context, cfg) =>
				{
					if (IsRunningInContainer)
						cfg.Host("rabbitmq");
					cfg.Host("localhost", "/", h =>
					{
						h.Username("guest");
						h.Password("guest");
					});

					cfg.UseDelayedMessageScheduler();

					cfg.ConfigureEndpoints(context);
				});
			});

			builder.Services.AddTransient<IEventBus, EventBus>();

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