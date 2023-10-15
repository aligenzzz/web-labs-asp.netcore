using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Services.BookService;

namespace Web_153505_Bybko.Areas.Admin.Pages
{
    // [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly IBookService _context;

        public DeleteModel(IBookService context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _context.DeleteBookAsync(id ?? default);

            return RedirectToPage("./Index");
        }
    }
}
