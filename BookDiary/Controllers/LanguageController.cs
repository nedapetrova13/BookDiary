using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.LanguageViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILanguageService _service;

        public LanguageController(ILanguageService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            var languageList = _service.GetAll();

            var viewModelList = languageList.Select(news => new LanguageCreateViewModel
            {
                Name = news.Name,
                Id = news.Id
            }).ToList();

            return View(viewModelList);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Add()
        {
            var model = new LanguageCreateViewModel();
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Add(LanguageCreateViewModel lcvm)
        {
            if (lcvm.Name == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(lcvm);
            }
            else
            {
                bool isExists = _service.GetAll().Where(x => x.Name == lcvm.Name).Any();
                if (!isExists)
                {
                    var language = new Language
                    {
                        Name = lcvm.Name
                    };
                    await _service.Add(language);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Вече съществува такъв език";
                    return View(lcvm);
                }
            }
            
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id)
        {

            var language = await _service.GetById(id);

            var model = new LanguageEditViewModel
            {
                Name = language.Name
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Edit(LanguageEditViewModel levm)
        {
            if (levm.Name == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(levm);
            }
            else
            {
                bool isExists = _service.GetAll().Where(x => x.Name == levm.Name).Any();
                if (!isExists)
                {
                    var model = new Language
                    {
                        Id = levm.Id,
                        Name = levm.Name
                    };
                    await _service.Update(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Вече съществува такъв език";
                    return View(levm);
                }
            }
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);  
            return RedirectToAction("Index");
        }
    }
}
