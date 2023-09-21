using Auth;
using Auth.Models;
using Auth.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Auth
{
	public class Program
	{
		public static List<User> Users = new();
		private static string _connStr = @"
	Server=127.0.0.1,1433;
	Database=Master;
	User Id=SA;
	Password=A&VeryComplex123Password
	MultipleActiveResultSets=true
	TrustServerCertificate=True"
;

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			//Custom services
			string dbString = !builder.Environment.IsDevelopment() ? "AuthDb" : "AuthDb-dev";
			builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(dbString)));
			//builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(_connStr));
			builder.Services.AddSingleton<IAuthService, AuthService>();
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

			using (var scope = app.Services.CreateScope())
			{
				var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
				authContext.Database.EnsureCreated();
				authContext.Seed();
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			//app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();

		}
	}
}

