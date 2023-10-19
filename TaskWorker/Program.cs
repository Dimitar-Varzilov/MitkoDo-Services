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
					var configuration = hostContext.Configuration;
					if (IsRunningInContainer)
					{
						services.Configure<HostOptions>(options =>
						{
							options.ShutdownTimeout = TimeSpan.FromSeconds(30);
						});
					}

					services.AddMassTransit(busConfigurator =>
					{
						busConfigurator.SetKebabCaseEndpointNameFormatter();

						var entryAssembly = Assembly.GetEntryAssembly();

						busConfigurator.AddConsumers(entryAssembly);
						busConfigurator.AddActivities(entryAssembly);

						busConfigurator.UsingRabbitMq((context, cfg) =>
						{
							cfg.Host(configuration["MessageBroker:Host"], "/", h =>
							{
								h.Username(configuration["MessageBroker:guest"]);
								h.Password(configuration["MessageBroker:guest"]);
							});

							cfg.ConfigureEndpoints(context);
						});
					});
					services.AddDbContext<TaskContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("TaskDb")));
				});
	}
}
