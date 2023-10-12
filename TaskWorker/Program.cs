using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TaskWorker.Consumers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskWorker
{
	public class Program
	{
		static bool? _isRunningInContainer;

		static bool IsRunningInContainer =>
			_isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;

		public static async Task Main(string[] args)
		{
			await CreateHostBuilder(args).Build().RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					if (IsRunningInContainer)
					{
						services.Configure<HostOptions>(options =>
						{
							options.ShutdownTimeout = TimeSpan.FromSeconds(30);
						});
					}
					services.Configure<MessageBrokerSettings>(hostContext.Configuration.GetSection("MessageBroker"));
					services.AddSingleton(services => services.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
					services.AddMassTransit(busConfigurator =>
					{
						busConfigurator.SetKebabCaseEndpointNameFormatter();

						// By default, sagas are in-memory, but should be changed to a durable
						// saga repository.

						var entryAssembly = Assembly.GetEntryAssembly();

						busConfigurator.AddConsumers(entryAssembly);
						busConfigurator.AddActivities(entryAssembly);

						busConfigurator.UsingRabbitMq((context, cfg) =>
						{
							MessageBrokerSettings messageBrokerSettings = context.GetRequiredService<MessageBrokerSettings>();
							//cfg.Host(hostContext.Configuration.GetConnectionString("DefaultConnection"));
							cfg.Host(messageBrokerSettings.Host, h =>
							{
								h.Username(messageBrokerSettings.Username);
								h.Password(messageBrokerSettings.Password);
							});

							cfg.ConfigureEndpoints(context);
							cfg.ReceiveEndpoint("task.user-created", e =>
							{
								e.ConfigureConsumer<UserCreatedEventConsumer>(context);
							});
						});
					});
					services.AddDbContext<TaskContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("TaskDb")));
				});
	}
}
