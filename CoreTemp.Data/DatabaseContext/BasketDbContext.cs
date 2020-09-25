﻿using CoreTemp.Data.Models.Basket;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder
                .UseSqlServer(@"Server=(local);Initial Catalog=CoreTempBasket;User Id=sa;Password=fery;Integrated Security=True;MultipleActiveResultSets=True;");
        }

        public DbSet<Basket> Baskets { get; set; }
    }
}