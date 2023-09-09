using Microsoft.AspNetCore.Mvc;

namespace Web_153505_Bybko.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
