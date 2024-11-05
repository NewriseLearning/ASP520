using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newrise.Shared.Models;
using System.Security.Claims;

namespace Newrise.Services {
	public class ServerAuthenticationStateProvider : AuthenticationStateProvider {
		private ProtectedSessionStorage _storage;

		private readonly AuthenticationState _anonymous = new AuthenticationState(
			new ClaimsPrincipal(new ClaimsIdentity()));
		private AuthenticationState _state;

		public ServerAuthenticationStateProvider(ProtectedSessionStorage storage) {
			_storage = storage;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
			if (_state != null) return _state;
			try {
				var result = await _storage.GetAsync<UserSession>("UserSession");
				if (result.Success) return _state = new AuthenticationState(
					result.Value.GetPrincipal());
				return _anonymous; } catch {
				return _anonymous; }
		}
		public async Task UpdateAuthenticationStateAsync(UserSession userSession) {
			if (userSession == null) {
				await _storage.DeleteAsync("UserSession");
				NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
			}
			else {
				await _storage.SetAsync("UserSession", userSession);
				_state = new AuthenticationState(userSession.GetPrincipal());
				NotifyAuthenticationStateChanged(Task.FromResult(_state));
			}
		}
	}
}
