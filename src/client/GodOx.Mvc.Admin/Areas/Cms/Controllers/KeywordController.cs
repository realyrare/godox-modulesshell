using GodOx.Cms.API.Models.Entity;
using GodOx.Share.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Cms.Controllers
{
    [Area("cms")]
    public partial class KeywordController : Controller
    {
        private readonly IBaseServer<Keyword> _keywordService;

        public KeywordController(IBaseServer<Keyword> KeywordService)
        {
            _keywordService = KeywordService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            Keyword model = id == 0 ? new Keyword() : await _keywordService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
