using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;
using Web_153505_Bybko.Services.BookService;

namespace Web_153505_Bybko.Areas.Admin.Pages
{
    // [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly IBookService _context;

        public IndexModel(IBookService context)
        {
            _context = context;
        }

        public ListModel<Book> Book { get; set; } = default!;

        public async Task OnGetAsync(int pageno = 1)
        {
            var answer = await _context.GetBooksListAsync("All", pageno);

            if (answer.Success && answer.Data != null)
            {
                Book = answer.Data;
            }
        }
    }
}
