using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.CityViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
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

        public IActionResult Index()
        {
            var cityList = _cityService.GetAll();

            var viewModelList = cityList.Select(news => new CityCreateViewModel
            {
                Name = news.Name,
                Id = news.Id // Ensure Id is mapped for Edit/Delete actions
            }).ToList();

            return View(viewModelList); 
        }
        public IActionResult Add()
        {
            var model = new CityCreateViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CityCreateViewModel ccvm)
        {
            var city = new City
            {
                Name = ccvm.Name
            };
            await _cityService.Add(city);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var city = await _cityService.GetById(id);
            var model = new CityEditViewModel
            {
                Name = city.Name
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CityEditViewModel cevm)
        {
            var city = new City
            {
                Id = cevm.Id,
                Name = cevm.Name
            };
                await _cityService.Update(city);
                return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _cityService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
