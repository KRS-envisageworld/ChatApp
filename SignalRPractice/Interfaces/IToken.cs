using SignalRPractice.Model;
using System.Threading.Tasks;

namespace SignalRPractice.Interfaces
{
	public interface IToken
	{
		Task<TokenDTO> GenerateAndSaveToken(string username, int userId);
	}
}
