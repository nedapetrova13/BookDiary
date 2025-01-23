using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace BookDiary.DataAccess
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
       
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookPublishingHouse> BooksPublishingHouses {  get; set; }
        public DbSet<BookTag> BooksTags { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentBook> CommentsBooks { get; set; }
        public DbSet<CommentNews> CommentsNews { get; set; }    
        public DbSet<CurrentRead> CurrentReads { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<PublishingHouse> PublishingHouses { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionGenre> QuestionsGenres { get; set; }
        public DbSet<QuestionGenreBook> QuestionsGenresBooks { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<ShelfBook> ShelvesBooks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
      
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .HasOne(a => a.Author)
                .WithMany(b => b.Books)
                .HasForeignKey(k => k.AuthorId);
            builder.Entity<Author>()
                .HasOne(a=>a.City)
                .WithMany(x=>x.Authors)
                .HasForeignKey(x=>x.CityId);
            builder.Entity<User>()
                .HasOne(x => x.City)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.CityId);
            builder.Entity<Book>()
                .HasOne(x => x.Genre)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.GenreId);
            builder.Entity<Book>()
                .HasOne(x => x.Series)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.SeriesId);
            builder.Entity<BookPublishingHouse>()
                .HasOne(x => x.Language)
                .WithMany(x => x.BookPublishingHouses)
                .HasForeignKey(x => x.LanguageId);
            builder.Entity<Comment>()
                .HasOne(x => x.User)
                .WithMany(x => x.MyComments)
                .HasForeignKey(x => x.UserId);
            builder.Entity<CurrentRead>()
                .HasOne(x=>x.Book)
                .WithMany(x=>x.CurrentReads)
                .HasForeignKey(x=>x.BookId);
            builder.Entity<CurrentRead>()
                .HasOne(x => x.User)
                .WithMany(x => x.CurrentReads)
                .HasForeignKey(x => x.UserId);
            builder.Entity<Notes>()
                .HasOne(x => x.Book)
                .WithMany(x => x.Notes)
                .HasForeignKey(x => x.BookId);
            builder.Entity<Notes>()
                .HasOne(x=>x.User)
                .WithMany(x => x.Notes)
                .HasForeignKey(x => x.UserId);
            builder.Entity<Shelf>()
                .HasOne(x=>x.User)
                .WithMany(x=>x.Shelves)
                .HasForeignKey(x=>x.UserId);
            builder.Entity<BookPublishingHouse>()
                .HasOne(x => x.Book)
                .WithMany(x => x.BookPublishingHouse)
                .HasForeignKey(x => x.BookId);
            builder.Entity<BookPublishingHouse>()
                .HasOne(x => x.PublishingHouse)
                .WithMany(x => x.bookPublishingHouses)
                .HasForeignKey(x => x.PublishingHouseId);
            builder.Entity<QuestionGenre>()
                .HasOne(x => x.Genre)
                .WithMany(x => x.QuestionGenres)
                .HasForeignKey(x => x.GenreId);
            builder.Entity<QuestionGenre>()
                .HasOne(x => x.Question)
                .WithMany(x => x.QuestionGenres)
                .HasForeignKey(x => x.QuestionId);
            builder.Entity<QuestionGenreBook>()
                .HasOne(x => x.Book)
                .WithMany(x => x.QuestionGenreBooks)
                .HasForeignKey(x => x.BookId);
            builder.Entity<QuestionGenreBook>()
                .HasOne(x=>x.User)
                .WithMany(x=>x.QuestionGenreBooks)
                .HasForeignKey(x=>x.UserId);
            builder.Entity<BookTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.BookTags)
                .HasForeignKey(x => x.TagId);
            builder.Entity<BookTag>()
                .HasOne(x => x.Book)
                .WithMany(x => x.BookTags)
                .HasForeignKey(x => x.BookId);
            builder.Entity<CommentBook>()
                .HasOne(x => x.Comment)
                .WithMany(x => x.CommentBooks)
                .HasForeignKey(x => x.CommentId);
            builder.Entity<CommentBook>()
                .HasOne(x => x.Book)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.BookId);
            builder.Entity<CommentNews>()
                .HasOne(x => x.News)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.NewsId);
        }
    }
}
