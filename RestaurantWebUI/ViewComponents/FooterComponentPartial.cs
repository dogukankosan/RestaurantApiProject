using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebUI.ViewComponents
{
    public class FooterComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
