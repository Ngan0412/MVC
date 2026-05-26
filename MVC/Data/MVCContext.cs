using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data
{
    public class MVCContext : IdentityDbContext<IdentityUser>
    {
        public MVCContext(DbContextOptions<MVCContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Bắt buộc phải giữ lại dòng base này để Identity tự cấu hình các thiết lập bảng của nó
            base.OnModelCreating(builder);

            // Code cấu hình Fluent API riêng của bạn (nếu có) viết ở bên dưới...
        }
        public DbSet<MVC.Models.Category> Category { get; set; } = default!;
        public DbSet<MVC.Models.Product> Product { get; set; } = default!;
        public DbSet<MVC.Models.AIModel> AIModel { get; set; } = default!;
        public DbSet<MVC.Models.User> User { get; set; } = default!;
    }
}
