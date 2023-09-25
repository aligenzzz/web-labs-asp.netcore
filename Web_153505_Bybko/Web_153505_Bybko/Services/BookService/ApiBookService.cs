using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;

namespace Web_153505_Bybko.Services.BookService
{
    public class ApiBookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiBookService> _logger;

        public ApiBookService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiBookService> logger) 
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value!;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public Task<ResponseData<ListModel<Book>>> GetBooksListAsync(string? genreName = "All", int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}books/");

            if (genreName != "All")
                urlString.Append($"{genreName}/");

            if (pageNo > 1)
                urlString.Append($"pageno{pageNo}");

            if (!_pageSize.Equals("3"))
                urlString.Append($"pagesize{_pageSize}");

            var response = _httpClient.GetAsync(new Uri(urlString.ToString())).Result;
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return response.Content.ReadFromJsonAsync<ResponseData<ListModel<Book>>>(_serializerOptions)!;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return Task.FromResult(new ResponseData<ListModel<Book>>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    });
                }
            }

            _logger.LogError($"Error: { response.StatusCode.ToString() }");
            return Task.FromResult(new ResponseData<ListModel<Book>> 
            {
                Success = false,
                ErrorMessage = $"Error: { response.StatusCode.ToString() }"
            });
        }

        public Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient!.BaseAddress!.AbsoluteUri + "books");

            var response = _httpClient.PostAsJsonAsync(uri, book, _serializerOptions).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions)!;

            _logger.LogError($"Error: { response.StatusCode.ToString() }");
            return Task.FromResult(new ResponseData<Book>
            {
                Success = false,
                ErrorMessage = $"Error: { response.StatusCode.ToString() }"
            });
        }

        public Task DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBookAsync(int id, Book book, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
