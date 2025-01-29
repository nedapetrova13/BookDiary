using BookDiary.Core.IServices;
using BookDiary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _cityService.GetAllCities();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(City city)
        {
            await _cityService.Add(city);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {

            var city = await _cityService.GetById(id);
            return View(city);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(City city)
        {
            if (ModelState.IsValid)
            {
                await _cityService.Update(city);
                return RedirectToAction("Index");
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _cityService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
