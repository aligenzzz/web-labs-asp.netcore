using Web_153505_Bybko.Services.GenreService;
using Web_153505_Bybko.Services.BookService;
using System.IdentityModel.Tokens.Jwt;
using Web_153505_Bybko.Domain.Models;
using Web_153505_Bybko.Models;
using Web_153505_Bybko.Middleware;

// creation of application
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IGenreService, ApiGenreService>();
builder.Services.AddScoped<IBookService, ApiBookService>();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

var uriData = builder.Configuration["UriData:ApiUri"];
builder.Services.AddHttpClient<IGenreService, ApiGenreService>(options =>
        options.BaseAddress = new Uri(uriData!));
builder.Services.AddHttpClient<IBookService, ApiBookService>(options =>
        options.BaseAddress = new Uri(uriData!));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie")
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

// my middleware
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages().RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
