using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestContainerWebApi.Auth.AuthModel;
using TestContainerWebApi.Auth.Helpers;
using TestContainerWebApi.Auth.dbAuth;

namespace TestContainerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        public AuthController(AuthDbContext context)
        {
            _context = context;
        }

        //POST: account/login
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] EntityLoginModel data)
        {
            // receive from form email and password
            // if email and/or password not set, send error code 400
            if (data.Email == "" || data.Password == "")
                return BadRequest("Email and/or password not set");

            string email = data.Email;
            string incomePassword = data.Password;

            // find user
            UserAuth user = await _context.GetUserByEmailAuth(email);


            // if user not found, send 401
            if (user is null)
                return Unauthorized("Access denied.  User is not registered.");

            if (!PasswordHasher.VerifyPassword(incomePassword, user.Password))
                return Unauthorized("Access denied. Incorrect password.");

            await Authenticate(user);


            return Ok(new { message = $"User with email: {email} and Role: {user.Role?.Name} successfully logged" });
        }


        // GET: account/logout
        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "The user has logged out" });
        }


        //POST: account/registration
        [Route("registration")]
        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] EntityRegisterModel data)
        {
            // receive from form email and password
            // if email and/or password not set, send error code 400
            if (data.Email == "" || data.Password == "" || data.ConfirmPassword == "" || data.Password != data.ConfirmPassword)
                return BadRequest("Email and/or password not set");
            string email = data.Email;
            string incomePassword = data.Password;
            string hashedPassword = PasswordHasher.HashPassword(incomePassword);

            // find user
            UserAuth user = await _context.GetUserByEmailAuth(email);

            // if user not found, registration
            if (user is null)
            {
                user = new UserAuth { Email = email, Password = hashedPassword };
                RoleAuth userRole = await _context.GetRoleByNameAuth("user");

                if (userRole != null)
                {
                    user.RoleId = userRole.Id;
                     user.Role = userRole;
                }

                int userId = await _context.CreateUserAuth(user);


                await Authenticate(user);
            }
            else
            {

                return Ok(new { message = "User is already registered" });
            }

            return Ok(new { message = $"User with email: {email} successfully registered" });
        }

        private async Task Authenticate(UserAuth user)
        {
            // create one claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // create object ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // set cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
