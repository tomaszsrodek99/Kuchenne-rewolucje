using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Kuchenne_rewolucje.Controllers
{
    public class AuthController(IUserRepository userRepository, IConfiguration config) : Controller
    {
        private readonly IUserRepository _repository = userRepository;
        private readonly IConfiguration _config = config;

        [HttpGet]
        public async Task<IActionResult> ProfilesView()
        {
            try
            {
                List<UserDto> users = await _repository.GetUsers();
                return View("Profiles", users);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public IActionResult LoginView(string username)
        {
            return View("Login", new UserLoginRequest { Username = username });
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            try
            {
                var user = await _repository.GetUserByEmail(request.Username);

                if (user == null)
                {
                    ModelState.AddModelError("Email", "Użytkownik o podanej nazwie nie istnieje.");
                    return View("Login", request);
                }

                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    ModelState.AddModelError("Password", "Niepoprawne hasło.");
                    return View("Login", request);
                }

                string token = CreateToken();
                var responseDto = new UserLoginResponseDto() { Token = token, User = user };

                var jwtCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false
                };
                Response.Cookies.Append("JWTToken", responseDto.Token, jwtCookie);

                var userIdCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false
                };
                Response.Cookies.Append("UserId", user.Id.ToString(), userIdCookie);

                var username = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false
                };
                Response.Cookies.Append("Username", user.Username, username);

                //HttpContext.Session.SetString("UserId", user.Id.ToString());
                //HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Article");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Błąd logowania. {ex.Message}.";
                return View("Login");
            }
        }

        [Authorize]
        public IActionResult RegisterView()
        {
            return View("Register", new UserRegisterRequestDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Register(UserRegisterRequestDto request)
        {
            try
            {
                if (await _repository.UserExists(request.Username))
                {
                    ModelState.AddModelError("Username", "Użytkownik o podanej nazwie istnieje.");
                    return View("Register", request);
                }

                _repository.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                await _repository.AddAsync(new User()
                {
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Username = request.Username
                });
                //TempData["SuccessMessage"] = $"Poprawna rejestracja. Proszę się zalogować.";
                TempData["SuccessMessage"] = $"Poprawna rejestracja.";
                //return RedirectToAction("ProfilesView");
                return RedirectToAction("Index", "Article");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Błąd logowania. {ex.Message}.";
                return View("Register");
            }
        }

        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("JWTToken");
            return RedirectToAction("ProfilesView");
        }

        private string CreateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}