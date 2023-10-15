using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Services.GenreService;
using Web_153505_Bybko.Services.BookService;
using System.Data;

namespace Web_153505_Bybko.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IBookService _context;
        private readonly IGenreService _service;

        public CreateModel(IBookService context, IGenreService service)
        {
            _context = context;
            _service = service;
        }

        public IActionResult OnGet()
        {
            SelectList = new SelectList(_service.GetGenresListAsync().Result.Data, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        [BindProperty]
        public int GenreId { get; set; } = 1;

        public SelectList? SelectList { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Book.GenreId = GenreId;
            Book.Genre = (await _service.GetGenresListAsync()).Data!.Where(g => g.Id == GenreId).FirstOrDefault();

            ModelState.ClearValidationState(nameof(Book));

            if (!TryValidateModel(Book, nameof(Book)) || Book == null || Image == null)
            {
                SelectList = new SelectList(_service.GetGenresListAsync().Result.Data, "Id", "Name");
                return Page();
            }

            var response = await _context.CreateBookAsync(Book, Image);

            if (!response.Success)
                throw new Exception(response.ErrorMessage);

            return RedirectToPage("./Index");
        }
    }
}
