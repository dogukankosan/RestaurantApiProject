using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebUI.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        [Route("Anasayfa")]
        public IActionResult Home()
        {
            return View();
        }
    }
}