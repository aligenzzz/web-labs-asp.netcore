using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Web_153505_Bybko.Controllers;
using Web_153505_Bybko.Services.BookService;
using Web_153505_Bybko.Services.GenreService;
using Web_153505_Bybko.Domain.Models;
using Web_153505_Bybko.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Web_153505_Bybko.Tests.Controllers
{
    public class BookControllerTest
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly Mock<IGenreService> _mockGenreService;
        private readonly BookController _bookController;

        public BookControllerTest()
        {
            _mockBookService = new();
            _mockGenreService = new();
            _bookController = new(_mockBookService.Object, _mockGenreService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public void GetGenreListReturns404()
        {
            _mockGenreService.Setup(s => s.GetGenresListAsync().Result)
                .Returns(new ResponseData<List<Genre>>() { Success = false });

            var result = (NotFoundObjectResult) _bookController.Index().Result;

            Assert.Equal((int) HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public void GetBookListReturns404()
        {
            _mockGenreService.Setup(s => s.GetGenresListAsync().Result)
                .Returns(new ResponseData<List<Genre>>() { Success = false });

            _mockBookService.Setup(s => s.GetBooksListAsync("All", 1).Result)
                .Returns(new ResponseData<ListModel<Book>>() { Success = false });

            var result = (NotFoundObjectResult)_bookController.Index().Result;

            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public void ViewDataContainsGenreList()
        {
            _mockGenreService.Setup(s => s.GetGenresListAsync().Result).Returns(
                new ResponseData<List<Genre>>()
                {
                    Success = true,
                    Data = new List<Genre>()
                });
            _mockBookService.Setup(s => s.GetBooksListAsync("All", 1).Result).Returns(
                new ResponseData<ListModel<Book>>() 
                { 
                    Success = true,
                    Data = new ListModel<Book>()
                });

            var result = (ViewResult) _bookController.Index().Result;

            Assert.IsType<List<Genre>>(result.ViewData["Genres"]);
        }

        [Fact]
        public void ViewDataCantainsCurrentGenre()
        {
            string genre = "All";

            _mockGenreService.Setup(s => s.GetGenresListAsync().Result).Returns(
                new ResponseData<List<Genre>>()
                {
                    Success = true,
                    Data = new List<Genre>()
                });
            _mockBookService.Setup(s => s.GetBooksListAsync(genre, 1).Result).Returns(
                new ResponseData<ListModel<Book>>()
                {
                    Success = true,
                    Data = new ListModel<Book>()
                });

            var result = (ViewResult) _bookController.Index().Result;

            Assert.Equal(genre, result.ViewData["currentGenre"]);
        }

        [Fact]
        public void ViewContainsBookListModel()
        {
            _mockGenreService.Setup(s => s.GetGenresListAsync().Result).Returns(
                new ResponseData<List<Genre>>()
                {
                    Success = true,
                    Data = new List<Genre>()
                });
            _mockBookService.Setup(s => s.GetBooksListAsync("All", 1).Result).Returns(
                new ResponseData<ListModel<Book>>()
                {
                    Success = true,
                    Data = new ListModel<Book>()
                });

            var result = _bookController.Index().Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ListModel<Book>>(viewResult.Model);
        }
    }
}
