using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
