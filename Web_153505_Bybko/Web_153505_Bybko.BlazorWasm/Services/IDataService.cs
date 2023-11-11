using Web_153505_Bybko.Domain.Entities;

namespace Web_153505_Bybko.BlazorWasm.Services
{
    public interface IDataService
    {
        List<Genre> Genres { get; set; }
        List<Book> Books { get; set; }
        bool Success { get; set; }
        string ErrorMessage { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }

		event Action DataLoaded;

		public Task GetBookListAsync(string genre = "All", int pageNo = 1);
        public Task GetBookByIdAsync(int id);
        public Task GetGenreListAsync();
    }
}
