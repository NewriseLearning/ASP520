using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
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
			services.AddScoped<ProtectedSessionStorage>();
			services.AddScoped<AuthenticationStateProvider,
				ServerAuthenticationStateProvider>();
			services.AddAuthentication(UserSession.AuthenticationType);
			services.AddCascadingAuthenticationState();

			services
				.AddRazorComponents()
				.AddInteractiveServerComponents();
			services.AddMemoryCache();
			services.AddMudServices(configuration => {
				configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
			});
			services.AddSingleton<OfficeListProvider>();

			var connectionString = builder.Configuration.GetConnectionString("NewriseDb");
			services.AddDbContextFactory<NewriseDbContext>(
				options => options.UseSqlServer(connectionString));

			services.AddSingleton<EventDataService>();
			services.AddScoped<ParticipantDataService>();

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
			app
				.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();
			app.Run();
		}
	}
}
