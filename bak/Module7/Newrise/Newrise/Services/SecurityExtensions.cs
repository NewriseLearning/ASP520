using Microsoft.IdentityModel.Tokens;
using Newrise.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Newrise.Services {
	public static class SecurityExtensions {
		public const string JWT_SECURITY_KEY = "mXu4yaf7oDGF5nRCXnrB1KQGhFTDlSax";
		public static readonly TimeSpan JWT_TOKEN_EXPIRES = TimeSpan.FromHours(8);
		public static void GenerateToken(this UserSession session, TimeSpan? expiresIn = null) {
			var identity = session.GetPrincipal().Identity as ClaimsIdentity;
			var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(tokenKey),
				SecurityAlgorithms.HmacSha256Signature);
			if (expiresIn == null) expiresIn = JWT_TOKEN_EXPIRES;
			var expiresAt = DateTime.UtcNow.Add(expiresIn.Value);
			var securityDescriptor = new SecurityTokenDescriptor {
				Subject = identity,
				Expires = expiresAt,
				SigningCredentials = signingCredentials
			};
			var securityTokenHandler = new JwtSecurityTokenHandler();
			var securityToken = securityTokenHandler.CreateToken(securityDescriptor);
			session.Token = securityTokenHandler.WriteToken(securityToken);
			session.ExpiresIn = expiresIn.Value;
			session.ExpiresAt = expiresAt;

		}
	}
}
