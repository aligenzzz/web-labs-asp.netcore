using Microsoft.AspNetCore.Mvc;
using Web_153505_Bybko.Services.AuthorService;
using Web_153505_Bybko.Services.BookService;

namespace Web_153505_Bybko.Controllers
{
    public class Book : Controller
    {
        IBookService _bookService;
        IGenreService _genreService;
      
        public Book(IBookService bookService, IGenreService genreService) 
        { 
            _bookService = bookService;
            _genreService = genreService;
        }
        public async Task<IActionResult> Index(string genre = "All", int pageno = 1)
        {
            ViewBag.genres = _genreService.GetGenresListAsync().Result.Data;
            ViewData["currentGenre"] = genre;

            var bookResponse = await _bookService.GetBooksListAsync(genre, pageno);

            if (!bookResponse.Success)
                return NotFound(bookResponse.ErrorMessage);

            return View(bookResponse.Data);
        }
    }
}
