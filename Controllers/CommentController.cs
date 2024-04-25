using AutoMapper;
using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kuchenne_rewolucje.Controllers
{
    [Authorize]
    public class CommentController(ICommentRepository commentRepository, IMapper mapper) : Controller
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentDto dto)
        {
            try
            {
                dto.CreatedAt = DateTime.Now;
                await _commentRepository.AddAsync(_mapper.Map<Comment>(dto));
                TempData["SuccessMessage"] = $"Poprawnie zamieszczono komentarz.";
                return RedirectToAction("SingleArticle", "Article", new { id = dto.ArticleId });

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd dodawania komentarza. {ex.Message}.";
                return RedirectToAction("SingleArticle", "Article", new { id = dto.ArticleId });

            }
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                Comment comment = await _commentRepository.GetAsync(id);
                _commentRepository.Detach(comment);
                await _commentRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Poprawnie usunięto komentarz.";
                return RedirectToAction("SingleArticle", "Article", new { id = comment.ArticleId });

            }
            catch (Exception ex)
            {
                Comment comment = await _commentRepository.GetAsync(id);
                TempData["ErrorMessage"] = $"Błąd usuwania komentarza. {ex.Message}.";
                return RedirectToAction("SingleArticle", "Article", new { id = comment.ArticleId });

            }
        }
    }
}