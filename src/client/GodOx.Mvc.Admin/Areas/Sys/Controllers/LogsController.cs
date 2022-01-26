using GodOx.Share.Repository;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class LogsController : Controller
    {
        private readonly IBaseServer<Log> _logService;
        public LogsController(IBaseServer<Log> logService)
        {
            _logService = logService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var model = await _logService.GetModelAsync(d => d.Id == id);
            return View(model);
        }
        [HttpGet]
        public IActionResult Echarts()
        {
            return View();
        }
    }
}
