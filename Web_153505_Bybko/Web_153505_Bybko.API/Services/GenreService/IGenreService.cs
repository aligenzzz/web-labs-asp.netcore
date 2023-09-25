using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;

namespace Web_153505_Bybko.API.Services.GenreService
{
    public interface IGenreService
    {
        /// <summary>
        /// Get list of all genres
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Genre>>> GetGenresListAsync();
    }
}
