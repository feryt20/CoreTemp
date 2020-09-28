using CoreTemp.Data.Models.Basket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Data.DatabaseContext
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext()
        {
        }
        public BasketDbContext(DbContextOptions<BasketDbContext> opt) : base(opt)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        //{
        //    optionBuilder
        //        .UseSqlServer(@"Server=(local);Initial Catalog=CoreTempBasket;User Id=sa;Password=fery;Integrated Security=True;MultipleActiveResultSets=True;");
        //}


        public DbSet<MyBasket> Baskets { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MyBasket>()
                .HasIndex(u => u.UserId);

            builder.Entity<MyBasket>()
                .HasIndex(u => u.ProductId);
        }
    }
}
