using CoreTemp.Data.Models.Identity;
using CoreTemp.Repo.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Repo.Repositories.Main.Interface
{
    public interface IUserRepository : IRepository<MyUser>
    {
        Task<MyUser> GetUserByUserNameAsync(string username);
        Task<bool> UserExistsAsync(string username);
    }
}
