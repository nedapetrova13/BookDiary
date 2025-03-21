using System.Data.Entity;
using System.Net;
using System.Reflection;
using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.BookViewModels;
using BookDiary.Models.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
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
            private readonly IShelfService _shelfService;
            private readonly IShelfBookService _shelfBookService;
            private readonly ICommentBookService _commentBookService;
            private readonly ICommentService _commentService;
            private readonly UserManager<User> _userManager;
            private readonly IUserService _userService;
            private readonly ICurrentReadService _currentReadService;



        public BookController(IBookService bookService,IUserService userService,ICurrentReadService currentReadService, UserManager<User> userManager, ICommentService commentService,ICommentBookService commentBookService, IShelfBookService shelfBookService, IShelfService shelfService, IBookPublishingHouseService bookPublishingHouse,IAuthorService authorService,IGenreService genreService,ISeriesService seriesService,ITagService tagService, IBookTagService bookTagService,ILanguageService languageService, IPublishingHouseService pubHouseService)
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
                _shelfService = shelfService;
                _shelfBookService = shelfBookService;
                _commentBookService = commentBookService;
                _commentService = commentService;
                _userManager = userManager;
                _userService = userService;
                _currentReadService = currentReadService;
            
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
            if ((filter.TagId != null))
            {
                query = query.AsQueryable()
                             .Include(x => x.BookTags) // Load BookTags properly
                             .Where(x => x.BookTags.Any(b => b.TagId == filter.TagId));
            }
            if (filter.PublishingHouseId != null)
            {
                query = query.AsQueryable().Include(x => x.BookPublishingHouse).Where(b => b.BookPublishingHouse.Any(b=>b.PublishingHouseId == filter.PublishingHouseId));

            }
            if (filter.LanguageId != null)
            {
                query = query.AsQueryable().Include(x => x.BookPublishingHouse).Where(b => b.BookPublishingHouse.Any(b => b.LanguageId == filter.LanguageId));

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
                Languages = new SelectList(_languageService.GetAll(),"Id","Name"),
                PublishingHouses = new SelectList(_pubHouseService.GetAll(), "Id", "Name"),
                Tags = new SelectList(_tagService.GetAll(), "Id", "Name"),
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
                var languages = _languageService.GetAll();
                var ph = _pubHouseService.GetAll();
                ViewBag.Genres = new SelectList(genres, "Id", "Name");
                ViewBag.Authors = new SelectList(authors, "Id", "Name");
                ViewBag.Series = new SelectList(series, "Id", "Title");
                ViewBag.Languages = new SelectList(languages, "Id", "Name");
                ViewBag.PublishingHouses = new SelectList(ph, "Id", "Name");
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
                BookPages = bookcvm.BookPages,
                Chapters = bookcvm.Chapters
            };

            await _bookService.Add(book);
            var bphl = new BookPublishingHouse
            {
                BookId = book.Id,
                PublishingHouseId = bookcvm.PublishingHouseId,
                LanguageId = bookcvm.LanguageId,
            };
            await _bookPublishingHouse.Add(bphl);
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
                return NotFound(); 
            }

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

        public async Task<IActionResult> Info(int bookId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var shelves = _shelfService.GetAll().Where(x => !x.ShelfBooks.Any(b => b.BookId == bookId)).ToList();  
            ViewBag.Shelves = new SelectList(shelves, "Id", "Name");
            
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
           .Select(a => a.Series)
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
                 .Where(ph => ph.bookPublishingHouses.Any(bph => bph.BookId == bookId))
                 .Select(ph => ph.Name)
                 .FirstOrDefault();
            

            var comments = await _commentService.Find(x => x.CommentBooks.Any(bt => bt.BookId == bookId));

          
            List<CommentUserViewModel> comuser = new List<CommentUserViewModel>();
            foreach ( var comment in comments)
            {
                var user = _userService.GetAll().Where(x=>x.Id==comment.UserId).Select(x=>x.Name).FirstOrDefault();

                var com = new CommentUserViewModel
                {
                    Id = comment.Id,
                    BookId = bookId,
                    Content = comment.Content,
                    Rating = comment.Rating,
                    UserName = user
                };
                comuser.Add(com);
            }
            
            var book = new BookAdminViewModel()
            {
                Id = bookcvm.Id,
                Title = bookcvm.Title,
                Description = bookcvm.Description,
                AuthorName = author,
                GenreName = genre,
                CoverImageURL = bookcvm.CoverImageURL,
                BookPages = bookcvm.BookPages,
                Chapters = bookcvm.Chapters,
                LanguageName=language,
                SelectedTags = tags,
                PublishingHouseName= publishinghouse,
                CommentUsers = comuser
            };
            if (series != null)
            {
                book.SeriesName = series.Title;
                book.SeriesId = series.Id;
            }
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveTag(int bookId, int tagId)
        {
            await _bookTagService.DeleteBookTag(bookId, tagId);
            return RedirectToAction("AssignTags", new { BookId = bookId });
        }
        [HttpPost]
        public async Task<IActionResult> AddBookToShelf(int bookid, int shelfId)
        {
            var book = _bookService.GetById(bookid);
            var shelf = _shelfService.GetById(shelfId);
            var bs = new ShelfBook
            {
                BookId = bookid,
                ShelfId = shelfId
            };
            await _shelfBookService.Add(bs);
            return RedirectToAction("Info", new { bookId = bookid });
        }


        [HttpGet]
        public async Task<IActionResult> AddComment(int BookId)
        {
            var book = await _bookService.GetById(BookId);
            if (book == null)
            {
                return NotFound();
            }
            var comment = new CommentViewModel();
            return View(comment);
        }



        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel comment)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Comment com = new Comment
            {
                UserId = currentUser.Id,
                Content=comment.Content,
                Rating = comment.Rating,
            };
            await _commentService.Add(com);
            CommentBook cb = new CommentBook
            {
                BookId = comment.BookId,
                CommentId = com.Id,
            };
            await _commentBookService.Add(cb);
            return RedirectToAction("Info", new { bookId = comment.BookId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int bookId, int commentid)
        {
            await _commentBookService.DeleteBookComment(bookId, commentid);
            return RedirectToAction("Info", new { BookId = bookId });
        }
        
    }
}
