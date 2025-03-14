using System.Data.Entity;
using System.Net;
using System.Reflection;
using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.BookViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NuGet.Packaging.Signing;
using NuGet.Versioning;


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
            private readonly ILanguageService _languageService;
            private readonly IPublishingHouseService _pubHouseService;
            private readonly IBookPublishingHouseService _bookPublishingHouse;

            public BookController(IBookService bookService, IBookPublishingHouseService bookPublishingHouse,IAuthorService authorService,IGenreService genreService,ISeriesService seriesService,ITagService tagService, IBookTagService bookTagService,ILanguageService languageService, IPublishingHouseService pubHouseService)
            {
                _bookService = bookService;
                _authorService = authorService;
                _genreService = genreService;
                _seriesService = seriesService;
                _tagService = tagService;
                _bookTagService = bookTagService;
                _languageService = languageService;
                _pubHouseService = pubHouseService;
                _bookPublishingHouse = bookPublishingHouse;
            }

        public async Task<IActionResult> Index(BookFilterViewModel? filter)
        {
            var books = _bookService.GetAll();
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

            var list = _bookService.GetAll();
            var testList = list.AsQueryable().Include(b => b.Genre).Include(b => b.Author).ToList();
            var model = new BookFilterViewModel
            {
                BookId = filter.BookId,
                GenreId = filter.GenreId,
                AuthorId = filter.AuthorId,
                PageMaxCount = filter.PageMaxCount,
                PageMinCount = filter.PageMinCount,
                Genres = new SelectList(_genreService.GetAll(), "Id", "Name"),
                Authors = new SelectList(_authorService.GetAll(), "Id", "Name"),
                Books = query.Include(b => b.Genre).Include(b => b.Author).ToList()

            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
                var model = new BookEditViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    AuthorId = book.AuthorId,
                    SeriesId = book.SeriesId,
                    BookPages = book.BookPages,
                    Chapters = book.Chapters,
                    GenreId = book.GenreId,
                    CoverImageURL = book.CoverImageURL,
                    
                };
                return View(model);
            }
        [Authorize(Roles = "Admin")]

        [HttpPost]
            public async Task<IActionResult> Edit(BookEditViewModel model)
            {
            var book = new Book
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                AuthorId = model.AuthorId,
                SeriesId = model.SeriesId,
                BookPages = model.BookPages,
                Chapters = model.Chapters,
                GenreId = model.GenreId,
                CoverImageURL = model.CoverImageURL
            };
                    await _bookService.Update(book);
                    return RedirectToAction("Index");

            }
        [Authorize(Roles = "Admin")]

        [HttpPost]
            public async Task<IActionResult> Delete(int id)
            {
                await _bookService.Delete(id);
                return RedirectToAction("Index");
            }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> AssignTags(int BookId)
        {
            var book = await _bookService.GetById(BookId);
            if (book == null)
            {
                return NotFound(); // Ensure the book exists
            }

            // Get assigned tags safely (list of Tag objects)
            var selectedTags = _bookService.GetAll()
                .Where(b => b.Id == BookId)
                .SelectMany(b => b.BookTags.Select(bt => bt.Tag))
                .ToList();

            // Get all tags safely (list of Tag objects)
            var allTags = _tagService.GetAll().ToList();

            // Get available tags (tags that are NOT assigned to the book)
            var availableTags = allTags.Except(selectedTags).ToList();

            var model = new AssignTagsToBookViewModel
            {
                BookId = BookId,
                SelectedTags = selectedTags,  // List<Tag>
                AvailableTags = availableTags // List<Tag>
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> AssignLangandPH(int id)
        {
            var languages = _languageService.GetAll();
            var publishinghouses = _pubHouseService.GetAll();
            ViewBag.Languages = new SelectList(languages, "Id", "Name");
            ViewBag.PublishingHouses = new SelectList(publishinghouses, "Id", "Name");
            var model = new BookLanguagePHVM
            {
                BookId = id,
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> AssignLangandPH(BookLanguagePHVM? model)
        {
            var book = new BookPublishingHouse()
            {
                BookId = model.BookId,
                LanguageId = model.LanguageId,
                PublishingHouseId = model.PublishingHouseId,
                PublishingDate = model.PublishingDate,
            };
            await _bookPublishingHouse.Add(book);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]

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
            var language = _languageService.GetAll()
                .Where(b => b.BookPublishingHouses.Select(bt=>bt.BookId==bookId).FirstOrDefault())
                .SelectMany(b => b.BookPublishingHouses.Select(bp => bp.Language.Name))
                .FirstOrDefault();
            var publishinghouse = _pubHouseService.GetAll()
                .Where(b => b.bookPublishingHouses.Select(c=>c.BookId==bookId).FirstOrDefault())
                .SelectMany(b => b.bookPublishingHouses.Select(bp => bp.PublishingHouse.Name))
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
                LanguageName=language,
                SelectedTags = tags,
                PublishingHouseName= publishinghouse
            };
            return View(book);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveTag(int bookId, int tagId)
        {
            var booktagmodel = _bookTagService.GetAll();
            
            foreach(var tag in booktagmodel)
            {
                if (tag.TagId == tagId && tag.BookId == bookId)
                {
                    await _bookTagService.DeleteBookTag(tag.BookId,tag.TagId);
                }
            }
       
            return View("AssignTags",bookId);

        }

    }
}
