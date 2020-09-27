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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly DbContext _db;
        public OrderDetailRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (CoreTempDbContext)dbContext;
        }
    }
}
