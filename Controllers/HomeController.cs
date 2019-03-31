using Microsoft.AspNetCore.Mvc;

namespace GlobalCityManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
