using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.DataAccess;
using BookDiary.Models.Enums;
using BookDiary.Models;
using System.Net.WebSockets;

namespace BookDiary.Tests.UnitTests
{
    public class DbSeeder
    {
        public static void SeedDatabase(ApplicationDbContext context)
        {
            SeedUsers(context);
            SeedAuthors(context);
            SeedGenres(context);
            SeedTags(context);
            SeedLanguages(context);
            SeedNews(context);
            SeedPublishingHouse(context);
            SeedSeries(context);
            SeedShelf(context);
            SeedBooks(context);
            SeedShelfBooks(context);
            SeedBookTag(context);
            SeedComment(context);
            SeedCurrentRead(context);
            SeedNotes(context);

            context.SaveChanges();
        }
        public static void SeedUsers(ApplicationDbContext context)
        {
            var admin = new User()
            {
                Id = "501889c2-7883-473b-9333-c55267249071",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@abv.bg",
                NormalizedEmail = "ADMIN@ABV.BG",
                Name = "Админ",
                Gender = GenderEnum.Мъж.ToString(),
                Birthdate = DateTime.ParseExact("2005-05-20 11:20", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                ProfilePictureURL = null,

            };

            var user = new User()
            {
                Id = "bae65efa-6885-4144-9786-0719b0e2ebc4",
                UserName = "userTest",
                NormalizedUserName = "USERTEST",
                Email = "user@abv.bg",
                NormalizedEmail = "USER@ABV.BG",
                Name = "Юзър Юзър",
                Gender = GenderEnum.Мъж.ToString(),
                Birthdate = DateTime.ParseExact("2007-06-10 11:20", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                ProfilePictureURL = null
            };
            context.Users.Add(admin);
            context.Users.Add(user);
        }
        public static void SeedAuthors(ApplicationDbContext context)
        {
            var author1 = new Author()
            {
                Id = 123,
                Name = "Джордж Оруел",
                Bio = "Английски писател, известен с '1984' и 'Фермата на животните'.",
                WebSiteLink= "https://www.sample.edu/",
                BirthDate=DateTime.Now,
                Email="author1@mail.com",
                Gender = GenderEnum.Мъж.ToString(),
                ProfilePictureURL= "https://vondy-images.com/image-proxy?w=800&url=https://protoinfrastack-myfirstbucketb8884501-fnnzvxt2ee5v.s3.amazonaws.com/VkEEmDnM1kZVg8NJyDCpP6xPBC5IyzwGvmyn.png"

            };

            var author2 = new Author()
            {
                Id = 455,
                Name = "Дж.К. Роулинг",
                Bio = "Автор на поредицата 'Хари Потър'.",
                WebSiteLink = "https://www.sample.edu/",
                BirthDate = DateTime.Now,
                Email = "author2@mail.com",
                Gender = GenderEnum.Жена.ToString(),
                ProfilePictureURL = "https://vondy-images.com/image-proxy?w=800&url=https://protoinfrastack-myfirstbucketb8884501-fnnzvxt2ee5v.s3.amazonaws.com/VkEEmDnM1kZVg8NJyDCpP6xPBC5IyzwGvmyn.png"

            };

            context.Authors.Add(author1);
            context.Authors.Add(author2);
        }
        public static void SeedGenres(ApplicationDbContext context)
        {
            
            
                var genre1 = new Genre() { Id = 523, Name = "Фантастика" };
                var genre2 = new Genre() { Id = 324, Name = "Трилър" };

                context.Genres.Add(genre1);
                context.Genres.Add(genre2);
              
        }
        public static void SeedTags(ApplicationDbContext context)
        {
           
                var tag1 = new Tag() { Id = 85, Name = "Дистопия" };
                var tag2 = new Tag() { Id = 245, Name = "Магия" };

                context.Tags.Add(tag1);
                context.Tags.Add(tag2);
            
        }
        public static void SeedLanguages(ApplicationDbContext context)
        {
            var lang1 = new Language() { Id = 21, Name = "Български" };
            var lang2 = new Language() { Id = 22, Name = "Английски" };
            context.Languages.Add(lang1);
            context.Languages.Add(lang2);
        }
       
        public static void SeedNews(ApplicationDbContext context)
        {
            var news1 = new News()
            {
                Id = 1,
                Title = "News title1",
                Content = "News content1",
                Created = DateTime.Now,
            };
            var news2 = new News()
            {
                Id = 2,
                Title = "News title2",
                Content = "News content2",
                Created = DateTime.Now,
            };

            context.News.Add(news1);
            context.News.Add(news2);
        }
        
        public static void SeedPublishingHouse(ApplicationDbContext context)
        {
            var ph1 = new PublishingHouse()
            {
                Id = 1,
                Name = "Сиела",
                YearFounded = 1991
            };
            var ph2 = new PublishingHouse()
            {
                Id = 2,
                Name = "Хеликон",
                YearFounded = 2000
            };

            context.PublishingHouses.Add(ph1);
            context.PublishingHouses.Add(ph2);
        }
        public static void SeedSeries(ApplicationDbContext context)
        {
            var series1 = new Series()
            {
                Id = 1,
                Title = "Поредица1",
                Description = "Описание1"
            };
            var series2 = new Series()
            {
                Id = 2,
                Title = "Поредица2",
                Description = "Описание2"
            };

            context.Series.Add(series1);
            context.Series.Add(series2);
        }
        public static void SeedShelf(ApplicationDbContext context)
        {
            var shelf1 = new Shelf()
            {
                Id = 1,
                UserId = "bae65efa-6885-4144-9786-0719b0e2ebc4",
                Name = "Shelf1",
                Description = "Description1"
            };
            var shelf2 = new Shelf()
            {
                Id = 2,
                UserId = "bae65efa-6885-4144-9786-0719b0e2ebc4",
                Name = "Shelf2",
                Description = "Description2"
            };

            context.Shelves.Add(shelf1);
            context.Shelves.Add(shelf2);
        }
       
        public static void SeedBooks(ApplicationDbContext context)
        {
            
                var book1 = new Book()
                {
                    Id = 219,
                    Title = "1984",
                    Description = "Дистопичен роман за тоталитаризма.",
                    AuthorId = context.Authors.FirstOrDefault(a => a.Name == "Джордж Оруел").Id,
                    GenreId = context.Genres.FirstOrDefault(g => g.Name == "Фантастика").Id,
                    SeriesId=1,
                    CoverImageURL= "https://www.designforwriters.com/wp-content/uploads/2017/10/design-for-writers-book-cover-tf-2-a-million-to-one.jpg",
                    PublishingHouseId=1,
                    LanguageId=21,
                    BookPages=350,
                    Chapters=50
                };

                var book2 = new Book()
                {
                    Id = 862,
                    Title = "Хари Потър и Философският камък",
                    Description = "Първата книга от поредицата 'Хари Потър'.",
                    AuthorId = context.Authors.FirstOrDefault(a => a.Name == "Дж.К. Роулинг").Id,
                    GenreId = context.Genres.FirstOrDefault(g => g.Name == "Фантастика").Id,
                    SeriesId = 2,
                    CoverImageURL = "https://www.designforwriters.com/wp-content/uploads/2017/10/design-for-writers-book-cover-tf-2-a-million-to-one.jpg",
                    PublishingHouseId = 2,
                    LanguageId = 22,
                    BookPages = 450,
                    Chapters = 53
                };

                context.Books.Add(book1);
                context.Books.Add(book2);
            
        }
        public static void SeedShelfBooks(ApplicationDbContext context)
        {
            var sb1 = new ShelfBook()
            {
                Id = 1,
                BookId = 219,
                ShelfId = 1
            };
            var sb2 = new ShelfBook()
            {
                Id = 2,
                BookId = 219,
                ShelfId = 2
            };

            context.ShelvesBooks.Add(sb1);
            context.ShelvesBooks.Add(sb2);
        }
        public static void SeedBookTag(ApplicationDbContext context)
        {
            var bt1 = new BookTag() { Id = 686, BookId = 219, TagId = 85 };
            var bt2 = new BookTag() { Id = 7657, BookId = 862, TagId = 245 };
            var bt3 = new BookTag() { Id = 686, BookId = 219, TagId = 245 };

            context.BooksTags.Add(bt1);
            context.BooksTags.Add(bt3);
            context.BooksTags.Add(bt2);
        }
        public static void SeedComment(ApplicationDbContext context)
        {
            var comment1 = new Comment()
            {
                Id = 578,
                UserId = "bae65efa-6885-4144-9786-0719b0e2ebc4",
                BookId = 219,
                Content = "some comment",
                Rating = 2,
            };
            var comment2 = new Comment()
            {
                Id = 58,
                UserId = "bae65efa-6885-4144-9786-0719b0e2ebc4",
                BookId = 862,
                Content = "another comment",
                Rating = 3
            };

            context.Comments.Add(comment1);
            context.Comments.Add(comment2);
        }
        public static void SeedCurrentRead(ApplicationDbContext context)
        {
            var cr1 = new CurrentRead()
            {
                Id = 1,
                BookId = 219,
                UserId = "bae65efa-6885-4144-9786-0719b0e2ebc4",
                CurrentPage = 120
            };
            var cr2 = new CurrentRead()
            {
                Id = 2,
                BookId = 862,
                UserId = "bae65efa-6885-4144-9786-0719b0e2ebc4",
                CurrentPage = 100
            };

            context.CurrentReads.Add(cr1);
            context.CurrentReads.Add(cr2);
        }
        public static void SeedNotes(ApplicationDbContext context)
        {
            var notes1 = new Notes()
            {
                Id = 1,
                UserId = "bae65efa-6885-4144-9786-0719b0e2ebc4",
                BookId = 862,
                Title = "Notes title1",
                NoteContent = "Note content1",
                BookChapter = 12
            };
            var notes2 = new Notes()
            {
                Id = 2,
                UserId = "bae65efa-6885-4144-9786-0719b0e2ebc4",
                BookId = 219,
                Title = "Notes title2",
                NoteContent = "Note content2",
                BookChapter = 20
            };

            context.Notes.Add(notes1);
            context.Notes.Add(notes2);
        }
    }
}
