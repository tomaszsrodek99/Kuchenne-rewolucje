﻿using AutoMapper;
using SkiaSharp;
using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kuchenne_rewolucje.Context;
using Microsoft.Extensions.Options;
using System.IO;

namespace Kuchenne_rewolucje.Controllers
{
    [Authorize]
    public class ArticleController(ICategoryRepository categoryRepository, IConfiguration config, IMapper mapper, IWebHostEnvironment webHostEnvironment, IArticleRepository articleRepository,
        IUserRepository userRepository, IFavouriteRepository favouritesRepository, IRatingRepository ratingRepository) : Controller
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IArticleRepository _articleRepository = articleRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IFavouriteRepository _favouritesRepository = favouritesRepository;
        private readonly IRatingRepository _ratingRepository = ratingRepository;
        private readonly IConfiguration _config = config;
        private readonly IMapper _mapper = mapper;
        private readonly IWebHostEnvironment _env = webHostEnvironment;

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryid)
        {
            try
            {
                List<ArticleDto> articles;
                if (categoryid == null)
                {
                    articles = await _articleRepository.GetArticles(null);
                }
                else
                {
                    articles = await _articleRepository.GetArticles(categoryid);
                }
                articles = [.. articles.OrderBy(x => x.CreatedAt)];
                return View(_mapper.Map<List<ArticleDto>>(articles));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SingleArticle(int id)
        {
            try
            {
                var userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
                var article = await _articleRepository.GetAsync(id);
                if (article.Ratings.Any())
                    ViewBag.Rate = await _ratingRepository.GetUserRate(userId, article.Id);
                else
                    ViewBag.Rate = 0;
                return View("Details", _mapper.Map<ArticleDto>(article));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd ładowania przepisu. {ex.Message}.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyRecipes()
        {
            try
            {
                var id = int.Parse(HttpContext.Request.Cookies["UserId"]);
                var myRecipes = await _articleRepository.GetArticlesByUserId(id);
                return View("UserRecipes", myRecipes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd ładowania przepisów. {ex.Message}.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateArticle(int id)
        {
            try
            {
                ViewBag.Categories = await _categoryRepository.GetCategories();
                return View("CreateArticle", new ArticleDto { UserId = id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd dodawania przepisu. {ex.Message}.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleDto dto)
        {
            try
            {
                if (dto.ImageFile == null)
                    return View("CreateArticle", dto);

                dto.ImageUrl = AddImage(dto.ImageFile, null);
                dto.CreatedAt = DateTime.Now;
                await _articleRepository.AddAsync(_mapper.Map<Article>(dto));
                TempData["SuccessMessage"] = $"Poprawnie dodano przepis.";
                return RedirectToAction("MyRecipes");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd dodawania przepisu. {ex.Message}.";
                return RedirectToAction("CreateArticle", dto.UserId);
            }
        }

        private static string AddImage(IFormFile imageFile, string? latestImage)
        {
            if (!string.IsNullOrEmpty(latestImage))
            {
                string imagePath = Path.Combine("wwwroot", "img", latestImage);
                FileInfo file = new(imagePath);
                if (file.Exists)
                {
                    file.Delete();
                }
            }

            string fileName = $"{DateTime.Now:yyyyMMddHHmmss}-{Path.GetFileName(imageFile.FileName)}";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

            using (var stream = imageFile.OpenReadStream())
            {
                using var fileStream = new FileStream(filePath, FileMode.Create);
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            return fileName;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateArticleView(int id)
        {
            try
            {
                var article = await _articleRepository.GetAsync(id);
                ViewBag.Categories = await _categoryRepository.GetCategories();
                return View("UpdateArticle", _mapper.Map<ArticleDto>(article));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd ładowania widoku. {ex.Message}.";
                return RedirectToAction("SingleArticle", id);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateArticle(ArticleDto dto)
        {
            try
            {

                var article = await _articleRepository.GetAsync(dto.Id);
                dto.CreatedAt = article.CreatedAt;

                if (dto.ImageFile != null)
                {
                    dto.ImageUrl = AddImage(dto.ImageFile, article.ImageUrl);
                }
                else
                {
                    dto.ImageUrl = article.ImageUrl;
                }
                _articleRepository.Detach(article);
                await _articleRepository.UpdateAsync(_mapper.Map<Article>(dto));
                TempData["SuccessMessage"] = "Poprawnie edytowano przepis.";
                return RedirectToAction("MyRecipes");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd edytowania przepisu: {ex.Message}";
                return RedirectToAction("MyRecipes");
            }
        }

        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                var article = await _articleRepository.GetAsync(id);

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", article.ImageUrl);
                FileInfo file = new(path);
                file.Delete();

                _articleRepository.Detach(article);
                await _articleRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Poprawnie usunięto przepis.";
                return RedirectToAction("MyRecipes");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd usuwania przepisu. {ex.Message}.";
                return RedirectToAction("MyRecipes");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFavouriteArticle([FromQuery] int id)
        {
            try
            {
                var userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
                var user = await _userRepository.GetAsync(userId);

                var existingFavourite = user.Favourites.FirstOrDefault(f => f.ArticleId == id);

                if (existingFavourite != null)
                {
                    await _favouritesRepository.DeleteAsync(existingFavourite.Id);
                    return Json(new { success = true });
                }
                else
                {
                    await _favouritesRepository.AddAsync(new FavouritesArticle { ArticleId = id, UserId = user.Id });
                }

                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd serwera. {ex.Message}";
                return RedirectToAction("Index", "Article");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRating(Rating dto)
        {
            try
            {
                if (dto.Value == 0 && dto.Id != 0)
                {
                    await _ratingRepository.DeleteAsync(dto.Id);
                }
                await _ratingRepository.UpdateAsync(dto);
                TempData["SuccessMessage"] = "Poprawnie dodano ocenę.";
                return RedirectToAction("SingleArticle", dto.ArticleId);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd dodawania oceny: {ex.Message}";
                return RedirectToAction("SingleArticle", dto.ArticleId);
            }
        }


    }
}