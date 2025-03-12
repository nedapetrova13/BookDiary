using System.Data.Entity;
using System.Net;
using System.Reflection;
using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.BookViewModels;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NuGet.Packaging.Signing;


namespace BookDiary.Controllers
{
    public class BookController : Controller
    {
            private readonly IBookService _bookService;
            private readonly IAuthorService _authorService;
            private readonly IGenreService _genreService;
            private readonly ISeriesService _seriesService;
            private readonly ITagService _tagService; 
            private readonly IBookTagService _bookTagService;

            public BookController(IBookService bookService,IAuthorService authorService,IGenreService genreService,ISeriesService seriesService,ITagService tagService, IBookTagService bookTagService)
            {
                _bookService = bookService;
                _authorService = authorService;
                _genreService = genreService;
                _seriesService = seriesService;
                _tagService = tagService;
                _bookTagService = bookTagService;
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
                    BookId=filter.BookId,
                    GenreId = filter.GenreId,
                    AuthorId = filter.AuthorId,
                    PageMaxCount = filter.PageMaxCount,
                    PageMinCount = filter.PageMinCount,
                    Genres = new SelectList( _genreService.GetAll(), "Id", "Name"),
                    Authors = new SelectList( _authorService.GetAll(), "Id", "Name"),
                    Books = query.Include(b => b.Genre).Include(b => b.Author).ToList()

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
                var model = new BookCreateViewModel();
                return View(model);
            }
            [HttpPost]
            public async Task<IActionResult> Add(BookCreateViewModel bookcvm)
            {
            var book = new Book
            {
                Title = bookcvm.Title,
                Description = bookcvm.Description,
                AuthorId = bookcvm.AuthorId,
                GenreId = bookcvm.GenreId,
                SeriesId = bookcvm.SeriesId,
                CoverImageURL = bookcvm.CoverImageURL,
                BookFormat = bookcvm.BookFormat,
                BookPages = bookcvm.BookPages,
                Chapters = bookcvm.Chapters
            };
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
        [HttpGet]
        public IActionResult AssignTags(int BookId)
        {
            var tags = _tagService.GetAll();
            var model = new AssignTagsToBookViewModel
            {
               BookId= BookId,
               TagList = tags.ToList(),
            };

            return View("AssignTags", model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignTags(AssignTagsToBookViewModel model)
        {
            foreach (var tag in model.SelectedTagIds)
            {
                var booktag = new BookTag()
                {
                    TagId = tag,
                    BookId = model.BookId
                };

                await _bookTagService.Add(booktag);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Info(int bookId)
        {
            var bookcvm = await _bookService.GetById(bookId);
            var tags = _bookService.GetAll()
             .Where(b => b.Id == bookId)
             .SelectMany(b => b.BookTags.Select(bt => bt.Tag.Name))
             .ToList();
            var author = _bookService.GetAll()
             .Where(b => b.Id == bookId)
             .Include(bt => bt.Author)
             .Select(a => a.Author.Name)
             .FirstOrDefault();
            var genre = _bookService.GetAll()
           .Where(b => b.Id == bookId)
           .Include(bt => bt.Genre)
           .Select(a => a.Genre.Name)
           .FirstOrDefault();
            var series = _bookService.GetAll()
           .Where(b => b.Id == bookId)
           .Include(bt => bt.Series)
           .Select(a => a.Series.Title)
           .FirstOrDefault();
            var format = _bookService.GetAll()
             .Where(b => b.Id == bookId)
             .Select(b => b.BookFormat.ToString()) // Convert the enum to a string
             .FirstOrDefault();
            var seriesview = _seriesService.Get(x => x.Title == series);
            
            var book = new BookAdminViewModel()
            {
                Id = bookcvm.Id,
                Title = bookcvm.Title,
                Description = bookcvm.Description,
                AuthorName = author,
                GenreName = genre,
                SeriesName = series,
                SeriesId = seriesview.Id,
                CoverImageURL = bookcvm.CoverImageURL,
                BookFormat = format,
                BookPages = bookcvm.BookPages,
                Chapters = bookcvm.Chapters,
                SelectedTags = tags
            };
            return View(book);
        }
    }
}
