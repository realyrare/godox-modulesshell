using Microsoft.AspNetCore.Mvc;

namespace GodOx.Mvc.Admin.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("error.html")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
