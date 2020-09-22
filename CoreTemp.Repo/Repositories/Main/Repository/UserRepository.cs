using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.Models.Identity;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Repo.Repositories.Main.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Repo.Repositories.Main.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DbContext _db;
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (CoreTempDbContext)dbContext;
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await GetAsync(p => p.UserName.Equals(username.ToLower()));
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            if (await GetAsync(p => p.UserName.Equals(username.ToLower())) != null)
                return true;

            return false;
        }
    }
}
