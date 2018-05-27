using Microsoft.AspNetCore.Mvc;

namespace MiningMonitor.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}