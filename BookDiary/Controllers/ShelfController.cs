using System;
using System.Data.Entity;
using System.Net;
using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.BookViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
using BookDiary.Models.ViewModels.SeriesViewModels;
using BookDiary.Models.ViewModels.ShelfViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace BookDiary.Controllers
{
    public class ShelfController : Controller
    {
        private readonly IShelfService _shelfService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IBookService _bookService;
        private readonly IShelfBookService _shelfBookService;
        private readonly ICurrentReadService _currentReadService;


       public ShelfController(IShelfService shelfService,IShelfBookService shelfBookService, IUserService userService,UserManager<User> userManager,IBookService bookService,ICurrentReadService currentReadService)
        {
           _shelfService = shelfService;
            _userService = userService;
            _userManager = userManager;
            _bookService = bookService;
            _shelfBookService = shelfBookService;
            _currentReadService = currentReadService;
        }

        public async Task<IActionResult> Index(string userid)
        {
            var shelfList = _shelfService.GetAll();
            var currentUser = await _userManager.GetUserAsync(User);
               
                    var viewModelList = shelfList.Where(x=>x.UserId==currentUser.Id).Select(shelf => new ShelfIndexViewModel
                    {
                        Name = shelf.Name,
                        Id = shelf.Id,
                        UserId = shelf.UserId,

                    }).ToList();
                
            
            return View(viewModelList);
        }
        public IActionResult Add(string userid)
        {
            var model = new ShelfCreateViewModel();
        
            return View(model);
        }
        [HttpPost]
        public  async Task<IActionResult> Add(ShelfCreateViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (model.Name == null || model.Description == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(model);
            }
            else
            {
                bool isExists = _shelfService.GetAll().Where(x=>x.Name == model.Name && x.UserId==currentUser.Id).Any();
                if(!isExists)
                {
                    var shelf = new Shelf
                    {
                        Name = model.Name,
                        Id = model.Id,
                        UserId = currentUser.Id,
                        Description = model.Description,
                    };
                    await _shelfService.Add(shelf);
                    TempData["success"] = "Успешно добавен шкаф!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Вече имате шкаф с такова име";
                    return View(model);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkRead(int bookid)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var book = await _bookService.GetById(bookid);
            var isshelf =  _shelfService.GetAll().Any(x=>x.Name=="Прочетени книги" && x.UserId==currentUser.Id);
            if (!isshelf)
            {
                var shelf = new Shelf
                {
                    Name = "Прочетени книги",
                    UserId = currentUser.Id,
                    Description = "Шкаф с всички прочетени книги",
                };
                await _shelfService.Add(shelf);
                var bs = new ShelfBook
                {
                    BookId = bookid,
                    ShelfId = shelf.Id
                };
                await _shelfBookService.Add(bs);
            }
            else
            {
                Shelf sh = await _shelfService.Get(x => x.Name == "Прочетени книги" && x.UserId == currentUser.Id);
                var books = await _shelfBookService.Get(x=>x.BookId == bookid && x.ShelfId==sh.Id);                          

                
                if (books==null)
                {
                    var bs = new ShelfBook
                    {
                        BookId = bookid,
                        ShelfId = sh.Id
                    };
                    await _shelfBookService.Add(bs);
                }
                else
                {
                    TempData["error"] = "книгата вече е прочетена";
                }
            }
            var currentRead = await _currentReadService.Get(x => x.UserId == currentUser.Id && x.BookId == bookid);
            if (currentRead != null)
            {
                await _currentReadService.DeleteCurrentRead(bookid, currentUser.Id);
            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            List<ShelfBook> sb = await _shelfBookService.Find(x=>x.ShelfId==id);
            foreach (var item in sb)
            {
                await _shelfBookService.DeleteShelfBook(item.BookId, item.ShelfId);
            }
            await _shelfService.Delete(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Info(int shelfId)
        {
            var shelf = await _shelfService.GetById(shelfId);
            var books = _shelfService.GetAll()
             .Where(b => b.Id == shelfId)
             .SelectMany(b => b.ShelfBooks.Select(x=>x.Book))
             .ToList();
            List<BookSeriesViewModel> list = new List<BookSeriesViewModel>();
            foreach (var b in books)
            {
               
                var bookvm = new BookSeriesViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    CoverImageURL = b.CoverImageURL,
                };
                list.Add(bookvm);
            }

            var shelfmodel = new ShelfInfoViewModel()
            {
                Id = shelfId,
                Name = shelf.Name,
                Description = shelf.Description,
                Books = list,
            };
            return View(shelfmodel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Shelf shelf = await _shelfService.GetById(id);
            var model = new ShelfCreateViewModel
            {
                Name = shelf.Name,
                Description = shelf.Description,
                UserId= currentUser.Id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ShelfCreateViewModel scvm)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (scvm.Name == null || scvm.Description == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(scvm);
            }
            else
            {
                bool isExists = _shelfService.GetAll().Where(x => x.Name == scvm.Name && x.UserId == currentUser.Id && x.Description==scvm.Description).Any();
                if (!isExists)
                {
                    var model = new Shelf
                    {
                        Id = scvm.Id,
                        Name = scvm.Name,
                        Description = scvm.Description,
                        UserId = currentUser.Id
                    };
                    await _shelfService.Update(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Вече имате шкаф с такова име";
                    return View(scvm);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteShelfBook(int bookId, int shelfid)
        {
            await _shelfBookService.DeleteShelfBook(bookId, shelfid);
            return RedirectToAction("Info", new { shelfId = shelfid });
        }
        public async Task<IActionResult> Details(int id, int? selectedBookId)
        {
           var shelf = _shelfService.GetAll().Where(x=>x.Id == id).FirstOrDefault();
            var book = _bookService.GetAll().Where(x=>x.Id==selectedBookId).FirstOrDefault();
            var books = _shelfService.GetAll()
            .Where(b => b.Id == id)
            .SelectMany(b => b.ShelfBooks.Select(x => x.Book))
            .ToList();

            if (shelf == null)
            {
                return NotFound();
            }
            List<BookSeriesViewModel> list = new List<BookSeriesViewModel>();
            foreach(var b in books)
            { 
                
                var bvm = new BookSeriesViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    CoverImageURL = b.CoverImageURL,
                };
                list.Add(bvm);
            }
            

            var viewModel = new ShelfInfoViewModel
            {
                Id = shelf.Id,
                Name = shelf.Name,
                Description = shelf.Description,
                Books = list,
                SelectedBook = book 
            };

            return View(viewModel);
        }
    }
}
