using CoreTemp.Data.Models.Identity;
using CoreTemp.Data.Models.Site;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Data.DatabaseContext
{
    public class CoreTempDbContext : IdentityDbContext<MyUser, Role, string,
    IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public CoreTempDbContext()
        {
        }
        public CoreTempDbContext(DbContextOptions<CoreTempDbContext> opt) : base(opt)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(local);Initial Catalog=CoreTempMain;User Id=sa;Password=fery;Integrated Security=True;MultipleActiveResultSets=True;");
        //    //optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=(local);Initial Catalog=CoreTempMain;User Id=sa;Password=fery;Integrated Security=True;MultipleActiveResultSets=True;");
        //}

        public DbSet<MyToken> MyTokens { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }

        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PaymentLog> PaymentLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Product>()
                .HasIndex(u => u.ProductId2)
                .IsUnique();

            //builder.Entity<Photo>()
            //   .Property(x => x.RowVersion)
            //   .IsConcurrencyToken()
            //   .ValueGeneratedOnAddOrUpdate();
        }



    }


    //add-migration CoreTempMain -c CoreTempDbContext -o Migrations\DbMain
    //update-database -Context CoreTempDbContext
}
