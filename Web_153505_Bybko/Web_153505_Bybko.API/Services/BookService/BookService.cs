using Microsoft.EntityFrameworkCore;
using Web_153505_Bybko.API.Data;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;

namespace Web_153505_Bybko.API.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public Task<ResponseData<ListModel<Book>>> GetBooksListAsync(string? genreName = "All", int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var result = new ResponseData<ListModel<Book>>();

            result.Data = new();
            List<Book> books = new();
            if (genreName == "All")
                books = _context.Books.Include(b => b.Genre).ToList();
            else
                books = _context.Books.Include(b => b.Genre)
                                      .Where(b => b.Genre != null && b.Genre.Name.Equals(genreName))
                                      .ToList();

            int totalBooks = 0;
            if (books != null) totalBooks = books.Count();

            result.Data.TotalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
            if (pageNo > result.Data.TotalPages)
                return Task.FromResult(new ResponseData<ListModel<Book>>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No such page"
                });
            result.Data.CurrentPage = pageNo;

            if (books != null)
                result.Data.Items = books.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            return Task.FromResult(result);
        }

        public Task<ResponseData<Book>> CreateBookAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBookAsync(int id, Book book)
        {
            throw new NotImplementedException();
        }
    }
}
