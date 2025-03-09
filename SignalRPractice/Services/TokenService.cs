using Microsoft.EntityFrameworkCore;

using SignalRPractice.Entities;
using SignalRPractice.EntityContext;
using SignalRPractice.Interfaces;
using SignalRPractice.Model;

using System;
using System.Threading.Tasks;

namespace SignalRPractice.Services
{
	public class TokenService(ChatDBContext dBContext):IToken
	{
		public async Task<TokenDTO> GenerateAndSaveToken(string username, int userId)
		{
			TokenDTO newToken = GenerateToken(username);

			try
			{
				var token = await dBContext.Tokens.FirstOrDefaultAsync(f => f.UserId == userId);
				if (token == null)
				{
					token = new Token { UserId = userId };
				}
				token.RefreshToken = newToken.RefreshToken;
				token.RefreshTokenPasswordExpiry = newToken.RefreshTokenPasswordExpiry;
				token.AccessToken = newToken.AccessToken;
				await dBContext.SaveChangesAsync();
				return newToken;
			}
			catch
			{
				throw;
			}
		}
		private TokenDTO GenerateToken(string username)
		{
			TokenDTO token = new() { AccessToken = $"token {username}sd2f12as1df21ads4f68as7d68f465asddfasod0!*##==", RefreshToken = $"RefreshToken", RefreshTokenPasswordExpiry = DateTime.UtcNow.AddDays(1) };
			return token;
		}
	}
}