using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebUI.ViewComponents
{
    public class ServiceComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}