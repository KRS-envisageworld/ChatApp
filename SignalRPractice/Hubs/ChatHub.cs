using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;

using SignalRPractice.Model;
using SignalRPractice.Services;

using System;
using System.Globalization;
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

		public async Task CreatePrivateChat(MessageDto message)
		{
			string groupName = GetPrivateGroupName(message.From, message.To);
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

			string toUser = chatService.GetConnectionIdByUser(message.To);
			await Groups.AddToGroupAsync(toUser, groupName);

			await Clients.Client(toUser).SendAsync("OpenPriavteChat", message);

		}

		public async Task	RecivePrivateMessage(MessageDto message)
		{
			string groupName = GetPrivateGroupName(message.From, message.To);
			await Clients.Groups(groupName).SendAsync("NewPrivateMessage", message);
		}

		public async Task RemovePriavteChat(string from, string to)
		{
			string groupName = GetPrivateGroupName(from, to);
			await Clients.Groups(groupName).SendAsync("RemovePrivateChat");

			await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
			string user = chatService.GetConnectionIdByUser(to);

			await Groups.RemoveFromGroupAsync(user, groupName);

		}

		public string GetPrivateGroupName(string from, string to)
		{
			var stringCompare = string.CompareOrdinal(from, to) > 0;
			return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
		}
	}
}