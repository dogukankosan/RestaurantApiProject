using Microsoft.AspNetCore.Mvc;
namespace RestaurantWebUI.ViewComponents
{
    public class ContactComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}