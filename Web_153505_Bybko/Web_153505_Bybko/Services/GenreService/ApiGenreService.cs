using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;
using Web_153505_Bybko.Services.AuthorService;
using Web_153505_Bybko.Services.BookService;

namespace Web_153505_Bybko.Services.GenreService
{
    public class ApiGenreService : IGenreService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiGenreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public Task<ResponseData<List<Genre>>> GetGenresListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}genres/");

            var response = _httpClient.GetAsync(new Uri(urlString.ToString())).Result;
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return response.Content.ReadFromJsonAsync<ResponseData<List<Genre>>>(_serializerOptions)!;
                }
                catch (JsonException ex)
                {
                    return Task.FromResult(new ResponseData<List<Genre>>
                    {
                        Success = false,
                        ErrorMessage = $"Error: {ex.Message}"
                    });
                }
            }

            return Task.FromResult(new ResponseData<List<Genre>>
            {
                Success = false,
                ErrorMessage = $"Error: {response.StatusCode.ToString()}"
            });
        }
    }
}
