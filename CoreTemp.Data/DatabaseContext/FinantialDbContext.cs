using CoreTemp.Data.Models.Finantial;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Data.DatabaseContext
{
    public class FinantialDbContext : DbContext
    {
        public FinantialDbContext()
        {
        }
        public FinantialDbContext(DbContextOptions<FinantialDbContext> opt) : base(opt)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder
                .UseSqlServer(@"Server=(local);Initial Catalog=CoreTempFinantial;User Id=sa;Password=fery;Integrated Security=True;MultipleActiveResultSets=True;");
        }

        public DbSet<Factor> Factors { get; set; }
        public DbSet<Entry> Entries { get; set; }
    }
}
