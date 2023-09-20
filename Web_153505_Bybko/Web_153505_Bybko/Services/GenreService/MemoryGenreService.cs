using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;
using Web_153505_Bybko.Services.AuthorService;

namespace Web_153505_Bybko.Services.GenreService
{
    public class MemoryGenreService : IGenreService
    {
        public Task<ResponseData<List<Genre>>> GetGenresListAsync()
        {
            var genres = new List<Genre>()
            {
                new Genre { Id = 1, Name = "Detective", Description = "Detective story, type of popular literature in which a crime " +
                                                        "is introduced and investigated and the culprit is revealed. Detective stories " +
                                                        "frequently operate on the principle that superficially convincing evidence is " +
                                                        "ultimately irrelevant." },
                new Genre { Id = 2, Name = "Manga", Description = "" },
            };

            var result = new ResponseData<List<Genre>>();
            result.Data = genres;

            return Task.FromResult(result);
        }
    }
}
