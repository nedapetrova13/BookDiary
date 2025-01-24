using BookDiary.Core.IServices;
using BookDiary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
             var list =await  _tagService.GetAllTags();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Tag tag)
        {
            _tagService.Add(tag);
            return RedirectToAction("Index");
        }
    }
}
