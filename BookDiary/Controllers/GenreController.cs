using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.GenreViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public IActionResult Index()
        {
            var genreList = _genreService.GetAll();

            var viewModelList = genreList.Select(news => new GenreCreateViewModel
            {
                Name = news.Name,
                Id = news.Id // Ensure Id is mapped for Edit/Delete actions
            }).ToList();

            return View(viewModelList);
        }

        public IActionResult Add()
        {
            var model = new GenreCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GenreCreateViewModel gcvm)
        {
            var genre = new Genre
            {
                Name = gcvm.Name
            };
            await _genreService.Add(genre);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _genreService.GetById(id);
            var model = new GenreEditViewModel
            {
                Name = tag.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GenreEditViewModel gevm) 
        {
            var model = new Genre
            {
                Id = gevm.Id,
                Name = gevm.Name
            };
            await _genreService.Update(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _genreService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
