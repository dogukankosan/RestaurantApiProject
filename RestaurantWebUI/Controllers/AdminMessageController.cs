using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebUI.Controllers
{
    public class AdminMessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
