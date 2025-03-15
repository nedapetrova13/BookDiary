using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.NewsViewModels;
using BookDiary.Models.ViewModels.TagViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookDiary.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IActionResult Index()
        {
            var tagList = _tagService.GetAll();

            var viewModelList = tagList.Select(news => new TagCreateViewModel
            {
                Name = news.Name,
                Id = news.Id // Ensure Id is mapped for Edit/Delete actions
            }).ToList();

            return View(viewModelList);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Add()
        {
            var model = new TagCreateViewModel();
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Add(TagCreateViewModel tcvm)
        {
            var tag = new Tag
            {
                Name = tcvm.Name,
            };
            await _tagService.Add(tag);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id)
        {
            Tag tag = await _tagService.GetById(id);
            var model = new TagEditViewModel
            {
                Name = tag.Name,
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Edit(TagEditViewModel tagModel)
        {
            var model = new Tag
            {
                Id = tagModel.Id,
                Name = tagModel.Name,
            };
            await _tagService.Update(model);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
