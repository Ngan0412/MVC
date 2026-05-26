using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVC.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    // ==========================================
    // 1. CHỨC NĂNG ĐĂNG KÝ (REGISTER)
    // ==========================================

    // [GET] Hiển thị trang Form Đăng Ký cho người dùng điền
    [HttpGet]
    public IActionResult Register()
    {
        // Nếu người dùng đã đăng nhập rồi thì đá họ về trang chủ luôn, không cho đăng ký lại
        //if (User.Identity != null && User.Identity.IsAuthenticated)
        //{
        //    return RedirectToAction("Index", "Home");
        //}
        return View();
    }

    // [POST] Tiếp nhận dữ liệu gửi lên từ Form Đăng Ký và xử lý lưu vào Database
    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, int age)
    {
        var user = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            // Thêm các thông tin bổ sung (Claims) vào User này để phục vụ Policy về sau
            await _userManager.AddClaimAsync(user, new Claim("AgeClaim", age.ToString()));

            // Gán luôn vai trò mặc định là "User"
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            // Đăng nhập luôn sau khi đăng ký thành công
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        // Nếu có lỗi (Ví dụ: mật khẩu quá ngắn, trùng email), đẩy lỗi vào ModelState để View hiển thị ra
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View();
    }

    // ==========================================
    // 2. CHỨC NĂNG ĐĂNG NHẬP (LOGIN)
    // ==========================================

    // [GET] Hiển thị trang Form Đăng Nhập cho người dùng điền
    [HttpGet]
    public IActionResult Login()
    {
        // Nếu đã đăng nhập rồi thì chuyển về trang chủ, không cần hiển thị form đăng nhập nữa
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // [POST] Tiếp nhận dữ liệu từ Form Đăng Nhập để kiểm tra và cấp Cookie
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, bool rememberMe)
    {
        // Hàm này tự băm pass gửi lên, đối chiếu database và tự cấp Auth Cookie về trình duyệt nếu khớp
        var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
        return View();
    }

    // ==========================================
    // 3. CHỨC NĂNG ĐĂNG XUẤT (LOGOUT)
    // ==========================================

    [HttpPost] // Thực tế nên dùng POST cho nút đăng xuất để đảm bảo an toàn bảo mật
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync(); // Xóa sạch Auth Cookie khỏi trình duyệt
        return RedirectToAction("Index", "Home");
    }

    // ==========================================
    // 4. TRANG BÁO LỖI KHÔNG ĐỦ QUYỀN
    // ==========================================

    [HttpGet]
    public IActionResult AccessDenied() => View();

    // [GET] Hiển thị giao diện đổi mật khẩu
    [Authorize] // Bắt buộc phải đăng nhập mới mở được trang này
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    // [POST] Xử lý nghiệp vụ đổi mật khẩu
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        // Hàm tự động check pass cũ, băm pass mới và cập nhật database
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (result.Succeeded)
        {
            // Đổi pass xong cần nạp lại trạng thái đăng nhập cho cookie đồng bộ
            await _signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Index", "Home");
        }

        // Nếu lỗi (mật khẩu cũ sai, mật khẩu mới không đủ độ bảo mật...)
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View();
    }
}
