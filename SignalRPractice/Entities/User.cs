using System;
using System.ComponentModel.DataAnnotations;

namespace SignalRPractice.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool EmailVerified { get; set; } = false;
		public DateTime CreatedOn { get; set; } = DateTime.Now;
	}
}
