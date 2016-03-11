using Microsoft.AspNet.Mvc;

namespace DistanceTracker
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("~/spa/Index.cshtml");
        }

        public IActionResult Error()
        {
            return Content("Sorry, unexpected error has occured.");
        }
    }
}
