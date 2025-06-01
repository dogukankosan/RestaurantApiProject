using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebUI.ViewComponents
{
    public class EventsComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}