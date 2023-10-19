using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationAPI
{
	public class Utilities
	{
		public static string? GetJwtToken(HttpRequest httpRequest)
		{
			StringValues header = httpRequest.Headers.Authorization;
			if (header.Count == 0)
				return null;
			return header[0]?.Split(" ")[1];
		}

	}
}
