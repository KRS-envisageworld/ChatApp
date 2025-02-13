using System.Collections.Generic;
using System.Linq;

namespace SignalRPractice.Services
{
	public class ChatService
	{
		private static readonly Dictionary<string, string> Users = new Dictionary<string, string>();

		public bool AddUser(string newUser)
		{
			lock (Users)
			{
				foreach (string user in Users.Keys)
				{
					if (user.ToLower() == newUser.ToLower())
					{
						return false;
					}
				}
				Users.Add(newUser, null);
				return true;
			}
		}

		public void AddConnectionId(string user, string connectionId)
		{
			lock (Users)
			{
				if (Users.ContainsKey(user))
					Users[user] = connectionId;
			}
		}

		public string GetUserByConnectionId(string connectionId)
		{
			lock (Users)
			{
				return Users.Where(w => w.Value == connectionId).FirstOrDefault().Key;
			}
		}

		public string GetConnectionIdByUser(string user)
		{
			lock (Users)
			{
				return Users.Where(w => w.Key == user).FirstOrDefault().Value;
			}
		}

		public void RemoveUser(string user)
		{
			lock (Users)
			{
				if (Users.ContainsKey(user))
				{
					Users.Remove(user);
				}
			}
		}

		public string[] GetOnlineUsers()
		{
			lock (Users)
			{
				return Users.OrderBy(o => o.Key).Select(s => s.Key).ToArray();
			}
		}
	}
}