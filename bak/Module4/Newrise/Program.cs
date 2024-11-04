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

			services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
			services.AddAuthentication(UserSession.AuthenticationType);
			services.AddCascadingAuthenticationState();

			var connectionString = "Server=.;Database=NewriseDb;TrustServerCertificate=True;Trusted_Connection=True";
			services.AddDbContextFactory<NewriseDbContext>(options => options.UseSqlServer(connectionString));
			builder.Services.AddSingleton<EventDataService>();
			builder.Services.AddScoped<ParticipantDataService>();

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
			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();
			app.UseAuthentication();
			app.UseAuthorization();
			app.Run();
		}
	}
}
