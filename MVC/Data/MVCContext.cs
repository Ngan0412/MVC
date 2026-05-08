using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Data
{
    public class MVCContext : DbContext
    {
        public MVCContext (DbContextOptions<MVCContext> options)
            : base(options)
        {
        }
        public DbSet<MVC.Models.Category> Category { get; set; } = default!;
        public DbSet<MVC.Models.Product> Product { get; set; } = default!;
        public DbSet<MVC.Models.AIModel> AIModel { get; set; } = default!;
        public DbSet<MVC.Models.User> User { get; set; } = default!;
    }
}
