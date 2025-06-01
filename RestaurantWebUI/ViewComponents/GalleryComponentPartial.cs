using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebUI.ViewComponents
{
    public class GalleryComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}