using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Newrise.Client {
	public class Program {
		static async Task Main(string[] args) {
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			var services = builder.Services;
			services.AddSingleton(sp => new HttpClient {
				BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
			});
			await builder.Build().RunAsync();
		}
	}
}
