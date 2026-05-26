using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MVCContext") ?? throw new InvalidOperationException("Connection string 'MVCContext' not found.")));
// 2. ??ng ký ASP.NET Core Identity (S? d?ng IdentityUser và IdentityRole)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<MVCContext>() // ch? ??nh n?i l?u tr? d? li?u Identity (s? d?ng DbContext ?ã ??ng ký)
.AddDefaultTokenProviders(); // b?t tính n?ng xác th?c OTP, email.

// 3. C?u hình Cookie Authentication
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";         // trang chuy?n h??ng khi ch?a ??ng nh?p
    options.AccessDeniedPath = "/Account/AccessDenied"; // trang n?u k có quy?n
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // th?i gian s?ng token
    options.SlidingExpiration = true;             // t? ??ng gia h?n token n?u ng??i dùng ho?t ??ng trong th?i gian s?ng c?a token
});

// 4. phân quy?n nâng cao (Policy-based)
builder.Services.AddAuthorization(options => {
    options.AddPolicy("MarketingManagerPolicy", policy => 
    policy.RequireAuthenticatedUser() // ph?i ??ng nh?p
          .RequireClaim("Department", "Marketing") // ph?i có claim Department v?i giá tr? Marketing
          .RequireAssertion(context =>
              {
                  var positionClaim = context.User.FindFirst("Position")?.Value;
                  return positionClaim == "Manager" || positionClaim == "Director";
              }
          ));
   
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddValidation();
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
