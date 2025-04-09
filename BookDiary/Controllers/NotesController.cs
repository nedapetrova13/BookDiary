using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.BookViewModels;
using BookDiary.Models.ViewModels.CommentViewModels;
using BookDiary.Models.ViewModels.NotesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace BookDiary.Controllers
{
    public class NotesController : Controller
    {
        private readonly INotesService _notesService;
        private readonly IBookService _bookService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public NotesController(INotesService notesService, IBookService bookService, UserManager<User> userManager, IUserService userService)
        {
            _notesService = notesService;
            _bookService = bookService;
            _userManager = userManager;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Add(int bookid)
        {
            var note = new CreateNoteViewModel();
            note.BookId=bookid;
            return View(note);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateNoteViewModel note)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if ( note.BookId == 0 || note.BookChapter <= 0 || note.NoteContent == null || note.Title == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(note);
            }
            else
            {
                Notes notes = new Notes
                {
                    UserId = currentUser.Id,
                    BookId = note.BookId,
                    BookChapter = note.BookChapter,
                    NoteContent = note.NoteContent,
                    Title = note.Title,
                };
                await _notesService.Add(notes);
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

                List<Book> books = await _bookService.Find(x => x.Notes.Any(x => x.UserId == currentUser.Id));
            List<IndexBookNotesViewModel> mybooknotes=new List<IndexBookNotesViewModel>();  
            foreach (Book book in books)
            {
                var b = new IndexBookNotesViewModel
                {
                    BookId = book.Id,
                    UserId = currentUser.Id,
                    BookCover = book.CoverImageURL

                };
                mybooknotes.Add(b);
            }
            return View(mybooknotes);
        }
        public async Task<IActionResult> BookNotes(int bookid)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            List<Notes> notes = await _notesService.Find(x => x.UserId==currentUser.Id && x.BookId==bookid);
            List<NotesInfoViewModel> info = new List<NotesInfoViewModel>(); 
            foreach (Notes note in notes)
            {
                var n = new NotesInfoViewModel
                {
                    Id = note.Id,
                    NoteContent = note.NoteContent,
                    Title = note.Title,
                    BookChapter = note.BookChapter,
                    BookId = note.BookId,   
                };
                info.Add(n);
            }
            return View(info);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var note = await _notesService.GetById(id);
            var model = new CreateNoteViewModel
            {
                Id = note.Id,
                Title = note.Title,
                BookId=note.BookId,
                BookChapter=note.BookChapter,
                NoteContent=note.NoteContent,
                UserId=currentUser.Id

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateNoteViewModel model)
        {
            if (model.UserId == null || model.BookId == 0 || model.BookChapter <= 0 || model.NoteContent == null || model.Title == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(model);
            }
            else
            {
                var note = new Notes
                {
                    Id = model.Id,
                    Title = model.Title,
                    BookId = model.BookId,
                    BookChapter = model.BookChapter,
                    NoteContent = model.NoteContent,
                    UserId = model.UserId
                };
                await _notesService.Update(note);
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _notesService.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
