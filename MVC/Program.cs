using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MVCContext") ?? throw new InvalidOperationException("Connection string 'MVCContext' not found.")));
// 2. ??ng ký ASP.NET Core Identity (S? d?ng IdentityUser và IdentityRole m?c ??nh)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    // Tùy bi?n quy ??nh v? m?t kh?u (Password Rules)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false; // Không b?t ký t? ??c bi?t
})
.AddEntityFrameworkStores<MVCContext>()
.AddDefaultTokenProviders();

// 3. C?u hình Cookie Authentication
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";         // Trang chuy?n h??ng n?u ch?a ??ng nh?p
    options.AccessDeniedPath = "/Account/AccessDenied"; // Trang chuy?n h??ng n?u không ?? quy?n
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Th?i gian s?ng c?a Cookie
    options.SlidingExpiration = true;             // T? ??ng gia h?n khi user còn ho?t ??ng
});

// 4. C?u hình Phân quy?n nâng cao (Policy-based)
builder.Services.AddAuthorization(options => {
    // ??ng ký m?t Policy tên là "Over18Policy"
    options.AddPolicy("Over18Policy", policy =>
        policy.RequireClaim("AgeClaim") // B?t bu?c user ph?i có th? thông tin tên "AgeClaim"
              .RequireAssertion(context => {
                  // Logic ki?m tra: Tu?i l?y t? Claim ph?i >= 18
                  var ageValue = context.User.FindFirst("AgeClaim")?.Value;
                  return int.TryParse(ageValue, out int age) && age >= 18;
              }));
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
