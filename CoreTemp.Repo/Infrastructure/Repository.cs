using CoreTemp.Common.Common;
using CoreTemp.Data.DTOs.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Repo.Infrastructure
{
    public abstract class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        private readonly DbContext _db;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(DbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }


        #region normal
        public int Count(Expression<Func<TEntity, bool>> where = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
                query = query.Where(where);

            return query.Count();
        }
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        public  void AddRange(List<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentException("nulll");
             _dbSet.AddRange(entities);
        }
        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("nulll");
            _dbSet.Update(entity);
        }
        public void Delete(object id)
        {
            var entity = GetById(id);
            if (entity == null)
                throw new ArgumentException("nulll");
            _dbSet.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("nulll");
            _dbSet.Remove(entity);
        }
        public void Delete(Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> obj = _dbSet.Where(where).AsEnumerable();
            foreach (var item in obj)
            {
                _dbSet.Remove(item);
            }
        }
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentException("nulll");
            _dbSet.RemoveRange(entities);
        }
        public TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }
        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> where = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);
            }

            return query.FirstOrDefault(where);
        }
        public TEntity GetAsNoTracking(Expression<Func<TEntity, bool>> where = null)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(where);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);
            }

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = orderBy(query);

            return query.ToList();
        }

        public IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
            {
                query = query.Where(where);
            }
            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public PagedList<TEntity> GetAllPagedList(PaginationDto paginationDto, Expression<Func<TEntity,
            bool>> where)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            //where
            if (where != null)
            {
                query = query.Where(where);
            }
            //include

            //orderby

            return PagedList<TEntity>.Create(query, paginationDto.PageNumber, paginationDto.PageSize);
        }

        public  IEnumerable<TEntity> GetManyPaging(Expression<Func<TEntity,
            bool>> where, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy,
            string includeEntity, int count, int firstCount, int page)
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
            {
                query = query.Where(where);
            }

            foreach (var includeentity in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeentity);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return  query.Skip(firstCount).Skip(count * page).Take(count).ToList();
        }

        #endregion normal


        #region Async
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> where = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
                query = query.Where(where);
            return await _dbSet.CountAsync();
        }
        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("nulll");
            await _dbSet.AddAsync(entity);
        }
        public async Task AddRangeAsync(List<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentException("nulll");
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(where);
        }
        public async Task<TEntity> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> where = null)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(where);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
            {
                query = query.Where(where);
            }
            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);
            }

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }
        public async Task<PagedList<TEntity>> GetAllPagedListAsync(PaginationDto paginationDto, Expression<Func<TEntity,
           bool>> where)
        {
            IQueryable<TEntity> query = _dbSet;
            //where
            if (where != null)
            {
                query = query.Where(where);
            }
            //include

            //orderby

            return await PagedList<TEntity>.CreateAsync(query,
                paginationDto.PageNumber, paginationDto.PageSize);
        }

        public async Task<IEnumerable<TEntity>> GetManyAsyncPaging(Expression<Func<TEntity,
            bool>> where, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy,
            string includeEntity, int count, int firstCount, int page)
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
            {
                query = query.Where(where);
            }

            foreach (var includeentity in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeentity);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.Skip(firstCount).Skip(count * page).Take(count).ToListAsync();
        }

        #endregion Async



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
      

        ~Repository()
        {
            Dispose(false);
        }
        #endregion dispose
    }
}
