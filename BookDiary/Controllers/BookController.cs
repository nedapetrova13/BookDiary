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
            private readonly IShelfService _shelfService;
            private readonly IShelfBookService _shelfBookService;
            private readonly ICommentService _commentService;
            private readonly UserManager<User> _userManager;
            private readonly IUserService _userService;
            private readonly ICurrentReadService _currentReadService;

        public BookController(IBookService bookService,IUserService userService,ICurrentReadService currentReadService, UserManager<User> userManager, ICommentService commentService, IShelfBookService shelfBookService, IShelfService shelfService,IAuthorService authorService,IGenreService genreService,ISeriesService seriesService,ITagService tagService, IBookTagService bookTagService,ILanguageService languageService, IPublishingHouseService pubHouseService)
        {
                _bookService = bookService;
                _authorService = authorService;
                _genreService = genreService;
                _seriesService = seriesService;
                _tagService = tagService;
                _bookTagService = bookTagService;
                _languageService = languageService;
                _pubHouseService = pubHouseService;
                _shelfService = shelfService;
                _shelfBookService = shelfBookService;
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
                             .Include(x => x.BookTags) 
                             .Where(x => x.BookTags.Any(b => b.TagId == filter.TagId));
            }
            if (filter.PublishingHouseId != null)
            {
                query = query.Where(g => g.PublishingHouseId == filter.PublishingHouseId);

            }
            if (filter.LanguageId != null)
            {
                query = query.Where(g => g.LanguageId == filter.LanguageId);

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
            if(bookcvm.Title==null || bookcvm.Description==null || bookcvm.AuthorId==0 || bookcvm.GenreId==0 || bookcvm.SeriesId==0 || bookcvm.CoverImageURL==null || bookcvm.BookPages<=0 || bookcvm.Chapters<=0 || bookcvm.LanguageId==0 || bookcvm.PublishingHouseId == 0)
            {
                TempData["error"] = "Невалидни данни";

                return View(bookcvm);
            }
            else
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
                    Chapters = bookcvm.Chapters,
                    LanguageId = bookcvm.LanguageId,
                    PublishingHouseId = bookcvm.PublishingHouseId

                };

                await _bookService.Add(book);

                return RedirectToAction("Index");

            }
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id)
        {
                var authors = _authorService.GetAll();
                var genres =  _genreService.GetAll();
                var series =  _seriesService.GetAll();
                var tags =  _tagService.GetAll();
                var languages = _languageService.GetAll();
                var publishinghouses = _pubHouseService.GetAll();
                ViewBag.Genres = new SelectList(genres, "Id", "Name");
                ViewBag.Authors = new SelectList(authors, "Id", "Name");
                ViewBag.Series = new SelectList(series, "Id", "Title");
                ViewBag.Tags = new SelectList(tags, "Id", "Name");
                ViewBag.Languages = new SelectList(languages, "Id", "Name");
                ViewBag.PublishingHouses = new SelectList(publishinghouses, "Id", "Name");
                
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
                    LanguageId = book.LanguageId,
                    PublishingHouseId= book.PublishingHouseId,
                };
                return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Edit(BookEditViewModel model)
        {
            if (model.Title == null || model.Description == null || model.AuthorId == 0 || model.GenreId == 0 || model.SeriesId == 0 || model.CoverImageURL == null || model.BookPages <= 0 || model.Chapters <= 0 || model.LanguageId == 0 || model.PublishingHouseId == 0)
            {
                TempData["error"] = "Невалидни данни";

                return View(model);
            }
            else
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
                    CoverImageURL = model.CoverImageURL,
                    LanguageId = model.LanguageId,
                    PublishingHouseId = model.PublishingHouseId
                };

                await _bookService.Update(book);
                return RedirectToAction("Index");

            }
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

            
            var allTags = _tagService.GetAll().ToList();

           
            var availableTags = allTags.Except(selectedTags).ToList();

            var model = new AssignTagsToBookViewModel
            {
                BookId = BookId,
                SelectedTags = selectedTags,  
                AvailableTags = availableTags 
               
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
            return RedirectToAction("Info", new { bookId = model.BookId });
        }



        public async Task<IActionResult> Info(int bookId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var shelves1 = _shelfService.GetAll().Where(x => !x.ShelfBooks.Any(b => b.BookId == bookId && x.UserId==currentUser.Id)).ToList();
            var shelves = await _shelfService.Find(x => x.UserId == currentUser.Id && !x.ShelfBooks.Any(b => b.BookId == bookId) && x.Name!="Прочетени книги");
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
           
           
            var language = _bookService.GetAll()
                .Where(b => b.Id == bookId)
               .Include(bt => bt.Language)
               .Select(a => a.Language.Name)
               .FirstOrDefault();


            var publishinghouse = _bookService.GetAll()
                 .Where(b => b.Id == bookId)
                 .Include(bt => bt.PublishingHouse)
                 .Select(a => a.PublishingHouse.Name)
                 .FirstOrDefault();



            var comments = await _commentService.Find(x => x.BookId==bookId);

          
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
            Shelf sh = await _shelfService.Get(x => x.Name == "Прочетени книги" && x.UserId == currentUser.Id);
            if (sh != null)
            {
                var books = await _shelfBookService.Get(x => x.BookId == bookcvm.Id && x.ShelfId == sh.Id);
                if (books == null)
                {
                    book.IsRead = false;
                }
                else
                {
                    book.IsRead = true;
                }
            }
            else
            {
                book.IsRead = false;
            }
            
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
            var book = await _bookService.GetById(bookid);
            var shelf = await _shelfService.GetById(shelfId);
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

            var existingComment = await _commentService.Find(x => x.BookId == comment.BookId && x.UserId == currentUser.Id);
            if (existingComment.Any())
            {
                TempData["error"] = "Вече сте коментирали тази книга!";
                return RedirectToAction("Info", new { bookId = comment.BookId });
            }
            if (comment.BookId == 0 || comment.Content == null || comment.Rating == 0)
            {
                TempData["error"] = "Невалидни данни";
                return RedirectToAction("Info", new { bookId = comment.BookId });
            }
            else
            {
                Comment com = new Comment
                {
                    UserId = currentUser.Id,
                    Content = comment.Content,
                    Rating = comment.Rating,
                    BookId = comment.BookId,
                };
                await _commentService.Add(com);

                return RedirectToAction("Info", new { bookId = comment.BookId });
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int bookId, int commentid)
        {
            Comment comment = _commentService.GetAll().Where(x=>x.Id==commentid).FirstOrDefault();
            await _commentService.DeleteComment(bookId,comment.UserId);
            return RedirectToAction("Info", new { BookId = bookId });
        }
        
    }
}
