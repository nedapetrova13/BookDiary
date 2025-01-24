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
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("BookDiary.DataAccess")));

builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
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
