using System.ComponentModel.DataAnnotations;

namespace FiqhBot.Models
{
    public class ChatViewModel
    {
        [Required(ErrorMessage = "لطفا پیام خود را وارد کنید")]
        [StringLength(1000, ErrorMessage = "پیام نمی‌تواند بیش از 1000 کاراکتر باشد")]
        public string Message { get; set; } = string.Empty;

        public List<ChatMessage> Messages { get; set; } = new();

        public string SelectedTarget { get; set; }
        public List<string> SelectedResources { get; set; } = new();
    }

    public class ChatMessage
    {
        public string Message { get; set; } = string.Empty;
        public string Role { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
