using CoreTemp.Repo.Repositories.Financial.Interface;
using CoreTemp.Repo.Repositories.Main.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Repo.Infrastructure
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IUserRepository _UserRepository { get; }
        ITokenRepository _TokenRepository { get; }
        IVerificationCodeRepository _VerificationCodeRepository { get; }

        IEntryRepository EntryRepository { get; }
        IFactorRepository FactorRepository { get; }

        void Save();
        Task<int> SaveAsync();
    }
}
