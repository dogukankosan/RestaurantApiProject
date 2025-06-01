using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebUI.ViewComponents
{
    public class ReservationComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}