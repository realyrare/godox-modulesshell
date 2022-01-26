using Microsoft.AspNetCore.Mvc;

namespace GodOx.Mvc.Admin.Areas.Shop.Controllers
{
    [Area("shop")]
    public class AppUserController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
