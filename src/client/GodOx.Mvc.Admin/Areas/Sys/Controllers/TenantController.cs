using GodOx.Share.Repository;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class TenantController : Controller
    {
        private readonly IBaseServer<Tenant> _tenantService;

        public TenantController(IBaseServer<Tenant> tenantService)
        {
            _tenantService = tenantService;
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
            Tenant tenant = null;
            if (id == 0)
            {
                tenant = new Tenant();
            }
            else
            {
                tenant = await _tenantService.GetModelAsync(d => d.Id == id && d.IsDel == false);
            }
            return View(tenant);
        }
    }
}
