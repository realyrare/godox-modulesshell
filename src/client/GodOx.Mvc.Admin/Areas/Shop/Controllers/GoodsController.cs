using GodOx.Share.Repository;
using GodOx.Shop.API.Services;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Shop.Controllers
{
    [Area("shop")]
    public class GoodsController : Controller
    {
        private readonly IGoodsService _goodsService;
        private readonly IBaseServer<Config> _configService;
        public GoodsController(IGoodsService goodsService, IBaseServer<Config> configService)
        {
            _goodsService = goodsService;
            _configService = configService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var datas = await _configService.GetListAsync(d => d.Type.Equals("Freight"));
            ViewBag.Freights = datas;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            var result = await _goodsService.DetailAsync(id);
            var datas = await _configService.GetListAsync(d => d.Type.Equals("Freight"));
            ViewBag.Freights = datas;

            return View(result.Data);
        }
    }
}
