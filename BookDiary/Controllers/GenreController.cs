using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.GenreViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            var genreList = _genreService.GetAll();

            var viewModelList = genreList.Select(news => new GenreCreateViewModel
            {
                Name = news.Name,
                Id = news.Id 
            }).ToList();

            return View(viewModelList);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Add()
        {
            var model = new GenreCreateViewModel();
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Add(GenreCreateViewModel gcvm)
        {
            if (gcvm.Name == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(gcvm);
            }
            else
            {
                var isExists = _genreService.Get(x=>x.Name == gcvm.Name);
                if (isExists == null)
                {
                    var genre = new Genre
                    {
                        Name = gcvm.Name
                    };
                    await _genreService.Add(genre);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Този жанр вече съществува";
                    return View(gcvm);
                }
            }
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _genreService.GetById(id);
            var model = new GenreEditViewModel
            {
                Name = tag.Name
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Edit(GenreEditViewModel gevm) 
        {
            if (gevm.Name == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(gevm);
            }
            else
            {
                var isExists = _genreService.Get(x => x.Name == gevm.Name);
                if (isExists == null)
                {
                    var model = new Genre
                    {
                        Id = gevm.Id,
                        Name = gevm.Name
                    };
                    await _genreService.Update(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Вече съществува такъв жанр";
                    return View(gevm);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _genreService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
