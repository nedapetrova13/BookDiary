using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
