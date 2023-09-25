using Microsoft.EntityFrameworkCore;
using Web_153505_Bybko.Domain.Entities;

namespace Web_153505_Bybko.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Book> Books => Set<Book>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
    }
}
