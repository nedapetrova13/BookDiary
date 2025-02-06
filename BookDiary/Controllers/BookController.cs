using System.Data.Entity;
using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.BookViewModels;
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
            private readonly ITagService _tagService; 

            public BookController(IBookService bookService,IAuthorService authorService,IGenreService genreService,ISeriesService seriesService,ITagService tagService)
            {
                _bookService = bookService;
                _authorService = authorService;
                _genreService = genreService;
                _seriesService = seriesService;
                _tagService = tagService; ;
            }

            public async  Task<IActionResult> Index(BookFilterViewModel? filter)
            {
                var books =  _bookService.GetAll();
                var query = books.AsQueryable();
                if ((filter.GenreId != null))
                {
                    query = query.Where(g => g.GenreId == filter.GenreId);
                }
                if ((filter.AuthorId != null))
                {
                    query = query.Where(b => b.AuthorId == filter.AuthorId);
                }
                if ((filter.PageMinCount != null) && (filter.PageMinCount != 0))
                {
                    query = query.Where(b => b.BookPages >= filter.PageMinCount);
                }
                if ((filter.PageMaxCount != null) && (filter.PageMaxCount != 0))
                {
                    query = query.Where(b => b.BookPages <= filter.PageMaxCount);
                }

                var list =  _bookService.GetAll();
                var testList = list.AsQueryable().Include(b => b.Genre).Include(b => b.Author).ToList();
                var model = new BookFilterViewModel
                {
                    GenreId = filter.GenreId,
                    AuthorId = filter.AuthorId,
                    PageMaxCount = filter.PageMaxCount,
                    PageMinCount = filter.PageMinCount,
                    Genres = new SelectList( _genreService.GetAll(), "Id", "Name"),
                    Authors = new SelectList( _authorService.GetAll(), "Id", "Name"),
                    Books = query.Include(b => b.Genre).Include(b => b.Author).ToList()
                    //Books = testList
                };
                return View(model);
            }
            public async Task<IActionResult> Add()
            {
                var authors = _authorService.GetAll();
                var genres =  _genreService.GetAll();
                var series =  _seriesService.GetAll();
                ViewBag.Genres = new SelectList(genres, "Id", "Name");
                ViewBag.Authors = new SelectList(authors, "Id", "Name");
                ViewBag.Series = new SelectList(series, "Id", "Title");
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
                var authors = _authorService.GetAll();
                var genres =  _genreService.GetAll();
                var series =  _seriesService.GetAll();
                var tags =  _tagService.GetAll();
                ViewBag.Genres = new SelectList(genres, "Id", "Name");
                ViewBag.Authors = new SelectList(authors, "Id", "Name");
                ViewBag.Series = new SelectList(series, "Id", "Title");
                ViewBag.Tags = new SelectList(tags, "Id", "Name");
                var book = await _bookService.GetById(id);
                return View(book);
            }
            [HttpPost]
            public async Task<IActionResult> Edit(Book book)
            {
            
                    await _bookService.Update(book);
                    return RedirectToAction("Index");

            }    
            [HttpPost]
            public async Task<IActionResult> Delete(int id)
            {
                await _bookService.Delete(id);
                return RedirectToAction("Index");
            }
        }
    }
