using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        [Route("/Admin")]
        public IActionResult Index()
        {
            ViewBag.CssFiles = new List<string>();
            ViewBag.CssFiles.Add("admin.css");

            return View();
        }
    }
}
