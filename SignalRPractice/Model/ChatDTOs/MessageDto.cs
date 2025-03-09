using System.ComponentModel.DataAnnotations;

namespace SignalRPractice.Model
{
	public class MessageDto
	{
		[Required]
		public string Message { get; set; }
		[Required]
		public string From { get; set; }
		public string To { get; set; } = "";
	}
}