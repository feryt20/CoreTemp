using CoreTemp.Data.Models.Log;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

namespace CoreTemp.Data.DatabaseContext
{
    public class LogDbContext : DbContext
    {
        public LogDbContext()
        {

        }
        public LogDbContext(DbContextOptions<LogDbContext> opt) : base(opt)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        //{

        //    optionBuilder
        //        .UseSqlServer("Server=(local);Initial Catalog=CoreTempLog;User Id=sa;Password=fery;Integrated Security=True;MultipleActiveResultSets=True;");
        //}

        public DbSet<ExtendedLog> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            LogModelBuilderHelper.Build(modelBuilder.Entity<ExtendedLog>());

            modelBuilder.Entity<ExtendedLog>().ToTable("ExtendedLog");
        }
    }
}
