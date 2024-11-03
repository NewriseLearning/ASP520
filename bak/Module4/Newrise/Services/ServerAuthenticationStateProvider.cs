using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using Newrise.Components.Pages.Event;
using Newrise.Shared.Models;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Newrise.Services {
	public class ServerAuthenticationStateProvider : AuthenticationStateProvider {
		
		private ProtectedSessionStorage _sessionStorage;
		private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

		public ServerAuthenticationStateProvider(ProtectedSessionStorage sessionStorage) {
			_sessionStorage = sessionStorage;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
			try {
				var result = await _sessionStorage.GetAsync<UserSession>("UserSession");
				var userSession = result.Success ? result.Value : null;
				if (userSession == null) return await Task.FromResult(new AuthenticationState(_anonymous));
				var principal = userSession.GetPrincipal();
				return await Task.FromResult(new AuthenticationState(principal));
			} catch {
				return await Task.FromResult(new AuthenticationState(_anonymous));
			}
		}

		public async Task UpdateAuthenticationState(UserSession userSession) {
			if (userSession != null) await _sessionStorage.SetAsync("UserSession", userSession);
			else await _sessionStorage.DeleteAsync("UserSession");
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}
	}
}
