using BookDiary.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BookDiary.DataAccess.Repository;
using BookDiary.Core;
using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using BookDiary.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//var connection = builder.Configuration.GetConnectionString("ArsenalConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("BookDiary.DataAccess")));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddDefaultIdentity<User>(options =>
{

    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddRazorPages();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookPublishingHouseService, BookPublishingHouseService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookTagService, BookTagService>(); 
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICommentBookService, CommentBookService>();
builder.Services.AddScoped<ICommentNewsService, CommentNewsService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICurrentReadService, CurrentReadService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<IPublishingHouseService, PublishingHouseService>();
builder.Services.AddScoped<IQuestionGenreBookService, QuestionGenreBookService>();
builder.Services.AddScoped<IQuestionGenreService, QuestionGenreService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ISeriesService, SeriesService>();
builder.Services.AddScoped<IShelfBookService, ShelfBookService>();
builder.Services.AddScoped<IShelfService, ShelfService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    // Ensure Roles Exist
    var roles = new List<string> { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    
    string adminEmail = "admin@abv.bg"; //do not remind of the fact that abv.bg exists!
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var user = new User
        {
            UserName = "Admin",
            Name = "Admin",
            Birthdate = DateTime.Now,
            Email = adminEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, "admin123");
        await userManager.AddToRoleAsync(user, "Admin");
    }
    string userEmail = "neda@abv.bg"; 
    var userUser = await userManager.FindByEmailAsync(userEmail);
    if (userUser == null)
    {
        var user1 = new User
        {
            UserName = "Neda",
            Name = "Neda",
            Birthdate = DateTime.Now,
            Email = userEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user1, "123456");
        await userManager.AddToRoleAsync(user1, "User");
    }
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    // Взимаме контекста на базата данни

    if (!dbContext.Authors.Any()) // Проверяваме дали вече има автори в базата
    {
        var authors = new List<Author>
        {
            new Author
            {
                Name = "Дж. К. Роулинг",
                BirthDate = new DateTime(1965, 7, 31),
                Email = "jkrowling@example.com",
                ProfilePictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRiFl56V2gTsWE_CssKfBRg7Yl5v8_aiuvKpg&s",
                Gender = "Жена",
                Bio = "Британска писателка, най-известна с поредицата за Хари Потър.",
                WebSiteLink = "https://www.jkrowling.com"
            },
            new Author
            {
                Name = "Джордж Р. Р. Мартин",
                BirthDate = new DateTime(1948, 9, 20),
                Email = "grrm@example.com",
                ProfilePictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTm1E9wP3UG35CREvu_2e8Xlp3Jxi85ry29jg&s",
                Gender = "Мъж",
                Bio = "Американски писател и сценарист, известен с поредицата „Песен за огън и лед“.",
                WebSiteLink = "https://georgerrmartin.com"
            },
            new Author
            {
                Name = "Агата Кристи",
                BirthDate = new DateTime(1890, 9, 15),
                Email = "agatha@example.com",
                ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/c/cf/Agatha_Christie.png",
                Gender = "Жена",
                Bio = "Английска писателка, известна с криминалните си романи за Еркюл Поаро.",
                WebSiteLink = "https://www.agathachristie.com"
            },
            new Author
            {
                Name = "Стивън Кинг",
                BirthDate = new DateTime(1947, 9, 21),
                Email = "sking@example.com",
                ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Stephen_King_at_the_2024_Toronto_International_Film_Festival_2_%28cropped%29.jpg/1200px-Stephen_King_at_the_2024_Toronto_International_Film_Festival_2_%28cropped%29.jpg",
                Gender = "Мъж",
                Bio = "Американски писател на ужаси, фантастика и трилъри.",
                WebSiteLink = "https://stephenking.com/index.html"
            },
           
        };

        dbContext.Authors.AddRange(authors);
        await dbContext.SaveChangesAsync();
    }
    if (!dbContext.Tags.Any()) 
    {
        var tags = new List<Tag>
        {
            new Tag { Name = "Отворен край" },
            new Tag { Name = "Бавен романс" },
            new Tag { Name = "Приятели към любовници" },
            new Tag { Name = "Втори шанс" },
            new Tag { Name = "Антигерой" },
            new Tag { Name = "Забранена любов" },
            new Tag { Name = "Предателство и отмъщение" },
            new Tag { Name = "Неочакван обрат" },
            new Tag { Name = "Любовен триъгълник" },
            new Tag { Name = "Фалшива връзка" }
        };

        dbContext.Tags.AddRange(tags); 
        await dbContext.SaveChangesAsync();
    }
    if (!dbContext.Series.Any()) // Проверяваме дали вече има поредици
    {
        var seriesList = new List<Series>
        {
            new Series
            {
                Title = "Хари Потър",
                Description = "Фентъзи поредица за младия магьосник Хари Потър и неговите приключения в Хогуортс."
            },
            new Series
            {
                Title = "Властелинът на пръстените",
                Description = "Епично приключение в Средната земя, където малък хобит трябва да унищожи Пръстена на Всевластието."
            },
            new Series
            {
                Title = "Песен за огън и лед",
                Description = "Фентъзи поредица, изпълнена с интриги, битки и дракони, разказваща за борбата за Железния трон."
            },
            new Series
            {
                Title = "Дневниците на вампира",
                Description = "Свръхестествена романтична поредица за вампири, върколаци и сложни любовни триъгълници."
            },
            new Series
            {
                Title = "Детектив Пуаро",
                Description = "Класически криминални мистерии с гениалния белгийски детектив Еркюл Пуаро."
            }
        };

        dbContext.Series.AddRange(seriesList); 
        await dbContext.SaveChangesAsync();
    }
    if (!dbContext.PublishingHouses.Any())
    {
        var publishingHouses = new List<PublishingHouse>
        {
            new PublishingHouse
            {
                Name = "Сиела",
                YearFounded = 1991
            },
            new PublishingHouse
            {
                Name = "Хермес",
                YearFounded = 1991
            },
            new PublishingHouse
            {
                Name = "Изток-Запад",
                YearFounded = 2002
            },
            new PublishingHouse
            {
                Name = "Егмонт",
                YearFounded = 1991
            },
            new PublishingHouse
            {
                Name = "Бард",
                YearFounded = 1992
            }
        };

        dbContext.PublishingHouses.AddRange(publishingHouses); 
        await dbContext.SaveChangesAsync(); 
    }
    if (!dbContext.News.Any()) 
    {
        var newsList = new List<News>
        {
            new News
            {
                Title = "Нова книга от Дж. К. Роулинг!",
                Content = "Авторката на Хари Потър обяви нов роман, който ще излезе през следващата година."
            },
            new News
            {
                Title = "Бестселър на годината",
                Content = "Романът 'Кръстникът' беше обявен за най-продаваната книга за изминалата година."
            },
            new News
            {
                Title = "Фестивал на книгата в София",
                Content = "Очаквайте страхотни намаления и срещи с автори на предстоящия книжен фестивал в НДК!"
            },
            new News
            {
                Title = "Екранизация на 'Властелинът на пръстените'",
                Content = "Голямо студио обяви нова поредица, базирана на хитовите книги на Толкин."
            },
            new News
            {
                Title = "Издателство 'Бард' обяви нова колекция",
                Content = "Феновете на фентъзи жанра ще бъдат изненадани с ново луксозно издание на любимите си книги."
            }
        };

        dbContext.News.AddRange(newsList);
        await dbContext.SaveChangesAsync();
    }
    if (!dbContext.Languages.Any()) // Проверяваме дали вече има езици
    {
        var languages = new List<Language>
        {
            new Language { Name = "Български" },
            new Language { Name = "Английски" },
            new Language { Name = "Френски" },
            new Language { Name = "Немски" },
            new Language { Name = "Испански" },
            new Language { Name = "Италиански" },
            new Language { Name = "Руски" },
            new Language { Name = "Японски" },
            new Language { Name = "Китайски" },
            new Language { Name = "Арабски" }
        };

        dbContext.Languages.AddRange(languages); // Добавяме езиците в базата
        await dbContext.SaveChangesAsync(); // Запазваме промените
    }
    if (!dbContext.Genres.Any()) 
    {  
        
        var genres = new List<Genre>
        {
            new Genre { Name = "Фентъзи" },
            new Genre { Name = "Научна фантастика" },
            new Genre { Name = "Романтика" },
            new Genre { Name = "Трилър" },
            new Genre { Name = "Ужаси" },
            new Genre { Name = "Криминален" },
            new Genre { Name = "Исторически роман" },
            new Genre { Name = "Дистопия" },
            new Genre { Name = "Комедия" },
            new Genre { Name = "Драма" },
            new Genre { Name = "Приключенски" },
            new Genre { Name = "Поезия" }
        };

        dbContext.Genres.AddRange(genres); 
        await dbContext.SaveChangesAsync(); 
    }
    

    if (!dbContext.Books.Any()) 
    {
        var books = new List<Book>
        {
            new Book
            {
                Title = "Хари Потър и Философският камък",
                AuthorId = dbContext.Authors.First(a => a.Name == "Дж. К. Роулинг").Id,
                GenreId = dbContext.Genres.First(g => g.Name == "Фентъзи").Id,
                BookPages = 320,
                Description = "Представете си, че сте прекарали първите 10 години от своя живот без родители, живеейки под стълбите в дома на семейство, което ви мрази. И на единайсетия си рожден ден откривате, че сте истински магьосник!\r\n\r\nТочно това се случва с Хари Потър в този пленяващ забавен роман.",
                Chapters = 17,
                CoverImageURL = "https://www.ciela.com/media/catalog/product/cache/9a7ceae8a5abbd0253425b80f9ef99a5/h/a/hari_potar_i_filosofskiat_kamak_hrm.jpg"
            },
           new Book
            {
                Title = "Хари Потър и Стаята на тайните",
                AuthorId = dbContext.Authors.First(a => a.Name == "Дж. К. Роулинг").Id,
                GenreId = dbContext.Genres.First(g => g.Name == "Фентъзи").Id,
                BookPages = 360,
                Description = "След като успешно преживява първата си година в Хогуортс, Хари Потър се завръща за още приключения. Но странни събития започват да се случват, когато древна магия се пробужда в Стаята на тайните.",
                Chapters = 18,
                CoverImageURL = "https://www.ciela.com/media/catalog/product/cache/9a7ceae8a5abbd0253425b80f9ef99a5/h/a/hari_potar_i_staiata_na_tainite_hrm.jpg"
            },

            new Book
            {
                Title = "Хари Потър и Затворникът от Азкабан",
                AuthorId = dbContext.Authors.First(a => a.Name == "Дж. К. Роулинг").Id,
                GenreId = dbContext.Genres.First(g => g.Name == "Фентъзи").Id,
                BookPages = 435,
                Description = "Сириус Блек, страховит магьосник, е избягал от затвора Азкабан и всички вярват, че е по следите на Хари Потър. Но дали истината е такава, каквато изглежда?",
                Chapters = 22,
                CoverImageURL = "https://egmontbulgaria.com/media/cache/product_original/product-images/29/26/97895444655751469431553.jpg?1469431553"
            },

            new Book
            {
                Title = "Игра на тронове",
                AuthorId = dbContext.Authors.First(a => a.Name == "Джордж Р. Р. Мартин").Id,
                GenreId = dbContext.Genres.First(g => g.Name == "Фентъзи").Id,
                BookPages = 835,
                Description = "Първата книга от епичната фентъзи сага „Песен за огън и лед“. В нея се разказва за борбата за власт между благородническите фамилии на Вестерос, където всеки ход може да е последен.",
                Chapters = 73,
                CoverImageURL = "https://www.book.store.bg/lrgimg/17366/pesen-za-ogyn-i-led-kniga-1-igra-na-tronove.jpg"
            },

            new Book
            {
                Title = "Сблъсък на крале",
                AuthorId = dbContext.Authors.First(a => a.Name == "Джордж Р. Р. Мартин").Id,
                GenreId = dbContext.Genres.First(g => g.Name == "Фентъзи").Id,
                BookPages = 969,
                Description = "Докато войната бушува из Вестерос, различни претенденти за трона започват да се борят за властта. Древни сили започват да се пробуждат, а северът е изправен пред нова заплаха.",
                Chapters = 69,
                CoverImageURL = "https://www.ateabooks.com/5822/-2-sblusuk-na-krale.jpg"
            },

            new Book
            {
                Title = "Вихър от мечове",
                AuthorId = dbContext.Authors.First(a => a.Name == "Джордж Р. Р. Мартин").Id,
                GenreId = dbContext.Genres.First(g => g.Name == "Фентъзи").Id,
                BookPages = 1128,
                Description = "Третата част от поредицата „Песен за огън и лед“, където борбата за власт става още по-кървава, а съдбите на любимите герои се преплитат в неочаквани обрати.",
                Chapters = 82,
                CoverImageURL = "https://www.bard.bg/files/mf/books/876_pic_1.jpg"
            },

            new Book
            {
                Title = "The Shining",
                AuthorId = dbContext.Authors.First(a => a.Name == "Стивън Кинг").Id,
                GenreId = dbContext.Genres.First(g => g.Name == "Ужаси").Id,
                BookPages = 447,
                Description = "Jack Torrance takes a job as the winter caretaker at the Overlook Hotel, but sinister forces begin to influence him.",
                Chapters = 58,
                CoverImageURL = "https://libris.to/media/jacket/02933315_shining.jpg"
            },
            new Book
            {
                Title = "Carrie",
                AuthorId = dbContext.Authors.First(a => a.Name == "Стивън Кинг").Id,
                GenreId = dbContext.Genres.First(g => g.Name == "Ужаси").Id,
                BookPages = 199,
                Description = "A bullied teenage girl discovers her telekinetic abilities and uses them to unleash a night of horror.",
                Chapters = 17,
                CoverImageURL = "https://th.bing.com/th/id/R.2c05123acc614da649f8be9a2ca4d8fb?rik=YteGOBdAvgOfcQ&riu=http%3a%2f%2fprodimage.images-bn.com%2fpimages%2f9780307743664_p0_v1_s1200x630.jpg&ehk=nJYIcUr2L%2fE0HjoqDWBESaX7Hh8FOQtF6bXZsNJaXNs%3d&risl=&pid=ImgRaw&r=0"
            },
            new Book
            {
                Title = "Murder on the Orient Express",
                AuthorId = dbContext.Authors.First(a => a.Name == "Агата Кристи").Id,
                GenreId = dbContext.Genres.First(g => g.Name == "Криминален").Id,
                BookPages = 256,
                Description = "Hercule Poirot investigates a murder aboard the luxurious Orient Express train.",
                Chapters = 30,
                CoverImageURL = "https://www.pluggedin.com/wp-content/uploads/2020/01/murder-on-the-orient-express-cover.jpg"
            }


        };

        dbContext.Books.AddRange(books); 
        await dbContext.SaveChangesAsync();
    }
    if (!dbContext.BooksPublishingHouses.Any())
    {
        var bph = new List<BookPublishingHouse>
        {
            new BookPublishingHouse
            {
                BookId=dbContext.Books.First(b=>b.Title=="Хари Потър и Философският камък").Id,
                PublishingHouseId=dbContext.PublishingHouses.First(ph=>ph.Name=="Егмонт").Id,
                LanguageId=dbContext.Languages.First(l=>l.Name=="Български").Id
            },
            new BookPublishingHouse
            {
                BookId=dbContext.Books.First(b=>b.Title=="Хари Потър и Стаята на тайните").Id,
                PublishingHouseId=dbContext.PublishingHouses.First(ph=>ph.Name=="Егмонт").Id,
                LanguageId=dbContext.Languages.First(l=>l.Name=="Български").Id
            },
            new BookPublishingHouse
            {
                BookId=dbContext.Books.First(b=>b.Title=="Хари Потър и Затворникът от Азкабан").Id,
                PublishingHouseId=dbContext.PublishingHouses.First(ph=>ph.Name=="Егмонт").Id,
                LanguageId=dbContext.Languages.First(l=>l.Name=="Български").Id
            },
             new BookPublishingHouse
            {
                BookId=dbContext.Books.First(b=>b.Title=="Игра на тронове").Id,
                PublishingHouseId=dbContext.PublishingHouses.First(ph=>ph.Name=="Бард").Id,
                LanguageId=dbContext.Languages.First(l=>l.Name=="Български").Id
            },
              new BookPublishingHouse
            {
                BookId=dbContext.Books.First(b=>b.Title=="Сблъсък на крале").Id,
                PublishingHouseId=dbContext.PublishingHouses.First(ph=>ph.Name=="Бард").Id,
                LanguageId=dbContext.Languages.First(l=>l.Name=="Български").Id
            },
               new BookPublishingHouse
            {
                BookId=dbContext.Books.First(b=>b.Title=="Вихър от мечове").Id,
                PublishingHouseId=dbContext.PublishingHouses.First(ph=>ph.Name=="Бард").Id,
                LanguageId=dbContext.Languages.First(l=>l.Name=="Български").Id
            },
             new BookPublishingHouse
            {
                BookId = dbContext.Books.First(b => b.Title == "The Shining").Id,
                PublishingHouseId = dbContext.PublishingHouses.First(ph => ph.Name == "Бард").Id,
                LanguageId = dbContext.Languages.First(l => l.Name == "Английски").Id
            },
            new BookPublishingHouse
            {
                BookId = dbContext.Books.First(b => b.Title == "Carrie").Id,
                PublishingHouseId = dbContext.PublishingHouses.First(ph => ph.Name == "Бард").Id,
                LanguageId = dbContext.Languages.First(l => l.Name == "Английски").Id
            },
            new BookPublishingHouse
            {
                BookId = dbContext.Books.First(b => b.Title == "Murder on the Orient Express").Id,
                PublishingHouseId = dbContext.PublishingHouses.First(ph => ph.Name == "Изток-Запад").Id,
                LanguageId = dbContext.Languages.First(l => l.Name == "Английски").Id
            }
        };
        dbContext.BooksPublishingHouses.AddRange(bph);
        await dbContext.SaveChangesAsync();
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error has occurred while applying migrations.");
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
