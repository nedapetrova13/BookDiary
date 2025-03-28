using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.AuthorViewModels;
using BookDiary.Models.ViewModels.AuthorViewModels;
using BookDiary.Models.ViewModels.BookViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
using BookDiary.Models.ViewModels.SeriesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

namespace BookDiary.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            var auList = _authorService.GetAll();

            var viewModelList = auList.Select(au => new AuthorCreateViewModel
            {
                Name = au.Name,
                BirthDate = au.BirthDate,
                Bio = au.Bio,
                Email = au.Email,
                Gender = au.Gender,
                ProfilePictureURL = au.ProfilePictureURL,
                WebSiteLink = au.WebSiteLink,
                Id = au.Id 
            }).ToList();

            return View(viewModelList);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Add()
        {
            var model = new AuthorCreateViewModel();
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Add(AuthorCreateViewModel acvm)
        {
            if (acvm.Name == null || acvm.BirthDate > DateTime.UtcNow || acvm.Bio == null || acvm.Email == null || acvm.ProfilePictureURL == null || acvm.Gender == null || acvm.WebSiteLink == null)
            {
                TempData["error"] = "Невалидни данни";

                return View(acvm);
            }
            else
            {
                bool isExists = _authorService.GetAll().Where(x=>x.Name == acvm.Name).Any();
                if (!isExists)
                {
                    var author = new Author
                    {
                        Name = acvm.Name,
                        BirthDate = acvm.BirthDate,
                        Bio = acvm.Bio,
                        Email = acvm.Email,
                        ProfilePictureURL = acvm.ProfilePictureURL,
                        Gender = acvm.Gender,
                        WebSiteLink = acvm.WebSiteLink
                    };
                    await _authorService.Add(author);
                }
                else
                {
                    TempData["error"] = "Вече съществува автор с такова име";
                    return View(acvm);
                }

                
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id)
        {

            var au = await _authorService.GetById(id);
            var model = new AuthorEditViewModel
            {
                Name = au.Name,
                BirthDate = au.BirthDate,
                Bio = au.Bio,
                Email = au.Email,
                ProfilePictureURL = au.ProfilePictureURL,
                Gender = au.Gender,
                WebSiteLink = au.WebSiteLink
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Edit(AuthorEditViewModel aevm)
        {
            if (aevm.Name == null || aevm.BirthDate > DateTime.UtcNow || aevm.Bio == null || aevm.Email == null || aevm.ProfilePictureURL == null || aevm.Gender == null || aevm.WebSiteLink == null)
            {
                TempData["error"] = "Невалидни данни";

                return View(aevm);
            }
            else
            {
                bool isExists = _authorService.GetAll().Where(x => x.Name == aevm.Name).Any();
                if (!isExists)
                {
                    var author = new Author
                    {
                        Id = aevm.Id,
                        Name = aevm.Name,
                        BirthDate = aevm.BirthDate,
                        Bio = aevm.Bio,
                        Email = aevm.Email,
                        ProfilePictureURL = aevm.ProfilePictureURL,
                        Gender = aevm.Gender,
                        WebSiteLink = aevm.WebSiteLink
                    };
                    await _authorService.Update(author);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Вече съществува автор с такова име";

                    return View(aevm);
                }
            }
               

        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.Delete(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Info(string authorName)
        {
            var authormodel = await _authorService.Get(x => x.Name == authorName);
            var books = _authorService.GetAll()
             .Where(b => b.Id == authormodel.Id)
             .SelectMany(b => b.Books)
             .ToList();
            List<BookSeriesViewModel> list = new List<BookSeriesViewModel>();
            foreach (var b in books)
            {
              
                var bookvm = new BookSeriesViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = authormodel.Name,
                    CoverImageURL = b.CoverImageURL,
                };
                list.Add(bookvm);
            }

            var au = new AuthorAdminViewModel()
            {
                Id=authormodel.Id,
                Name=authormodel.Name,
                BirthDate=authormodel.BirthDate,
                Bio=authormodel.Bio,
                Email=authormodel.Email,
                ProfilePictureURL=authormodel.ProfilePictureURL,
                WebSiteLink=authormodel.WebSiteLink,
                Books = list,
            };
            return View(au);
        }
    }
}
