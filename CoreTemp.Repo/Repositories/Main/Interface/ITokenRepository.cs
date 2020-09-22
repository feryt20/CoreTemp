using CoreTemp.Data.Models.Identity;
using CoreTemp.Repo.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Repo.Repositories.Main.Interface
{
    public interface ITokenRepository : IRepository<MyToken>
    {
    }
}
