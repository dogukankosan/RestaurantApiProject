using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebUI.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound404()
        {
            return View();
        }
        public IActionResult ServerError500()
        {
            return View();
        }
    }
}