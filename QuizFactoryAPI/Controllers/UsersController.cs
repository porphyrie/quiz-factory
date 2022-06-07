using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizFactoryAPI.Authorization;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Models.Users;
using QuizFactoryAPI.Services;

namespace QuizFactoryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _userService.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest model)
        {
            var response = _userService.Login(model);
            return Ok(response);
        }

        [Authorize(Role.admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{username}")] //de modif
        public IActionResult GetByUsername(string username)
        {
            // only admins can access other user records
            var currentUser = (User)HttpContext.Items["User"];
            if (username != currentUser.Username && currentUser.Role != Role.admin.ToString())
                return Unauthorized(new { message = "Unauthorized" });

            var user = _userService.GetByUsername(username);
            return Ok(user);
        }
    }
}
