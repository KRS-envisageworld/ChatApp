using SignalRPractice.Entities;
using SignalRPractice.Model;
using SignalRPractice.Model.RequestDTO;

using System.Threading.Tasks;

namespace SignalRPractice.Interfaces
{
	public interface IUser
	{
		Task<string[]> RegisterAsync(UserRequestDTO newUser);

		Task<User> GetUserByUsername(string username);

		Task<User> GetUserByUserId(int userId);

		Task<TokenDTO> ValidateUser(string username, string password);
	}
}
