using MudBlazor.Services;
using Newrise.Components;

namespace Newrise {
	public class Program {
		public const string AppName = "Newrise";
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);
			var services = builder.Services;
			services
				.AddRazorComponents()
				.AddInteractiveServerComponents();
			services.AddMudServices();
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
			app
				.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();
			app.Run();
		}
	}
}
