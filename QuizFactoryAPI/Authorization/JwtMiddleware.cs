using QuizFactoryAPI.Services;

namespace QuizFactoryAPI.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var username = jwtUtils.ValidateJwtToken(token);
            if (username != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetByUsername(username); //autentificare -> pui aici datele de care ai nevoie la autorizare gen 
            }

            await _next(context);
        }
    }
}
