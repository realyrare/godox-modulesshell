using GodOx.Cms.API.Models.Entity;
using GodOx.Share.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Cms.Controllers
{
    [Area("cms")]
    public class ColumnController : Controller
    {
        private readonly IBaseServer<Column> _columnService;

        public ColumnController(IBaseServer<Column> columnService)
        {
            _columnService = columnService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            Column model = id == 0 ? new Column() : await _columnService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
