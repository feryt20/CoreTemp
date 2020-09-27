using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.Models.Site;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Repo.Repositories.Main.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Repo.Repositories.Main.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly DbContext _db;
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (CoreTempDbContext)dbContext;
        }
    }
}
