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
            public DbSet<BookTag> BooksTags { get; set; }
            public DbSet<City> Cities { get; set; }
            public DbSet<Comment> Comments { get; set; }
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
                  .OnDelete(DeleteBehavior.Cascade);
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
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Book>()
                    .HasOne(x => x.Series)
                    .WithMany(x => x.Books)
                    .HasForeignKey(x => x.SeriesId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book>()
                .HasOne(x => x.PublishingHouse)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.PublishingHouseId)
                .OnDelete(DeleteBehavior.Cascade);


                builder.Entity<Comment>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.MyComments)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<CurrentRead>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.CurrentReads)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<CurrentRead>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.CurrentReads)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Notes>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.Notes)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Notes>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.Notes)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Shelf>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.Shelves)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

               

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
                .HasKey(bt => new { bt.BookId, bt.TagId }); // Composite Key

                builder.Entity<BookTag>()
                    .HasOne(bt => bt.Book)
                    .WithMany(b => b.BookTags)
                    .HasForeignKey(bt => bt.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<BookTag>()
                    .HasOne(bt => bt.Tag)
                    .WithMany(t => t.BookTags)
                    .HasForeignKey(bt => bt.TagId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Comment>()
                        .HasKey(x => new { x.UserId, x.BookId });

                builder.Entity<Comment>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.MyComments)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
                builder.Entity<Comment>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.Comments)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.NoAction);


                builder.Entity<ShelfBook>()
                    .HasKey(x => new { x.ShelfId, x.BookId });

                builder.Entity<ShelfBook>()
                    .HasOne(x => x.Shelf)
                    .WithMany(x => x.ShelfBooks)
                    .HasForeignKey(x => x.ShelfId)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.Entity<ShelfBook>()
                    .HasOne(x => x.Book)
                    .WithMany(x => x.ShelfBooks)
                    .HasForeignKey(x => x.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);

            }
        }
    }
