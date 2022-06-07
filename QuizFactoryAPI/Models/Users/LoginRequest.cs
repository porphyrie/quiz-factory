using System.ComponentModel.DataAnnotations;

namespace QuizFactoryAPI.Models.Users
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
