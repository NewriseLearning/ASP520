using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newrise.Components;
using Newrise.Services;
using System.Text;

namespace Newrise {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);
			var services = builder.Services;

			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new() {
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.ASCII.GetBytes(SecurityExtensions.JWT_SECURITY_KEY)),
						ValidateAudience = false,
						ValidateIssuer = false
					};
				});

			services.AddControllers();
			services.AddSwaggerGen(options => {
				options.SwaggerDoc("v1", new OpenApiInfo {
					Title = "Newrise API",
					Version = "v1"
				});
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement {{
					new OpenApiSecurityScheme {
						Reference = new OpenApiReference {
						Type = ReferenceType.SecurityScheme, Id="Bearer" }},
						new string[]{}}});
			});
			services.AddMemoryCache();
			services.AddSingleton<OfficeListProvider>();

			var connectionString = builder.Configuration.GetConnectionString("NewriseDb");
			services.AddDbContextFactory<NewriseDbContext>(options =>
				options.UseSqlServer(connectionString));

			services.AddSingleton<EventDataService>();
			services.AddSingleton<ParticipantDataService>();

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
			app.UseAuthentication();
			app.UseAuthorization();
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
