using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newrise.Components;
using Newrise.Services;

namespace Newrise {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);
			var services = builder.Services;
			services.AddControllers();
			services.AddSwaggerGen(options => {
				options.SwaggerDoc("v1", new OpenApiInfo {
					Title = "Newrise API",
					Version = "v1"
				});
			});
			services.AddMemoryCache();
			services.AddSingleton<OfficeListProvider>();

			var connectionString = builder.Configuration.GetConnectionString("NewriseDb");
			services.AddDbContextFactory<NewriseDbContext>(options =>
				options.UseSqlServer(connectionString));

			services.AddSingleton<EventDataService>();


			services.AddRazorComponents()
				.AddInteractiveWebAssemblyComponents();

			var app = builder.Build();
			if (app.Environment.IsDevelopment()) {
				app.UseWebAssemblyDebugging();
			} else {
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseAntiforgery();
			app.MapControllers();
			app.UseSwagger();
			app.UseSwaggerUI(options =>
				options.SwaggerEndpoint(
				"/swagger/v1/swagger.json",
				"Newrise API"));

			app.MapRazorComponents<App>()
				.AddInteractiveWebAssemblyRenderMode()
				.AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
			app.Run();
		}
	}
}
