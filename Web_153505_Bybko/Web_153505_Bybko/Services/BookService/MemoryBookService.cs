using Microsoft.AspNetCore.Mvc;
using Web_153505_Bybko.Domain.Entities;
using Web_153505_Bybko.Domain.Models;
using Web_153505_Bybko.Services.AuthorService;

namespace Web_153505_Bybko.Services.BookService
{
    public class MemoryBookService : IBookService
    {
        List<Book> _books = new();
        List<Genre>? _genres;

        int _itemsPerPage;

        public MemoryBookService([FromServices] IConfiguration config, IGenreService genreService)
        {
            _itemsPerPage = config.GetValue<int>("ItemsPerPage");

            _genres = genreService.GetGenresListAsync().Result.Data;
            SetupData();
        }

        /// <summary>
        /// List's initialization
        /// </summary>
        private void SetupData() 
        {
            if (_genres == null) return;

            _books = new List<Book> 
            { 
                new Book { Id = 1, Name = "Sherlock Holmes: The Hound of the Baskervilles", Image = "Images/books/1.jpg",
                           Genre = _genres.Find(g => g.Name.Equals("Detective")), Author = "Sir Arthur Conan Doyle",
                           Description = "With a deadly phantom hound on the loose and a mysterious man living on the moors, " +
                                         "Devon is a dangerous place to be. But Holmes and Watson must put their fears aside. " +
                                         "The country’s favourite crime-fighting duo need to unravel the strange case of Sir Charles " +
                                         "Baskerville’s murder before his nephew meets the same fate.", Price = 7.99 },
                new Book { Id = 2, Name = "And Then There Were None", Image = "Images/books/2.jpg",
                           Genre = _genres.Find(g => g.Name.Equals("Detective")), Author = "Agatha Christie", 
                           Description = "Ten people, each with something to hide and something to fear, are invited to an isolated mansion " +
                                         "on Indian Island by a host who, surprisingly, fails to appear. On the island they are cut off from " +
                                         "everything but each other and the inescapable shadows of their own past lives. One by one, the guests " +
                                         "share the darkest secrets of their wicked pasts. And one by one, they die…", Price = 9.99 },
                new Book { Id = 3, Name = "A COFFIN FROM HONG KONG", Image = "Images/books/3.jpg",
                           Genre = _genres.Find(g => g.Name.Equals("Detective")), Author = "James Hadley Chase",
                           Description = "A mysterious voice on the telephone, a beautiful Chinese girl brutally murdered, a doddering millionaire with " +
                                         "a guilty conscience, a private detective ensnared by unfathomable crimes, a Kowloon hooker who talks too much for " +
                                         "her own good . . . and a coffin from Hong Kong . . .These are the barbs that pull P.I. Ryan Nelson into the " +
                                         "strangest case of his career. To solve the mystery, he must leave the postcard-pretty beaches of his California " +
                                         "home and burrow into the sinister alleys of the ancient walled city of Kowloon.", Price = 3.99 },
                
                new Book { Id = 4, Name = "Attack on Titan 1", Image = "Images/books/4.jpg",
                           Genre = _genres.Find(g => g.Name.Equals("Manga")), Author = "Hajime Isayama",
                           Description = "In this post-apocalytpic sci-fi story, humanity has been devastated by the bizarre, giant humanoids known as the Titans. " +
                                         "Little is known about where they came from or why they are bent on consuming mankind. Seemingly unintelligent, " +
                                         "they have roamed the world for years, killing everyone they see. For the past century, what's left of man has hidden in " +
                                         "a giant, three-walled city. People believe their 50-meter-high walls will protect them from the Titans, but the sudden " +
                                         "appearance of an immense Titan is about to change everything.", Price = 12.99 },
                new Book { Id = 5, Name = "Tomie", Image = "Images/books/5.jpg",
                           Genre = _genres.Find(g => g.Name.Equals("Manga")), Author = "Junji Ito",
                           Description = "Tomie Kawakami is a femme fatale with long black hair and a beauty mark just under her left eye. She can seduce nearly " +
                                         "any man, and drive them to murder as well, even though the victim is often Tomie herself. While one lover seeks to keep " +
                                         "her for himself, another grows terrified of the immortal succubus. But soon they realize that no matter how many times " +
                                         "they kill her, the world will never be free of Tomie.", Price = 18.99 },
                new Book { Id = 6, Name = "All You Need Is Kill", Image = "Images/books/6.jpg",
                           Genre = _genres.Find(g => g.Name.Equals("Manga")), Author = "Ryosuke Takeuchi, Yoshitoshi Abe",
                           Description = "When the alien Mimics invade, Keiji Kiriya is just one of many recruits shoved into a suit of battle armor called a " +
                                         "Jacket and sent out to kill. Keiji dies on the battlefield, only to be reborn each morning to fight and die again " +
                                         "and again. On his 158th iteration, he gets a message from a mysterious ally—the female soldier known as " +
                                         "the Full Metal Bitch. Is she the key to Keiji's escape or his final death?", Price = 9.99 }
            };
        }

        public Task<ResponseData<ListModel<Book>>> GetBooksListAsync(string? genreName = "All", int pageNo = 1)
        {
            var result = new ResponseData<ListModel<Book>>();

            result.Data = new();
            List<Book> books = new();
            if (genreName == "All")
                books = _books;
            else
                books = _books.FindAll(b => b.Genre != null && b.Genre.Name.Equals(genreName));

            int totalBooks = 0;
            if (books != null) totalBooks = books.Count();

            result.Data.TotalPages = (int) Math.Ceiling((double) totalBooks / _itemsPerPage);
            result.Data.CurrentPage = pageNo;
            if (books != null)
                result.Data.Items = books.Skip((pageNo - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();

            return Task.FromResult(result);
        }

        public Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBookAsync(int id, Book book, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
