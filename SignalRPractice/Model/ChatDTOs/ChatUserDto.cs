using System.ComponentModel.DataAnnotations;

namespace SignalRPractice.Model.ChatDTOs
{
    public class ChatUserDto
    {
        [Required]
        public string Name { get; set; }
    }
}
