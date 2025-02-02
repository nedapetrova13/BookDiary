﻿using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
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

        public async Task<IActionResult> Index()
        {
            var list = await _authorService.GetAllAuthors();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Author au)
        {
            await _authorService.Add(au);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {

            var au = await _authorService.GetById(id);
            return View(au);
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
