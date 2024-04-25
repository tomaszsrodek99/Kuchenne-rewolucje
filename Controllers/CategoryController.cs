using AutoMapper;
using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kuchenne_rewolucje.Controllers
{
    [Authorize]
    public class CategoryController(ICategoryRepository categoryRepository, IMapper mapper) : Controller
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View("GetAllCategoriesWithStats", _mapper.Map<List<CategoryDto>>(categories));
        }

        [HttpGet]
        public async Task<List<Category>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesWithStats()
        {
            var categories = await _categoryRepository.GetCategoriesWithStats();
            categories = [.. categories.OrderBy(x => x.Name)];
            return View("CategoryList", categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto dto)
        {
            try
            {
                if (!await _categoryRepository.CategoryExists(dto.Name))
                {
                    await _categoryRepository.AddAsync(_mapper.Map<Category>(dto));
                    TempData["SuccessMessage"] = $"Poprawnie dodano kategorię.";
                    return RedirectToAction("CreateArticle", "Article");
                }
                ModelState.AddModelError("Name", "Kategoria o takiej nazwie istnieje.");
                return View("CreateCategory", dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd dodawania kategori. {ex.Message}.";
                return RedirectToAction("CreateArticle", "Article");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CategoryExists([FromQuery] string name)
        {
            try
            {
                if (!await _categoryRepository.CategoryExists(name))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd serwera. {ex.Message}";
                return RedirectToAction("Index", "Article");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategoryView(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            return View("UpdateCategory", _mapper.Map<CategoryDto>(category));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryDto dto)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(dto.Id);
                if (category.Name != dto.Name)
                {
                    if (await _categoryRepository.CategoryExists(dto.Name))
                    {
                        ModelState.AddModelError("Name", "Kategoria o takiej nazwie już istnieje.");
                        return View("UpdateCategory", dto);
                    }
                    else
                    {
                        _categoryRepository.Detach(category);
                        await _categoryRepository.UpdateAsync(_mapper.Map<Category>(dto));
                        return RedirectToAction("GetAllCategoriesWithStats");
                    }
                }
                else
                {
                    TempData["SuccessMessage"] = $"Nazwa kategorii nie uległa zmianie.";
                    return View("UpdateCategory", dto);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd edytowania kategori. {ex.Message}.";
                return View("UpdateCategory", dto);
            }
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Poprawnie usunięto kategorię.";
                return RedirectToAction("GetAllCategoriesWithStats");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd usuwania kategori. {ex.Message}.";
                return RedirectToAction("GetAllCategoriesWithStats");
            }
        }
    }
}