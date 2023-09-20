
namespace Web_153505_Bybko.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? MIMEType { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; } = null;
        public string? Author {  get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
    }
}
