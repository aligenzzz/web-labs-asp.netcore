using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;

namespace Web_153505_Bybko.Services.BookService
{
    public interface IBookService
    {
        /// <summary>
        /// Get list of all books
        /// </summary>
        /// <param name="genreName">Genre's name</param>
        /// <param name="pageNo">Number of list's page</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Book>>> GetBooksListAsync(string? genreName = "All", int pageNo = 1);

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
        /// <param name="formFile">Image's file</param>
        /// <returns></returns>
        public Task UpdateBookAsync(int id, Book book, IFormFile? formFile);

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
        /// <param name="formFile">Image's file</param>
        /// <returns>Creating book</returns>
        public Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile);
    }
}
