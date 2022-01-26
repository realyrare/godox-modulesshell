using GodOx.Share.Repository;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Models.Dtos.Output;
using GodOx.Sys.API.Models.Entity;
using GodOx.Sys.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IBaseServer<Config> _configService;

        private readonly ICurrentUserContext _currentUserContext;

        public MenuController(IMenuService menuService, IBaseServer<Config> configService, ICurrentUserContext currentUserContext)
        {
            _menuService = menuService;
            _configService = configService;
            _currentUserContext = currentUserContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id)
        {
            MenuDetailOutput model = new MenuDetailOutput();
            string alreadyBtns = string.Empty;
            if (id > 0)
            {
                model.MenuOutput = await _menuService.GetModelAsync(d => d.Id == id);
                if (model.MenuOutput != null)
                {
                    if (model.MenuOutput.BtnCodeIds.Length > 0)
                    {
                        for (int i = 0; i < model.MenuOutput.BtnCodeIds.Length; i++)
                        {
                            alreadyBtns += model.MenuOutput.BtnCodeIds[i] + ",";
                        }
                        if (!string.IsNullOrEmpty(alreadyBtns))
                        {
                            alreadyBtns = alreadyBtns.TrimEnd(',');
                        }
                    }
                }
                ViewBag.AlreadyBtns = alreadyBtns;
            }
            else
            {
                model.MenuOutput = new Menu();
            }
            var configs = await _configService.GetListAsync(d => d.Type == nameof(Button));
            model.ConfigOutputs = configs;
            return View(model);
        }
    }
}
