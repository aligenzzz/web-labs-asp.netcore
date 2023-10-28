using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using Web_153505_Bybko.API.Data;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;

namespace Web_153505_Bybko.API.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _accessor;


        public BookService(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _context = context;
            _env = env;
            _accessor = accessor;
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

        public async Task<ResponseData<Book>> CreateBookAsync(Book book)
        {
            book.Genre = _context.Genres.Where(g => g.Id == book.GenreId).FirstOrDefault()!;

            if (book.Genre == null)
                throw new Exception("Book doesn't have genre...");

            await _context.Books.AddAsync(book);

            _context.SaveChanges();

            return new ResponseData<Book> { Data = book, Success = true, ErrorMessage = "" };
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                await DeleteImageAsync(id);
                _context.Books.Remove(book);
            }
                
            _context.SaveChanges();
        }

        public Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            var books = _context.Books.AsQueryable();

            var data = books.Where(b => b.Id == id).Include(b => b.Genre).FirstOrDefaultAsync().Result;
            var response = new ResponseData<Book>();

            if (data != null)
            {
                response.Data = data;
            }           
            else
            {
                response.ErrorMessage = "Can't find such book...";
                response.Success = false;
            }

            return Task.FromResult(response);
        }

        public async Task UpdateBookAsync(int id, Book book)
        {
            var book_ = (await GetBookByIdAsync(id)).Data;

            if (book_ != null)
            {
                book_.Author = book.Author;
                book_.Price = book.Price;
                book_.Description = book.Description;
                book_.Name = book.Name;
                book_.GenreId = book.GenreId;

                if (!String.IsNullOrEmpty(book.Image))
                    book_.Image = book.Image;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = await GetBookByIdAsync(id);
            Book book;

            if (!responseData.Success)
            {
                return new ResponseData<string>
                { Success = false, ErrorMessage = responseData.ErrorMessage };

            }
            else
            {
                book = responseData.Data!;
            }

            var host = "https://" + _accessor.HttpContext!.Request.Host;
            var imageFolder = Path.Combine(_env.WebRootPath, "Images/books");

            if (formFile != null)
            {
                // delete previous book's image
                if (book != null && book.Image != null)
                {
                    var prevImage = Path.Combine(imageFolder, Path.GetFileName(book.Image));
                    File.Delete(prevImage);
                }

                // create file name
                var ext = Path.GetExtension(formFile.FileName);
                var fileName = Path.ChangeExtension(Path.GetRandomFileName(), ext);

                var filePath = Path.Combine(imageFolder, fileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                book!.Image = $"{host}/Images/books/{fileName}";
                book.MIMEType = ext;

                await _context.SaveChangesAsync();
                return new ResponseData<string> { Data = book.Image };
            }
            else
            {
                return new ResponseData<string>
                {
                    Success = false,
                    ErrorMessage = "Error: no file where provided"
                };
            }
        }

        public async Task<ResponseData<string>> DeleteImageAsync(int id)
        {
            var responseData = await GetBookByIdAsync(id);
            Book book;

            if (!responseData.Success)
            {
                return new ResponseData<string>
                { Success = false, ErrorMessage = responseData.ErrorMessage };

            }
            else
            {
                book = responseData.Data!;
            }

            var host = "https://" + _accessor.HttpContext!.Request.Host;
            var imageFolder = Path.Combine(_env.WebRootPath, "Images/books");

            if (book.Image != null)
            {
                var image = Path.Combine(imageFolder, Path.GetFileName(book.Image));
                File.Delete(image);

                return new ResponseData<string> { Success = true };
            }
            else
            {
                return new ResponseData<string>
                {
                    Success = false,
                    ErrorMessage = "Error: such book doesn't exist"
                };
            }
        }
    }
}
