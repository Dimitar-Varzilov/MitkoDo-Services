using Auth;
using Auth.Models;
using Auth.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Auth
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
			string dbString = envIsDev ? "AuthDb-dev" : "AuthDb";
			builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(dbString)));
			builder.Services.AddScoped<IAuthService, AuthService>();
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new AutoMapperProfile());
			});
			IMapper mapper = mapperConfig.CreateMapper();
			builder.Services.AddSingleton(mapper);

			var app = builder.Build();

			//using (var scope = app.Services.CreateScope())
			//{
			//	var db = scope.ServiceProvider.GetRequiredService<AuthContext>();
			//	db.Database.Migrate(); // This is needed to ensure the db is in the latest version.
			//}

			//using (var scope = app.Services.CreateScope())
			//{
			//	var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
			//	authContext.Database.EnsureCreated();
			//	authContext.Seed();
			//}

			// Configure the HTTP request pipeline.
			if (envIsDev)
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

