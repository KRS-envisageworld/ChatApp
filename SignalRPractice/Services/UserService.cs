using Microsoft.EntityFrameworkCore;

using SignalRPractice.Entities;
using SignalRPractice.EntityContext;
using SignalRPractice.Interfaces;
using SignalRPractice.Model;
using SignalRPractice.Model.RequestDTO;

using System;
using System.Threading.Tasks;

namespace SignalRPractice.Services
{
	public class UserService : IUser
	{

		private readonly ChatDBContext _dbContext;
		private readonly IToken _tokenService;

		public UserService(ChatDBContext dBContext, IToken tokenService)
		{
			_dbContext = dBContext;
			_tokenService = tokenService;
		}


		public async Task<string[]> RegisterAsync(UserRequestDTO newUser)
		{
			try
			{
				if (await _dbContext.Users.AnyAsync(f => f.Username == newUser.Username))
				{
					return ["Username is already in use. Please choose another."];
				}
				if (await _dbContext.Users.AnyAsync(f => f.Email == newUser.Email))
				{
					return ["Email address is already in use."];
				}
				User user = new User
				{
					Username = newUser.Username,
					PasswordHash = GetHashPassword(newUser.Password),
					Email = newUser.Email,
					FirstName = newUser.FirstName,
					LastName = newUser.LastName,
				};

				await _dbContext.Users.AddAsync(user);
				await _dbContext.SaveChangesAsync();

				return [];
			}
			catch
			{
				throw;
			}
		}

		public async Task<User> GetUserByUsername(string username)
		{
			return await _dbContext.Users.FirstOrDefaultAsync(f => f.Username == username);
		}

		public async Task<User> GetUserByUserId(int userId)
		{
			return await _dbContext.Users.FirstOrDefaultAsync(f => f.Id == userId);
		}

		public async Task<TokenDTO> ValidateUser(string username, string password)
		{
			try
			{
				var User = await _dbContext.Users.FirstOrDefaultAsync(f => f.Username == username);
				if (User != null)
				{
					if (User.PasswordHash == GetHashPassword(password))
					{
						var token = await _tokenService.GenerateAndSaveToken(username, User.Id);
						return token;
					}
				}
				return null;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private string GetHashPassword(string password)
		{
			return password;
		}
	}
}
