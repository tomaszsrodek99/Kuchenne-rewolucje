using AutoMapper;
using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kuchenne_rewolucje.Controllers
{
    [Authorize]
    public class UserController(IUserRepository userRepository, IConfiguration config, IMapper mapper, IArticleRepository articleRepository) : Controller
    {
        private readonly IUserRepository _repository = userRepository;
        private readonly IArticleRepository _articleRepository = articleRepository;
        private readonly IConfiguration _config = config;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> UserData(int id)
        {
            User user = await _repository.GetAsync(id);
            ViewBag.NumberOfArticle = await _articleRepository.GetNumberOfArticlesForUser(id);
            ViewBag.NumberOfComments = await _articleRepository.GetNumberOfCommentsForUser(id);
            ViewBag.NumberOfRatings = await _articleRepository.GetNumberOfRatingsForUser(id);
            return View("ChangeData", _mapper.Map<UserDto>(user));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUsername(UserDto dto)
        {
            try
            {
                User user = await _repository.GetAsync(dto.Id);
                if (user.Username == dto.Username)
                {
                    ModelState.AddModelError("Username", "Nazwa użytkownika nie uległa zmianie.");
                    return View("ChangeData", _mapper.Map<UserDto>(user));
                }
                else if (await _repository.UserExists(dto.Username))
                {
                    {
                        ModelState.AddModelError("Username", "Użytkownik o podanej nazwie istnieje.");
                        return View("ChangeData", _mapper.Map<UserDto>(user));
                    }
                }
                else
                {
                    user.Username = dto.Username;
                    await _repository.UpdateAsync(user);
                    TempData["SuccessMessage"] = $"Poprawnie zmieniono nazwę użytkownika.";
                }
                return RedirectToAction("UserData", new { id = dto.Id });
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Błąd zmiany nazwy użytkownika. {ex.Message}.";
                return RedirectToAction("UserData", new { id = dto.Id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserRegisterRequestDto dto)
        {
            try
            {
                _repository.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                User user = await _repository.GetAsync(dto.Id);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await _repository.UpdateAsync(user);
                TempData["SuccessMessage"] = $"Poprawnie zmieniono hasło użytkownika.";
                return RedirectToAction("UserData", new { id = dto.Id });
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Błąd zmiany hasła. {ex.Message}.";
                return RedirectToAction("UserData", new { id = dto.Id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeImage(UserDto dto)
        {
            try
            {
                User user = await _repository.GetAsync(dto.Id);
                AddImage(dto.ImageFile, out byte[] imageUrl);
                user.ImageUrl = imageUrl;
                await _repository.UpdateAsync(user);
                TempData["SuccessMessage"] = $"Poprawnie zmieniono zdjęcie profilowe.";
                return RedirectToAction("UserData", new { id = dto.Id });
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Błąd zmiany zdjęcia. {ex.Message}.";
                return RedirectToAction("UserData", new { id = dto.Id });
            }
        }
        private static void AddImage(IFormFile imageFile, out byte[] ImageUrl)
        {
            using var memoryStream = new MemoryStream();
            imageFile.CopyTo(memoryStream);
            ImageUrl = memoryStream.ToArray();
        }
    }
}