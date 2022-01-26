using GodOx.Share.Repository;
using GodOx.Shop.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Shop.Controllers
{
    [Area("shop")]
    public class CategoryController : Controller
    {
        private readonly IBaseServer<Category> _categoryService;

        public CategoryController(IBaseServer<Category> categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            Category model = id == 0 ? new Category() : await _categoryService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
