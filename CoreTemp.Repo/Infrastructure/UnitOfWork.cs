using CoreTemp.Common.Helpers;
using CoreTemp.Repo.Repositories.Financial.Interface;
using CoreTemp.Repo.Repositories.Financial.Repository;
using CoreTemp.Repo.Repositories.Main.Interface;
using CoreTemp.Repo.Repositories.Main.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Repo.Infrastructure
{
    //public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        #region ctr
        protected readonly DbContext _db;
        public UnitOfWork(TContext context)
        {
            //_db = new TContext();
            _db = context;
        }
        #endregion


        private IUserRepository userRepository;
        public IUserRepository _UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(_db);
                }
                return userRepository;
            }
        }


        private ITokenRepository tokenRepository;
        public ITokenRepository _TokenRepository
        {
            get
            {
                if (tokenRepository == null)
                {
                    tokenRepository = new TokenRepository(_db);
                }
                return tokenRepository;
            }
        }

      

        private IVerificationCodeRepository verificationCodeRepository;
        public IVerificationCodeRepository _VerificationCodeRepository
        {
            get
            {
                if (verificationCodeRepository == null)
                {
                    verificationCodeRepository = new VerificationCodeRepository(_db);
                }
                return verificationCodeRepository;
            }
        }

        private IProductGroupRepository productGroupRepository;
        public IProductGroupRepository _ProductGroupRepository
        {
            get
            {
                if (productGroupRepository == null)
                {
                    productGroupRepository = new ProductGroupRepository(_db);
                }
                return productGroupRepository;
            }
        }

        private IProductRepository productRepository;
        public IProductRepository _ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository(_db);
                }
                return productRepository;
            }
        }

        private IOrderRepository orderRepository;
        public IOrderRepository _OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new OrderRepository(_db);
                }
                return orderRepository;
            }
        }

        #region methods


        #region privateFinancialrepository
        private IEntryRepository entryRepository;
        public IEntryRepository EntryRepository
        {
            get
            {
                if (entryRepository == null)
                {
                    entryRepository = new EntryRepository(_db);
                }
                return entryRepository;
            }
        }
        private IFactorRepository factorRepository;
        public IFactorRepository FactorRepository
        {
            get
            {
                if (factorRepository == null)
                {
                    factorRepository = new FactorRepository(_db);
                }
                return factorRepository;
            }
        }
        #endregion

        public void Save()
        {
            _cleanStrings();
            _db.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                _cleanStrings();
                return await _db.SaveChangesAsync();
            }
            catch (Exception)
            {

                return 0;
            }

        }

        private void _cleanStrings()
        {
            var changedEntities = _db.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added || p.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.CleanString();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
            {

            }
        }
        #endregion

        #region dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion dispose
    }
}
