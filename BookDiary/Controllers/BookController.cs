using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookDiary.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;
        private readonly ISeriesService _seriesService;

        public BookController(IBookService bookService,IAuthorService authorService,IGenreService genreService,ISeriesService seriesService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _genreService = genreService;
            _seriesService = seriesService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _bookService.GetAllBooks();
            return View(list);
        }
        public async Task<IActionResult> Add()
        {
            var authors = await _authorService.GetAllAuthors();
            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            var genres = await _genreService.GetAllGenres();
            ViewBag.Genres = new SelectList(genres, "Id", "Name");
            var series = await _seriesService.GetAllSeries();
            ViewBag.Series = new SelectList(series, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Book book)
        {
            await _bookService.Add(book);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {

            var book = await _bookService.GetById(id);
            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookService.Update(book);
                return RedirectToAction("Index");
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
