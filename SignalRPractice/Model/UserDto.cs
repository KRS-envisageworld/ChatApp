using System.ComponentModel.DataAnnotations;

namespace SignalRPractice.Model
{
	public class UserDto
	{
		[Required]
		public string Name { get; set; }
	}
}
