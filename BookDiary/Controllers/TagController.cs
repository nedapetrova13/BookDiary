﻿using BookDiary.Core.IServices;
using BookDiary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookDiary.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _tagService.GetAllTags();
            return View(list);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Tag tag)
        {
            await _tagService.Add(tag);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagService.GetById(id);
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Tag tag)
        {
            await _tagService.Update(tag);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
