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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
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

    // Ensure Admin User Exists
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
    string userEmail = "neda@abv.bg"; //do not remind of the fact that abv.bg exists!
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
