using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
