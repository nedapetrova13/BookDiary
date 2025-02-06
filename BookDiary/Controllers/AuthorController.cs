using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.AuthorViewModels;
using BookDiary.Models.ViewModels.AuthorViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

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
            var list =  _authorService.GetAll();
            return View(list);
        }
        public IActionResult Add()
        {
            var model = new AuthorCreateViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AuthorCreateViewModel acvm)
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
            return RedirectToAction("Index");
        }
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
        [HttpPost]
        public async Task<IActionResult> Edit(Author au)
        {
            if (ModelState.IsValid)
            {
                await _authorService.Update(au);
                return RedirectToAction("Index");
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
