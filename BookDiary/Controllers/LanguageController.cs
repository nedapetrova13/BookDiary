using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.LanguageViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
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

        public IActionResult Index()
        {
            var languageList = _service.GetAll();

            var viewModelList = languageList.Select(news => new LanguageCreateViewModel
            {
                Name = news.Name,
                Id = news.Id // Ensure Id is mapped for Edit/Delete actions
            }).ToList();

            return View(viewModelList);
        }
        public IActionResult Add()
        {
            var model = new LanguageCreateViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(LanguageCreateViewModel lcvm)
        {
            var language = new Language
            {
                Name = lcvm.Name
            };
            await _service.Add(language);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {

            var language = await _service.GetById(id);
            var model = new LanguageEditViewModel
            {
                Name = language.Name
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(LanguageEditViewModel levm)
        {
            var model = new Language
            {
                Id = levm.Id,
                Name = levm.Name
            };
            await _service.Update(model);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);  
            return RedirectToAction("Index");
        }
    }
}
