using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.Models.Basket;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Repo.Repositories.Basket.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Repo.Repositories.Basket.Repository
{
    public class MyBasketRepository : Repository<MyBasket>, IMyBasketRepository
    {
        private readonly DbContext _db;
        public MyBasketRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (BasketDbContext)dbContext;
        }
    }
}
