using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;

namespace Web_153505_Bybko.API.Services.BookService
{
    public interface IBookService
    {
        /// <summary>
        /// Get list of all books
        /// </summary>
        /// <param name="genreName">Genre's name</param>
        /// <param name="pageNo">Number of list's page</param>
        /// <param name="pageSize">Books' count on the one page</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Book>>> GetBooksListAsync(string? genreName = "All", int pageNo = 1, int pageSize = 3);

        /// <summary>
        /// Get book by Id
        /// </summary>
        /// <param name="id">Book's Id</param>
        /// <returns>Found book or null</returns>
        public Task<ResponseData<Book>> GetBookByIdAsync(int id);

        /// <summary>
        /// Book's updating
        /// </summary>
        /// <param name="id">Updating book's Id</param>
        /// <param name="book">Book with new data</param>
        /// <returns></returns>
        public Task UpdateBookAsync(int id, Book book);

        /// <summary>
        /// Book's deleting
        /// </summary>
        /// <param name="id">Deliting book's Id</param>
        /// <returns></returns>
        public Task DeleteBookAsync(int id);

        /// <summary>
        /// Book's creating 
        /// </summary>
        /// <param name="book">New book</param>
        /// <returns>Creating book</returns>
        public Task<ResponseData<Book>> CreateBookAsync(Book book);

        /// <summary>
        /// Save image to book
        /// </summary>
        /// <param name="id">Book's Id</param>
        /// <param name="formFile">Image's file</param>
        /// <returns>Url to image's file</returns>
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
