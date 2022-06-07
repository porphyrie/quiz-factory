using QuizFactoryAPI.Entities;

namespace QuizFactoryAPI.Models.Users
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public LoginResponse(User user, string token)
        {
            Username = user.Username;
            Role = user.Role;
            Token = token;
        }
    }
}
