using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;

namespace Web_153505_Bybko.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        public List<Genre> Genres { get; set; } = new();
        public List<Book> Books { get; set; } = new();
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public int TotalPages { get; set; } = 0;
        public int CurrentPage { get; set; } = 1;

        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _tokenProvider;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly string _pageSize;

        public event Action? DataLoaded;

        public Book? Book { get; set; }

        public DataService(HttpClient httpClient, IAccessTokenProvider tokenProvider, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            _pageSize = configuration.GetSection("ItemsPerPage").Value!;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task GetBookByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}books/{id}");

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions) 
                            ?? throw new ArgumentNullException();

                        Book = result.Data;
                        Success = true;

                        DataLoaded!.Invoke();
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Error: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Error:{response.StatusCode}";
                }
            }
            else
            {
                Success = false;
                ErrorMessage = $"Cannot recieve token.";
            }
        }
        public async Task GetBookListAsync(string genre = "All", int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}books/");

            if (genre != "All")
                urlString.Append($"{genre}/");
            if (pageNo > 1)
                urlString.Append($"pageno{pageNo}");
            if (!_pageSize.Equals("3"))
                urlString.Append($"pagesize{_pageSize}");

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        ResponseData<ListModel<Book>> result = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Book>>>
                                        (_serializerOptions) ?? throw new ArgumentNullException();

                        Books = result.Data!.Items;
                        Success = true;
                        TotalPages = result.Data.TotalPages;
                        CurrentPage = result.Data.CurrentPage;

                        DataLoaded!.Invoke();
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Error: {ex.Message}";
                    }
                    catch (ArgumentNullException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Error: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Error:{response.StatusCode}";
                }
            }
            else
            {
                Success = false;
                ErrorMessage = $"Cannot recieve token.";
            }
        }
        public async Task GetGenreListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}genres/");

            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        ResponseData<List<Genre>> result = await response.Content.ReadFromJsonAsync<ResponseData<List<Genre>>>
                                        (_serializerOptions) ?? throw new ArgumentNullException();

                        Genres = result.Data!;
                        Success = true;

						DataLoaded!.Invoke();
					}
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Error: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Error:{response.StatusCode}";
                }
            }
            else
            {
                Success = false;
                ErrorMessage = $"Cannot recieve token.";
            }
        }
    }
}
