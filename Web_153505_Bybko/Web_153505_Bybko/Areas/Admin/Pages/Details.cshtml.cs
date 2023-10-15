using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Services.BookService;

namespace Web_153505_Bybko.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IBookService _context;

        public DetailsModel(IBookService context)
        {
            _context = context;
        }

      public Book Book { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.GetBookByIdAsync(id ?? default(int));
            if (book.Success == false)
            {
                return NotFound();
            }

            if (book.Data != null)
            {
                Book = book.Data;
            }

            return Page();
        }
    }
}
