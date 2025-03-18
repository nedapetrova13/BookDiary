using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.TagViewModels;
using BookDiary.Models.ViewModels.SeriesViewModels;
using Microsoft.AspNetCore.Mvc;
using BookDiary.Models.ViewModels.NewsViewModels;
using BookDiary.Models.ViewModels.BookViewModels;
using System.Net;
using System.Data.Entity;
using Microsoft.AspNetCore.Authorization;

namespace BookDiary.Controllers
{
    public class SeriesController : Controller
    {
        private readonly ISeriesService _seriesService;
        private readonly IBookService _bookService;

        public SeriesController(ISeriesService seriesService,IBookService bookService)
        {
            _seriesService = seriesService;
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            var seriesList = _seriesService.GetAll();

            var viewModelList = seriesList.Select(news => new SeriesCreateViewModel
            {
                Title = news.Title,
                 Description = news.Description,
                Id = news.Id // Ensure Id is mapped for Edit/Delete actions
            }).ToList();

            return View(viewModelList);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Add()
        {
            var model = new SeriesCreateViewModel();
            return View();
        }
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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

        public async Task<IActionResult> Info(string seriesName)
        {
            var seriesmodel = await _seriesService.Get(x=>x.Title == seriesName);
            var books = _seriesService.GetAll()
             .Where(b => b.Id == seriesmodel.Id)
             .SelectMany(b => b.Books)
             .ToList();
            List<BookSeriesViewModel> list = new List<BookSeriesViewModel>();
            foreach(var b in books)
            {
                var author = _bookService.GetAll()
             .Where(bo => bo.Id == b.Id)
             .Include(bt => bt.Author)
             .Select(a => a.Author.Name)
             .FirstOrDefault();
                var bookvm = new BookSeriesViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = author,
                    CoverImageURL = b.CoverImageURL,
                };
                list.Add(bookvm);
            }
            
            var book = new SeriesAdminViewModel()
            {
                Title=seriesmodel.Title,
                Description=seriesmodel.Description,
                Books=list,
            };
            return View(book);
        }
    }
}
