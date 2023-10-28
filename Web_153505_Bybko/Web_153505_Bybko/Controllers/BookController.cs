using Microsoft.AspNetCore.Mvc;
using Web_153505_Bybko.Services.GenreService;
using Web_153505_Bybko.Services.BookService;
using Web_153505_Bybko.Extensions;

namespace Web_153505_Bybko.Controllers
{
    public class BookController : Controller
    {
        IBookService _bookService;
        IGenreService _genreService;
      
        public BookController(IBookService bookService, IGenreService genreService) 
        { 
            _bookService = bookService;
            _genreService = genreService;
        }

        [Route("Catalog")]
        [Route("Catalog/{genre}")]
        public async Task<IActionResult> Index(string genre = "All", int pageno = 1)
        {
            var genreResponse = await _genreService.GetGenresListAsync();

            if (!genreResponse.Success)
                return NotFound(genreResponse.ErrorMessage);

            ViewBag.genres = genreResponse.Data;

            ViewData["currentGenre"] = genre;

            var bookResponse = await _bookService.GetBooksListAsync(genre, pageno);

            if (!bookResponse.Success)
                return NotFound(bookResponse.ErrorMessage);

            if (Request.IsAjaxRequest())
                return PartialView("_BooksPartial", bookResponse.Data);

            return View(bookResponse.Data);
        }
    }
}
