﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SignalRPractice.Model
{
	public class UserDTO
	{
		public int Id { get; set; }
		[Required]
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool EmailVerified { get; set; } = false;
		public DateTime CreatedOn { get; set; } = DateTime.Now;
	}
}
