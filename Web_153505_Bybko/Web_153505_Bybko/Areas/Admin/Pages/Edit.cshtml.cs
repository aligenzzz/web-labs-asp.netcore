using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Services.GenreService;
using Web_153505_Bybko.Services.BookService;
using System.Data;

namespace Web_153505_Bybko.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IBookService _context;
        private readonly IGenreService _service;

        public EditModel(IBookService context, IGenreService service)
        {
            _context = context;
            _service = service;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;
        [BindProperty]
        public int GenreId { get; set; } = 1;

        public SelectList? SelectList { get; set; }

        [BindProperty]
        public IFormFile? Image { get; set; }

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
            
            SelectList = new SelectList((await _service.GetGenresListAsync()).Data,
                            nameof(Genre.Id), nameof(Genre.Name), Book.Genre!.Name);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Book.GenreId = GenreId;
            Book.Genre = (await _service.GetGenresListAsync()).Data!.Where(g => g.Id == GenreId).FirstOrDefault();

            // clear previous possible errors
            ModelState.ClearValidationState(nameof(Book));

            if (!TryValidateModel(Book, nameof(Book)))
            {
                SelectList = new SelectList((await _service.GetGenresListAsync()).Data,
                                        nameof(Genre.Id), nameof(Genre.Name));
                return Page();
            }

            await _context.UpdateBookAsync(Book.Id, Book, Image);

            return RedirectToPage("./Index");
        }
    }
}
