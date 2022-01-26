using GodOx.Cms.API.Models.Entity;
using GodOx.Share.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Cms.Controllers
{
    [Area("cms")]
    public class ArticleController : Controller
    {
        private readonly IBaseServer<Article> _articleService;

        public ArticleController(IBaseServer<Article> articleService)
        {
            _articleService = articleService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            var model = id == 0 ? new Article() : await _articleService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
