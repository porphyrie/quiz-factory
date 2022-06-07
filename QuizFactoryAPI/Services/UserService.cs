using QuizFactoryAPI.Authorization;
using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Helpers;
using QuizFactoryAPI.Models.Users;
using BCryptNet = BCrypt.Net.BCrypt;

namespace QuizFactoryAPI.Services
{
    public interface IUserService
    {
        LoginResponse Login(LoginRequest model);
        void Register(RegisterRequest model);
        IEnumerable<User> GetAllUsers();
        User GetByUsername(string username);
    }

    public class UserService : IUserService
    {
        private QuizFactoryContext _context;
        private IJwtUtils _jwtUtils;

        public UserService(QuizFactoryContext context, IJwtUtils jwtUtils)
        {
            _context = context;
            _jwtUtils = jwtUtils;
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.Username == model.Username))
                throw new AppException("Username '" + model.Username + "' is already taken");

            // map model to new user object
            var user = new User() { 
                LastName = model.LastName,
                FirstName = model.FirstName,
                Username = model.Username,
                Role = model.Role
            };

            // hash password
            user.PasswordHash = BCryptNet.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public LoginResponse Login(LoginRequest model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username.Equals(model.Username));

            // validate
            if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash))
                throw new AppException("Username or password is incorrect");

            // authentication successful so generate jwt token
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new LoginResponse(user, jwtToken);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users;
        }

        public User GetByUsername(string username)
        {
            var user = _context.Users.Find(username);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user; //not ok, ca trebuie facut si aici un model
        }
    }
}
