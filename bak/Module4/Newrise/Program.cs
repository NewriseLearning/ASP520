using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Newrise.Components;
using Newrise.Services;
using Newrise.Shared.Models;

namespace Newrise {
	public class Program {
		public const string AppName = "Newrise";

		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);
			var services = builder.Services;

			services
				.AddRazorComponents()
				.AddInteractiveServerComponents();
			services.AddMemoryCache();
			services.AddMudServices();
			services.AddSingleton<OfficeListProvider>();

			var connectionString = "Server=.;Database=NewriseDb;TrustServerCertificate=True;Trusted_Connection=True";
			services.AddDbContextFactory<NewriseDbContext>(options => options.UseSqlServer(connectionString));
			builder.Services.AddSingleton<EventDataService>();

			services.AddScoped<ProtectedSessionStorage>();
			services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
			services.AddAuthentication(UserSession.AuthenticationType);

			var app = builder.Build();
			var env = app.Environment;
			if (env.IsDevelopment()) {
			}
			else { 
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseAntiforgery();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();
			app.Run();
		}
	}
}
