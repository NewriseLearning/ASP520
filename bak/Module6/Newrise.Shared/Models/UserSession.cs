using System;
using System.Security.Claims;

namespace Newrise.Shared.Models {
	public class UserSession {
		public string UserId { get; set; }
		public bool IsAdmin { get; set; }
		public string Token { get; set; }
		public TimeSpan ExpiresIn { get; set; }
		public DateTime ExpiresAt { get; set; }

		public const string AuthenticationType = "NewriseAuthentication";

		public ClaimsPrincipal GetPrincipal() {
			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.Name, UserId));
			if (IsAdmin) claims.Add(new Claim(ClaimTypes.Role, "admin"));
			var identity = new ClaimsIdentity(claims, AuthenticationType);
			return new ClaimsPrincipal(identity);
		}
	}
}
