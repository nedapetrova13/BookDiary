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
                Id = news.Id 
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
            if (tcvm.Name == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(tcvm);
            }
            else
            {
                bool isExists = _tagService.GetAll().Where(x => x.Name == tcvm.Name).Any();
                if (!isExists)
                {
                    var tag = new Tag
                    {
                        Name = tcvm.Name,
                    };
                    await _tagService.Add(tag);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Вече съществува такава характеристика";
                    return View(tcvm);
                }
            }
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
            if (tagModel.Name == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(tagModel);
            }
            else
            {
                bool isExists = _tagService.GetAll().Where(x => x.Name == tagModel.Name).Any();
                if (!isExists)
                {
                    var model = new Tag
                    {
                        Id = tagModel.Id,
                        Name = tagModel.Name,
                    };
                    await _tagService.Update(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Вече съществува такава характеристика";
                    return View(tagModel);
                }
            } 
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
