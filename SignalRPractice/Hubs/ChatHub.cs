using Microsoft.AspNetCore.SignalR;

using SignalRPractice.Model;
using SignalRPractice.Services;

using System;
using System.Threading.Tasks;

namespace SignalRPractice.Hubs
{
	public class ChatHub : Hub
	{
		private ChatService chatService;
		public ChatHub(ChatService _chatService)
		{
			chatService = _chatService;
		}

		public override async Task OnConnectedAsync()
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, "Techies");
			await Clients.Caller.SendAsync("NewUserJoined");
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Techies");

			var user = chatService.GetUserByConnectionId(Context.ConnectionId);
			chatService.RemoveUser(user);

			await DispalyOnlineUser();

			await base.OnDisconnectedAsync(exception);
		}

		public async Task AddUserConnectionId(string user)
		{
			chatService.AddConnectionId(user, Context.ConnectionId);
			 
			await DispalyOnlineUser();
		}

		public async Task RecieveMessage(MessageDto message)
		{
			await Clients.Groups("Techies").SendAsync("NewMessage", message);
		}

		public async Task DispalyOnlineUser()
		{
			var onlineUsers = chatService.GetOnlineUsers();
			await Clients.Groups("Techies").SendAsync("UpdateOnlineUsers", onlineUsers);
		}
	}
}