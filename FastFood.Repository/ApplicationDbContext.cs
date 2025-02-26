using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using FastFood.Modals;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                            .HasOne(i => i.SubCategory)
                            .WithMany(sc => sc.Items)
                            .HasForeignKey(i => i.SubCategoryId)
                            .OnDelete(DeleteBehavior.Restrict);  // Prevents cascade delete

            // Disable cascade delete from SubCategory to Category
            modelBuilder.Entity<SubCategory>()
                        .HasOne(sc => sc.Category)
                        .WithMany(c => c.SubCategories)
                        .HasForeignKey(sc => sc.CategoryId)
                        .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
