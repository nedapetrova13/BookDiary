using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.TagViewModels;
using BookDiary.Models.ViewModels.SeriesViewModels;
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

        public IActionResult Index()
        {
            var list =  _seriesService.GetAll();
            return View(list);
        }
        public IActionResult Add()
        {
            var model = new SeriesCreateViewModel();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(SeriesCreateViewModel scvm)
        {
            var series = new Series
            {
                Title = scvm.Title,
                Description = scvm.Description,
            };
            await _seriesService.Add(series);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            Series series = await _seriesService.GetById(id);
            var model = new SeriesEditViewModel
            {
                Title = series.Title,
                Description = series.Description,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SeriesEditViewModel sevm)
        {
            var model = new Series
            {
                Id = sevm.Id,
                Title = sevm.Title,
                Description = sevm.Description,
            };
            await _seriesService.Update(model);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _seriesService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
