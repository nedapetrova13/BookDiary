using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class SeriesController : Controller
    {
        private readonly ISeriesService _seriesService;

        public SeriesController(ISeriesService seriesService)
        {
            _seriesService = seriesService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _seriesService.GetAllSeries();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Series series)
        {
            await _seriesService.Add(series);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {

            var series = await _seriesService.GetById(id);
            return View(series);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Series series)
        {
            if (ModelState.IsValid)
            {
                await _seriesService.Update(series);
                return RedirectToAction("Index");
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _seriesService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
