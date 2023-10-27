using Microsoft.AspNetCore.Mvc;
using Web_153505_Bybko.Domain.Models;

namespace Web_153505_Bybko.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }
        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
