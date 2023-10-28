using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_153505_Bybko.Domain.Models;
using Web_153505_Bybko.Services.BookService;

namespace Web_153505_Bybko.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IBookService _bookService;
        private readonly Cart _cart;
        public CartController(IBookService bookService, Cart cart)
        {
            _bookService = bookService;
            _cart = cart ?? throw new ArgumentNullException();
        }

        public ActionResult Index()
        {
            return View(_cart.CartItems);
        }

        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _bookService.GetBookByIdAsync(id);
           
            if (data.Success)
            {
                _cart.AddToCart(data.Data!);
            }

            return Redirect(returnUrl);
        }

        public ActionResult Delete(int id)
        {
            _cart.RemoveItems(id);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult ClearAll()
        {
            _cart.ClearAll();

            return RedirectToAction(nameof(Index));
        }
    }
}
