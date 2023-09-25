using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Web_153505_Bybko.Data;
using Web_153505_Bybko.Services.AuthorService;
using Web_153505_Bybko.Services.BookService;
using Web_153505_Bybko.Services.GenreService;

// creation of application
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString != null)
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IGenreService, ApiGenreService>();
builder.Services.AddScoped<IBookService, ApiBookService>();

var uriData = builder.Configuration["UriData:ApiUri"];
builder.Services.AddHttpClient<IGenreService, ApiGenreService>(options =>
        options.BaseAddress = new Uri(uriData!));
builder.Services.AddHttpClient<IBookService, ApiBookService>(options =>
        options.BaseAddress = new Uri(uriData!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// configuring route processing
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();
