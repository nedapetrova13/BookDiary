using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.CurrentReadViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class CurrentReadController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly ICurrentReadService _currentReadService;
        private readonly IBookService _bookService;

        public CurrentReadController(UserManager<User> userManager, IUserService userService, ICurrentReadService currentReadService, IBookService bookService)
        {
            _userManager = userManager;
            _userService = userService;
            _currentReadService = currentReadService;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var crs = await _currentReadService.Find(x => x.UserId == currentUser.Id);
            List<CurrentReadIndexViewModel> currentReads = new List<CurrentReadIndexViewModel>();
            foreach ( var item in crs)
            {
                Book book = await _bookService.GetById(item.BookId);
                var currentread = new CurrentReadIndexViewModel
                {
                    Id = item.Id,
                    Userid = item.UserId,
                    BookId = item.BookId,
                    BookName = book.Title,
                    CoverImageURL = book.CoverImageURL,
                    Pages = item.CurrentPage,
                    TotalPages=book.BookPages
                };
                currentReads.Add(currentread);
            }
            return View(currentReads);
        }

        [HttpPost]
        public async Task<IActionResult> SetCurrentRead(int bookid)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentreads = await _currentReadService.Get(x => x.UserId == currentUser.Id && x.BookId == bookid);
            if (currentreads == null)
            {
                CurrentRead cr = new CurrentRead
                {
                    BookId = bookid,
                    UserId = currentUser.Id,
                    CurrentPage = 0
                };
                await _currentReadService.Add(cr);
            }
            else
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCurrentRead(int bookId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

  
            await _currentReadService.DeleteCurrentRead(bookId, currentUser.Id);
           return RedirectToAction("Index");
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(int bookid, int pages)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            CurrentRead currentread = await  _currentReadService.Get(x => x.BookId == bookid && x.UserId == currentUser.Id);
            currentread.CurrentPage = pages;
            await _currentReadService.Update(currentread);
            return RedirectToAction("Index");

        }
    }
}
