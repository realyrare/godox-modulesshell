using GodOx.Share.Repository;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class RoleController : Controller
    {
        private readonly IBaseServer<Role> _roleService;
        public RoleController(IBaseServer<Role> roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        //
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            Role model = id == 0 ? new Role() : await _roleService.GetModelAsync(d => d.Id == id);
            return View(model);
        }
        [HttpGet]
        public IActionResult SetMenu(int id)
        {
            ViewBag.RoleId = id;
            return View();
        }
    }
}
