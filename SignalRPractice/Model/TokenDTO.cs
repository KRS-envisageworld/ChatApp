using System;

namespace SignalRPractice.Model
{
	public class TokenDTO
	{
		public string AccessToken { get; set; } = default;
		public string RefreshToken { get; set; } = default;
		public DateTime RefreshTokenPasswordExpiry { get; set; }
	}
}
