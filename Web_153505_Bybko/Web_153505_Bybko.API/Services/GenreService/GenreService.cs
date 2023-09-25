using Microsoft.EntityFrameworkCore;
using Web_153505_Bybko.API.Data;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;

namespace Web_153505_Bybko.API.Services.GenreService
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _context;

        public GenreService(AppDbContext context)
        {
            _context = context;
        }

        public Task<ResponseData<List<Genre>>> GetGenresListAsync()
        {
            var result = new ResponseData<List<Genre>>();
            result.Data = _context.Genres.ToListAsync().Result;

            return Task.FromResult(result);
        }
    }
}
