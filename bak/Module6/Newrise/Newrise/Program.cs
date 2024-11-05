using Newrise.Components;
using Newrise.Services;

namespace Newrise {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);
			var services = builder.Services;
			services.AddControllers();
			services.AddMemoryCache();
			services.AddSingleton<OfficeListProvider>();
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
			app.MapRazorComponents<App>()
				.AddInteractiveWebAssemblyRenderMode()
				.AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
			app.Run();
		}
	}
}
