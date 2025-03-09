using System;

namespace SignalRPractice.Entities
{
	public class Token
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string AccessToken { get; set; } = default;
		public string RefreshToken { get; set; } = default;
		public DateTime RefreshTokenPasswordExpiry { get; set; }
	}
}
