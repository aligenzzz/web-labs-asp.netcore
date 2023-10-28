using Web_153505_Bybko.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;
using Web_153505_Bybko.API.Services.BookService;

namespace Web_153505_Bybko.Tests.Services
{
    public class ApiBookServiceTest : IDisposable
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly DbConnection _connection;

        public ApiBookServiceTest() 
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<AppDbContext>()
               .UseSqlite(_connection)
               .Options;

            using var context = new AppDbContext(_options);
            context.Database.EnsureCreated();

            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "TestGenre1" },
                new Genre { Id = 2, Name = "TestGenre2" }
            };
            var books = new List<Book>
            {
                new Book { Id = 1, GenreId = 1, Name = "TestBook1" },
                new Book { Id = 2, GenreId = 2, Name = "TestBook2" },
                new Book { Id = 3, GenreId = 2, Name = "TestBook3" },
                new Book { Id = 4, GenreId = 1, Name = "TestBook4" }
            };

            context.Genres.AddRange(genres);
            context.Books.AddRange(books);
            context.SaveChanges();
        }

        private AppDbContext CreateContext() => new AppDbContext(_options);
        public void Dispose() => _connection.Dispose();

        [Fact]
        public void GetFirstPageWithThreeBooksList()
        {
            using var context = CreateContext();

            var service = new BookService(context, null!, null!);
            var result = service.GetBooksListAsync().Result;

            Assert.Equal(1, result.Data!.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.IsType<ListModel<Book>>(result.Data);
        }

        [Fact]
        public void GetBooksListOnCurrentPage()
        {
            using var context = CreateContext();

            var service = new BookService(context, null!, null!);
            var result = service.GetBooksListAsync("All", 2).Result;

            Assert.Equal(2, result.Data!.CurrentPage);
            Assert.IsType<ListModel<Book>>(result.Data);
        }

        [Fact]
        public void GetBooksListWithCertainGenre()
        {
            string genre = "TestGenre1";

            using var context = CreateContext();

            var bookService = new BookService(context, null!, null!);
            var result = bookService.GetBooksListAsync(genre).Result;

            Assert.IsType<ListModel<Book>>(result.Data);
            Assert.True(result.Data.Items.All(b => b.Genre!.Name.Equals(genre)));
        }

        [Fact]
        public void GetBooksListWithGreaterMaxPageSize()
        {
            int pageSize = 21;

            using var context = CreateContext();

            var bookService = new BookService(context, null!, null!);
            var result = bookService.GetBooksListAsync("All", 1, pageSize).Result;

            Assert.Equal(context.Books.Count(), result.Data!.Items.Count);
            Assert.IsType<ListModel<Book>>(result.Data);
        }

        [Fact]
        public void GetBooksListWithWrongPage()
        {
            int page = 3;

            using var context = CreateContext();

            var bookService = new BookService(context, null!, null!);
            var result = bookService.GetBooksListAsync("All", page).Result;

            Assert.False(result.Success);
        }
    }
}
