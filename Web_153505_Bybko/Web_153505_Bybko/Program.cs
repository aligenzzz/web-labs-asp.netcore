using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_153505_Bybko.Data;
using Web_153505_Bybko.Services.GenreService;
using Web_153505_Bybko.Services.BookService;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

// creation of application
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// if (connectionString != null)
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//        options.UseSqlServer(connectionString));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IGenreService, ApiGenreService>();
builder.Services.AddScoped<IBookService, ApiBookService>();

var uriData = builder.Configuration["UriData:ApiUri"];
builder.Services.AddHttpClient<IGenreService, ApiGenreService>(options =>
        options.BaseAddress = new Uri(uriData!));
builder.Services.AddHttpClient<IBookService, ApiBookService>(options =>
        options.BaseAddress = new Uri(uriData!));

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie")
.AddJwtBearer(opt =>
{
    opt.Authority =
            builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
    opt.TokenValidationParameters.ValidateAudience = false;
    opt.TokenValidationParameters.ValidTypes =
                                    new[] { "at+jwt" };
    opt.TokenValidationParameters.RoleClaimType = "role";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
    options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
    options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];

    // get user's Claims
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ResponseType = "code";
    options.ResponseMode = "query";
    options.SaveTokens = true;
});

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

app.MapRazorPages().RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
