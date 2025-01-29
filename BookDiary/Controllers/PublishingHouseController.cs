using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class PublishingHouseController : Controller
    {
        private readonly IPublishingHouseService _publishingHouseService;

        public PublishingHouseController(IPublishingHouseService publishingHouseService)
        {
            _publishingHouseService = publishingHouseService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _publishingHouseService.GetAllPublishingHouses();
            return View(list);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PublishingHouse ph)
        {
            await _publishingHouseService.Add(ph);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ph = await _publishingHouseService.GetById(id);
            return View(ph);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PublishingHouse ph)
        {
            await _publishingHouseService.Update(ph);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _publishingHouseService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
