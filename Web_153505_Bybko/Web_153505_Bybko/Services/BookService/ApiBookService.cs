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

        public async Task<ResponseData<ListModel<Book>>> GetBooksListAsync(string? genreName = "All", int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}books/");

            if (genreName != "All")
                urlString.Append($"{genreName}/");

            if (pageNo > 1)
                urlString.Append($"pageno{pageNo}");

            if (!_pageSize.Equals("3"))
                urlString.Append($"pagesize{_pageSize}");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return (await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Book>>>(_serializerOptions))!;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<ListModel<Book>>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    };
                }
            }

            _logger.LogError($"Error: { response.StatusCode.ToString() }");
            return new ResponseData<ListModel<Book>> 
            {
                Success = false,
                ErrorMessage = $"Error: { response.StatusCode.ToString() }"
            };
        }

        public async Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}books");

            var response = await _httpClient.PostAsJsonAsync(new Uri(urlString.ToString()), book);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var book_ = await response.Content.ReadFromJsonAsync<Book>(_serializerOptions);

                    if (formFile != null)
                        await SaveImageAsync(book_!.Id, formFile);

                    return new ResponseData<Book>
                    {
                        Data = book_,
                        Success = true,
                        ErrorMessage = ""
                    };
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<Book>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    };
                }
            }
               
            _logger.LogError($"Error: { response.StatusCode.ToString() }");
            return new ResponseData<Book>
            {
                Success = false,
                ErrorMessage = $"Error: { response.StatusCode.ToString() }"
            };
        }

        public async Task DeleteBookAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}books/{id}");

            var response = await _httpClient.DeleteAsync(new Uri(urlString.ToString()));

            _logger.LogError($"Error: {response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}books/{id}");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return (await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions))!;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    return new ResponseData<Book>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    };
                }
            }

            
            return new ResponseData<Book>
            {
                Success = false,
                ErrorMessage = $"Error: {response.StatusCode.ToString()}"
            };

        }

        public async Task UpdateBookAsync(int id, Book book, IFormFile? formFile)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}books/{id}");

            var response = await _httpClient.PutAsJsonAsync(new Uri(urlString.ToString()), book);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    if (formFile != null)
                        await SaveImageAsync(id, formFile);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }  
            }            
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}books/{id}")
            };

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());

            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;

            var answer = await _httpClient.SendAsync(request);
            if (!answer.IsSuccessStatusCode)
                throw new Exception("Something went wrong while saving image...");
        }
    }
}
