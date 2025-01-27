using BookDiary.Core.IServices;
using BookDiary.Models;
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

        public async Task<IActionResult> Index()
        {
            var list = await  _service.GetAllLanguages();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Language language)
        {
            await _service.Add(language);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var language = await _service.GetById(id);
            return View(language);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Language language)
        {
            await _service.Update(language);
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
