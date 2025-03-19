﻿using System.Data.Entity;
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

       public ShelfController(IShelfService shelfService,IShelfBookService shelfBookService, IUserService userService,UserManager<User> userManager,IBookService bookService)
        {
           _shelfService = shelfService;
            _userService = userService;
            _userManager = userManager;
            _bookService = bookService;
            _shelfBookService = shelfBookService;
        }

        public async Task<IActionResult> Index(string userid)
        {
            //var user = _userService.GetByIdUser(userid);
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

            var shelf = new Shelf
            {
                Name = model.Name,
                Id = model.Id,
                UserId = currentUser.Id,
                Description = model.Description,
            };
            await _shelfService.Add(shelf);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
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

            var model = new Shelf
            {
                Id = scvm.Id,
                Name = scvm.Name,
                Description = scvm.Description,
                UserId=currentUser.Id
            };
            await _shelfService.Update(model);
            return RedirectToAction("Index");

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
