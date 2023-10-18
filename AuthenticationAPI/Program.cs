using AuthenticationAPI.Data;
using AuthenticationAPI.Services;
using AuthenticationAPI.Swagger;
using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthenticationAPI
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
			builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
			bool envIsDev = builder.Environment.IsDevelopment();

			//Custom services
			builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:AuthDb"]));
			builder.Services.AddScoped<IAuthService, AuthService>();

			builder.Services.AddMassTransit(x =>
			{

				x.SetKebabCaseEndpointNameFormatter();

				x.UsingRabbitMq((context, cfg) =>
				{
					if (IsRunningInContainer)
						cfg.Host(builder.Configuration["MessageBroker:Host"]);
					else
					{
						cfg.Host("localhost", "/", h =>
						{
							h.Username("guest");
							h.Password("guest");
						});
					}

					cfg.ConfigureEndpoints(context);
				});
			});

			var app = builder.Build();

			app.UseCors(option => option.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());

			// Configure the HTTP request pipeline.
			if (envIsDev)
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();

		}
	}
}

