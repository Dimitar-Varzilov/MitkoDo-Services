using AuthenticationAPI.Data;
using AuthenticationAPI.Services;
using AuthenticationAPI.Swagger;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace AuthenticationAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			ConfigurationManager configuration = builder.Configuration;

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
			bool envIsDev = builder.Environment.IsDevelopment();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
						ValidateLifetime = true
					};
				});

			//Custom services
			builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(configuration["ConnectionStrings:AuthDb"]));
			builder.Services.AddScoped<IAuthService, AuthService>();

			builder.Services.AddMassTransit(x =>
			{

				x.SetKebabCaseEndpointNameFormatter();

				x.UsingRabbitMq((context, cfg) =>
				{
					cfg.Host(configuration["MessageBroker:Host"], "/", h =>
					{
						h.Username(configuration["MessageBroker:guest"]);
						h.Password(configuration["MessageBroker:guest"]);
					});

					cfg.ConfigureEndpoints(context);
				});
			});

			var app = builder.Build();

			app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

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

