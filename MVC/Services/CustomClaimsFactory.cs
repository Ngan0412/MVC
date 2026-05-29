using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace MVC.Services;

public class CustomClaimsFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>
{
    private readonly UserManager<IdentityUser> _userManager;

    public CustomClaimsFactory(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
        _userManager = userManager;
    }

    // Hàm này chính là nơi đóng gói Claims vào danh tính (Identity) trước khi nén vào Cookie
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
    {
        // 1. Khởi tạo danh tính với các Claim mặc định (ID, Username, Security Stamp)
        var identity = await base.GenerateClaimsAsync(user);

        // 2. Lấy toàn bộ Claims dưới Database lên để lọc
        var allDbClaims = await _userManager.GetClaimsAsync(user);

        // 3. 🎯 BỘ LỌC CHỦ ĐỘNG: Định nghĩa danh sách các Claim ĐƯỢC PHÉP đi vào Cookie
        var allowedClaims = new List<string> { "FullName", "AvatarUrl", "Department" };

        // Duyệt qua mớ hỗn độn dưới DB, cái nào nằm trong danh sách cho phép thì mới nạp vào Cookie
        foreach (var claim in allDbClaims)
        {
            if (allowedClaims.Contains(claim.Type))
            {
                identity.AddClaim(claim); // Nạp vào Cookie
            }
        }
        // Trả về kết quả đã được lọc sạch sẽ
        return identity;
    }
}