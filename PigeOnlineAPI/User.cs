using System.ComponentModel.DataAnnotations;

namespace PigeOnlineAPI.Controllers
{
    public class User
    {
        [Key, Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string DisplayName { get; set; }
        public List<Chat> Chats=new List<Chat>();

    }
}
