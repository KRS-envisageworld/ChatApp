using Microsoft.AspNetCore.Mvc;

using SignalRPractice.Model;
using SignalRPractice.Services;

namespace SignalRPractice.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private ChatService chatService;
		public ChatController(ChatService _chatService)
		{
			chatService = _chatService;
		}

		[HttpPost("Register")]
		public IActionResult Register([FromBody] UserDto user)
		{
			if (chatService.AddUser(user.Name))
			{
				return Created();
			}
			return BadRequest("Please choose another name.");
		}
	}
}