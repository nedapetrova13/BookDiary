    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BookDiary.DataAccess.Configurations;
    using BookDiary.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    namespace BookDiary.DataAccess
    {
        public class ApplicationDbContext:IdentityDbContext<User>
        {
            private bool seedDb;

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, bool seedDb = true) : base(options)
            {
                /*if (this.Database.IsRelational())
                {
                    this.Database.Migrate();
                }
                else
                {
                    this.Database.EnsureCreated();
                }

                this.seedDb = seedDb;*/
            }
          
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
                  .HasForeignKey(k => k.AuthorId)
                  .OnDelete(DeleteBehavior.NoAction);
                builder.Entity<User>()
                    .HasOne(u => u.Book)
                    .WithMany(b => b.Users)
                    .HasForeignKey(u => u.FavouriteBookId)
                    .OnDelete(DeleteBehavior.SetNull);

                /*builder.Entity<Author>()
                    .HasOne(a => a.City)
                    .WithMany(x => x.Authors)
                    .HasForeignKey(x => x.CityId)
                    .OnDelete(DeleteBehavior.NoAction);*/

             /*   builder.Entity<User>()
                    .HasOne(x => x.City)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.CityId)
                    .OnDelete(DeleteBehavior.NoAction);*/

                builder.Entity<Book>()
                    .HasOne(x => x.Genre)
                    .WithMany(x => x.Books)
                    .HasForeignKey(x => x.GenreId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<Book>()
                    .HasOne(x => x.Series)
                    .WithMany(x => x.Books)
                    .HasForeignKey(x => x.SeriesId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<BookPublishingHouse>()
                    .HasOne(x => x.Language)
                    .WithMany(x => x.BookPublishingHouses)
                    .HasForeignKey(x => x.LanguageId)
                     .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<Comment>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.MyComments)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<CurrentRead>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.CurrentReads)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<CurrentRead>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.CurrentReads)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<Notes>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.Notes)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<Notes>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.Notes)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<Shelf>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.Shelves)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<BookPublishingHouse>()
                    .HasKey(x => new { x.PublishingHouseId, x.BookId });
                builder.Entity<BookPublishingHouse>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.BookPublishingHouse)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<BookPublishingHouse>()
                    .HasOne(x => x.PublishingHouse)
                    .WithMany(x => x.bookPublishingHouses)
                    .HasForeignKey(x => x.PublishingHouseId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<QuestionGenre>()
                    .HasKey(x => new { x.QuestionId, x.GenreId });
                builder.Entity<QuestionGenre>()
                    .HasOne(x => x.Genre)
                    .WithMany(x => x.QuestionGenres)
                    .HasForeignKey(x => x.GenreId)
                    .OnDelete(DeleteBehavior.NoAction);
                builder.Entity<QuestionGenre>()
                    .HasOne(x => x.Question)
                    .WithMany(x => x.QuestionGenres)
                    .HasForeignKey(x => x.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<QuestionGenreBook>()
                    .HasKey(qgb => new { qgb.BookId, qgb.QuestionId, qgb.GenreId });
                builder.Entity<QuestionGenreBook>()
                    .HasOne(qgb => qgb.QuestionGenre)
                    .WithMany(qg => qg.QuestionGenreBooks)
                    .HasForeignKey(qgb => new { qgb.QuestionId, qgb.GenreId })
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<QuestionGenreBook>()
                    .HasOne(qgb => qgb.Book)
                    .WithMany(b => b.QuestionGenreBooks)
                    .HasForeignKey(qgb => qgb.BookId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<QuestionGenreBook>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.QuestionGenreBooks)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<BookTag>()
                    .HasKey(x => new { x.TagId, x.BookId });
                builder.Entity<BookTag>()
                    .HasOne(x => x.Tag)
                    .WithMany(x => x.BookTags)
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.NoAction);
                builder.Entity<BookTag>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.BookTags)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<CommentBook>()
                    .HasKey(x => new { x.CommentId, x.BookId });

                builder.Entity<CommentBook>()
                    .HasOne(x => x.Comment)
                    .WithMany(x => x.CommentBooks)
                    .HasForeignKey(x => x.CommentId)
                    .OnDelete(DeleteBehavior.NoAction);
                builder.Entity<CommentBook>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.Comments)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<CommentNews>()
                    .HasKey(x => new { x.NewsId, x.CommentId });

                builder.Entity<CommentNews>()
                    .HasOne(x => x.News)
                    .WithMany(x => x.Comments)
                    .HasForeignKey(x => x.NewsId)
                    .OnDelete(DeleteBehavior.NoAction);
                builder.Entity<CommentNews>()
                    .HasOne(x => x.Comment)
                    .WithMany(x => x.CommentNews)
                    .HasForeignKey(x => x.CommentId)
                    .OnDelete(DeleteBehavior.NoAction);

              /*  builder.Entity<User>()
                    .HasOne(u => u.City)
                    .WithMany(c => c.Users)
                    .HasForeignKey(u => u.CityId)
                    .OnDelete(DeleteBehavior.SetNull);*/

                builder.Entity<ShelfBook>()
                    .HasKey(x => new { x.ShelfId, x.BookId });

                builder.Entity<ShelfBook>()
                    .HasOne(x => x.Shelf)
                    .WithMany(x => x.ShelfBooks)
                    .HasForeignKey(x => x.ShelfId)
                    .OnDelete(DeleteBehavior.NoAction);
                builder.Entity<ShelfBook>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.ShelfBooks)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.NoAction);
                if (seedDb)
                {
                    builder.ApplyConfiguration(new RoleConfiguration());
                    builder.ApplyConfiguration(new UserConfiguration());
                    builder.ApplyConfiguration(new UserRoleConfiguration());
                }


            }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);

            }
        }
    }
