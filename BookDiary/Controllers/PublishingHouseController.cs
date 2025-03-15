using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.TagViewModels;
using BookDiary.Models.ViewModels.PublishingHouseViewModels;
using Microsoft.AspNetCore.Mvc;
using BookDiary.Models.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BookDiary.Controllers
{
    public class PublishingHouseController : Controller
    {
        private readonly IPublishingHouseService _publishingHouseService;

        public PublishingHouseController(IPublishingHouseService publishingHouseService)
        {
            _publishingHouseService = publishingHouseService;
        }

        public  IActionResult Index()
        {
            var pubhouseList = _publishingHouseService.GetAll();

            var viewModelList = pubhouseList.Select(news => new PublishingHouseCreateViewModel
            {
                Name = news.Name,
                YearFounded = news.YearFounded,
                Id = news.Id // Ensure Id is mapped for Edit/Delete actions
            }).ToList();

            return View(viewModelList);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Add()
        {
            var model = new PublishingHouseCreateViewModel();
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Add(PublishingHouseCreateViewModel phcvm)
        {
            var pubhouse = new PublishingHouse
            {
                Name = phcvm.Name,
                YearFounded = phcvm.YearFounded,
            };
            await _publishingHouseService.Add(pubhouse);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id)
        {
            PublishingHouse phouse = await _publishingHouseService.GetById(id);
            var model = new PublishingHouseEditViewModel
            {
                Name = phouse.Name,
                YearFounded = phouse.YearFounded,
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Edit(PublishingHouseEditViewModel phevm)
        {
            var model = new PublishingHouse
            {
                Id = phevm.Id,
                Name = phevm.Name,
            };
            await _publishingHouseService.Update(model);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _publishingHouseService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
