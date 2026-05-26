using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVC.Controllers;

public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AdminController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }
    [HttpGet]
    public IActionResult Index()
    {
        // Lấy toàn bộ danh sách User từ bảng AspNetUsers
        var users = _userManager.Users.ToList();

        // Lấy toàn bộ danh sách Role từ bảng AspNetRoles
        var roles = _roleManager.Roles.ToList();

        ViewBag.Roles = roles;
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (!string.IsNullOrEmpty(roleName))
        {
            // Kiểm tra xem trùng tên chưa, nếu chưa thì tạo mới
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && !string.IsNullOrEmpty(roleName))
        {
            // Hàm này tự chèn 1 dòng gồm UserId và RoleId tương ứng vào bảng AspNetUserRoles
            await _userManager.AddToRoleAsync(user, roleName);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> ManageClaims(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        // Đọc ra tất cả các Claim hiện có của User này trong bảng AspNetUserClaims
        var currentClaims = await _userManager.GetClaimsAsync(user);

        ViewBag.User = user;
        return View(currentClaims); // Trả về danh sách Claim để hiển thị
    }

    [HttpPost]
    public async Task<IActionResult> AddClaim(string userId, string claimType, string claimValue)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && !string.IsNullOrEmpty(claimType))
        {
            // Tạo thẻ mới và lưu vào bảng AspNetUserClaims
            var newClaim = new Claim(claimType, claimValue);
            await _userManager.AddClaimAsync(user, newClaim);
        }
        return RedirectToAction("ManageClaims", new { userId = userId });
    }

    [HttpPost]
    public async Task<IActionResult> LockUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // Ép thuộc tính LockoutEnd (Thời gian hết hạn khóa) kéo dài đến 100 năm sau
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UnlockUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // Đưa thời gian khóa về thời điểm hiện tại để mở khóa luôn
            await _userManager.SetLockoutEndDateAsync(user, null);

            // Reset lại thuộc tính AccessFailedCount (Số lần nhập sai pass) về số 0
            await _userManager.ResetAccessFailedCountAsync(user);
        }
        return RedirectToAction("Index");
    }
}
