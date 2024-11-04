using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Newrise.Shared.Models;
using System.Security.Cryptography;
using System.Text;

namespace Newrise.Services {
	public class ParticipantDataService {
		readonly IDbContextFactory<NewriseDbContext> _dcFactory;
		readonly ServerAuthenticationStateProvider _authenticationStateProvider;
		public ParticipantDataService(
			IDbContextFactory<NewriseDbContext> dcFactory,
			AuthenticationStateProvider authenticationStateProvider) {
			_dcFactory = dcFactory;
			_authenticationStateProvider =
				(ServerAuthenticationStateProvider)
				authenticationStateProvider;
		}

		const string PASSWORD_SALT = "$_{0}@newrise.921";

		public static string HashPassword(string password) {
			var ha = SHA256.Create();
			password = string.Format(PASSWORD_SALT, password);
			var hashedPassword = ha.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hashedPassword);
		}

		public async Task InitializeAsync() {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				if (await dc.Participants.FindAsync("admin") == null) {
					var user = new Participant {
						Id = "admin",
						Name = "Administrator",
						Email = "symbolicon@live.com",
						Company = "Newrise Learning",
						Position = "Web Administrator",
						PasswordHash = HashPassword("P@ssw0rd"),
						IsAdmin = true
					};
					await dc.Participants.AddAsync(user);
					await dc.SaveChangesAsync();
				}
			}
		}

		public async Task SignInAsync(string userId, string password) {
			var hashedPassword = HashPassword(password);
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var user = dc.Participants.FirstOrDefault(p => (
					p.Id == userId || p.Email == userId) && p.PasswordHash == hashedPassword);
				if (user == null) throw new Exception("Invalid user Id or password.");
				var userSession = new UserSession { UserId = user.Id, IsAdmin = user.IsAdmin };
				await _authenticationStateProvider.UpdateAuthenticationStateAsync(userSession);
			}
		}

		public async Task<Participant> GetParticipantAsync(string id) {
			using (var dc = await _dcFactory.CreateDbContextAsync())
				return dc.Participants.FirstOrDefault(p => p.Id == id || p.Email == id);
		}
		public async Task<Participant> GetCurrentParticipantAsync() {
			var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
			if (state != null) return await GetParticipantAsync(state.User.Identity.Name);
			return null;
		}
		public async Task SignOutAsync() {
			await _authenticationStateProvider.UpdateAuthenticationStateAsync(null);
		}

		public async Task AddParticipantAsync(NewParticipant participant) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				if (await dc.Participants.FirstOrDefaultAsync(
				p => p.Id == participant.Id) != null)
					throw new Exception("User ID already taken. Use another ID.");
				if (await dc.Participants.FirstOrDefaultAsync(
					p => p.Email == participant.Email) != null)
					throw new Exception("Email already registered. Use another email.");
				participant.PasswordHash = HashPassword(participant.Password);
				dc.Participants.Add(participant);
				dc.SaveChanges();
			}
		}

	}
}
