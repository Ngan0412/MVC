using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Models;
using MVC.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MVCContext") ?? throw new InvalidOperationException("Connection string 'MVCContext' not found.")));
// 2. ??ng k� ASP.NET Core Identity (S? d?ng IdentityUser v� IdentityRole)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<MVCContext>() // ch? ??nh n?i l?u tr? d? li?u Identity (s? d?ng DbContext ?� ??ng k�)
.AddDefaultTokenProviders() // b?t t�nh n?ng x�c th?c OTP, email.
.AddClaimsPrincipalFactory<CustomClaimsFactory>(); // thay ??i c�ch ?�ng g�i Claims v�o Identity tr??c khi n�n v�o Cookie
// 3. C?u h�nh Cookie Authentication
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";         // trang chuy?n h??ng khi ch?a ??ng nh?p
    options.AccessDeniedPath = "/Account/AccessDenied"; // trang n?u k c� quy?n
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // th?i gian s?ng token
    options.SlidingExpiration = true;             // t? ??ng gia h?n token n?u ng??i d�ng ho?t ??ng trong th?i gian s?ng c?a token
});

// 4. ph�n quy?n n�ng cao (Policy-based)
builder.Services.AddAuthorization(options => {
    options.AddPolicy("MarketingManagerPolicy", policy => 
    policy.RequireAuthenticatedUser() // ph?i ??ng nh?p
          .RequireClaim("Department", "Marketing") // ph?i c� claim Department v?i gi� tr? Marketing
          .RequireAssertion(context =>
              {
                  var positionClaim = context.User.FindFirst("Position")?.Value;
                  return positionClaim == "Manager" || positionClaim == "Director";
              }
          ));
   
});
// Add services to the container.

builder.Services.AddMemoryCache();

builder.Services.AddControllersWithViews();
builder.Services.AddValidation();

builder.Services.AddTransient<ITransientService, TransientService>();
builder.Services.AddScoped<IScopedService, ScopedService>();
builder.Services.AddSingleton<ISingletonService, SingletonService>();
builder.Services.AddScoped<ITestDIService, TestDIService>();

var app = builder.Build();
var supportedCultures = new[] { "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
