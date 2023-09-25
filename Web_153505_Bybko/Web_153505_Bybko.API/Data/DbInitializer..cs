using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using Web_153505_Bybko.Domain.Entities;

namespace Web_153505_Bybko.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
            await context.Database.MigrateAsync();

            var configuration = app.Configuration;
            var apiUrl = configuration["AppSettings:ApiUrl"];

            await ClearDataAsync(context);

            var genres = new List<Genre>()
            {
                new Genre { Id = 1, Name = "Detective", Description = "Detective story, type of popular literature in which a crime " +
                                                        "is introduced and investigated and the culprit is revealed. Detective stories " +
                                                        "frequently operate on the principle that superficially convincing evidence is " +
                                                        "ultimately irrelevant." },
                new Genre { Id = 2, Name = "Manga", Description = "" },
            };
            await context.AddRangeAsync(genres);
            await context.SaveChangesAsync();

            if (apiUrl == null) return;

            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Sherlock Holmes: The Hound of the Baskervilles", Image = $"{apiUrl}/Images/books/1.jpg",
                           GenreId = 1, Genre = context.Genres.FirstOrDefault(g => g.Name.Equals("Detective")), Author = "Sir Arthur Conan Doyle",
                           Description = "With a deadly phantom hound on the loose and a mysterious man living on the moors, " +
                                         "Devon is a dangerous place to be. But Holmes and Watson must put their fears aside. " +
                                         "The country’s favourite crime-fighting duo need to unravel the strange case of Sir Charles " +
                                         "Baskerville’s murder before his nephew meets the same fate.", Price = 7.99 },
                new Book { Id = 2, Name = "And Then There Were None", Image = $"{apiUrl}/Images/books/2.jpg",
                           GenreId = 1, Genre = context.Genres.FirstOrDefault(g => g.Name.Equals("Detective")), Author = "Agatha Christie",
                           Description = "Ten people, each with something to hide and something to fear, are invited to an isolated mansion " +
                                         "on Indian Island by a host who, surprisingly, fails to appear. On the island they are cut off from " +
                                         "everything but each other and the inescapable shadows of their own past lives. One by one, the guests " +
                                         "share the darkest secrets of their wicked pasts. And one by one, they die…", Price = 9.99 },
                new Book { Id = 3, Name = "A COFFIN FROM HONG KONG", Image = $"{apiUrl}/Images/books/3.jpg",
                           GenreId = 1, Genre = context.Genres.FirstOrDefault(g => g.Name.Equals("Detective")), Author = "James Hadley Chase",
                           Description = "A mysterious voice on the telephone, a beautiful Chinese girl brutally murdered, a doddering millionaire with " +
                                         "a guilty conscience, a private detective ensnared by unfathomable crimes, a Kowloon hooker who talks too much for " +
                                         "her own good . . . and a coffin from Hong Kong . . .These are the barbs that pull P.I. Ryan Nelson into the " +
                                         "strangest case of his career. To solve the mystery, he must leave the postcard-pretty beaches of his California " +
                                         "home and burrow into the sinister alleys of the ancient walled city of Kowloon.", Price = 3.99 },

                new Book { Id = 4, Name = "Attack on Titan 1", Image = $"{apiUrl}/Images/books/4.jpg",
                           GenreId = 2, Genre = context.Genres.FirstOrDefault(g => g.Name.Equals("Manga")), Author = "Hajime Isayama",
                           Description = "In this post-apocalytpic sci-fi story, humanity has been devastated by the bizarre, giant humanoids known as the Titans. " +
                                         "Little is known about where they came from or why they are bent on consuming mankind. Seemingly unintelligent, " +
                                         "they have roamed the world for years, killing everyone they see. For the past century, what's left of man has hidden in " +
                                         "a giant, three-walled city. People believe their 50-meter-high walls will protect them from the Titans, but the sudden " +
                                         "appearance of an immense Titan is about to change everything.", Price = 12.99 },
                new Book { Id = 5, Name = "Tomie", Image = $"{apiUrl}/Images/books/5.jpg",
                           GenreId = 2, Genre = context.Genres.FirstOrDefault(g => g.Name.Equals("Manga")), Author = "Junji Ito",
                           Description = "Tomie Kawakami is a femme fatale with long black hair and a beauty mark just under her left eye. She can seduce nearly " +
                                         "any man, and drive them to murder as well, even though the victim is often Tomie herself. While one lover seeks to keep " +
                                         "her for himself, another grows terrified of the immortal succubus. But soon they realize that no matter how many times " +
                                         "they kill her, the world will never be free of Tomie.", Price = 18.99 },
                new Book { Id = 6, Name = "All You Need Is Kill", Image = $"{apiUrl}/Images/books/6.jpg",
                           GenreId = 2, Genre = context.Genres.FirstOrDefault(g => g.Name.Equals("Manga")), Author = "Ryosuke Takeuchi, Yoshitoshi Abe",
                           Description = "When the alien Mimics invade, Keiji Kiriya is just one of many recruits shoved into a suit of battle armor called a " +
                                         "Jacket and sent out to kill. Keiji dies on the battlefield, only to be reborn each morning to fight and die again " +
                                         "and again. On his 158th iteration, he gets a message from a mysterious ally—the female soldier known as " +
                                         "the Full Metal Bitch. Is she the key to Keiji's escape or his final death?", Price = 9.99 }
            };
            await context.AddRangeAsync(books);
            await context.SaveChangesAsync();
        }

        public static async Task ClearDataAsync(AppDbContext context)
        {
            var booksToDelete = context.Books.ToList();
            var genresToDelete = context.Genres.ToList();

            context.Books.RemoveRange(booksToDelete);
            context.Genres.RemoveRange(genresToDelete);

            await context.SaveChangesAsync();
        }
    }
}
