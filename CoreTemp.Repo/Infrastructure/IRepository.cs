using CoreTemp.Common.Common;
using CoreTemp.Data.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Repo.Infrastructure
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region sync
        int Count(Expression<Func<TEntity, bool>> where = null);
        void Add(TEntity entity);
        void AddRange(List<TEntity> entities);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> where);
        void DeleteRange(IEnumerable<TEntity> entities);
        TEntity GetById(object id);
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> where = null, params Expression<Func<TEntity, object>>[] includes);
        TEntity GetAsNoTracking(Expression<Func<TEntity, bool>> where = null);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> where,Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
             params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);
        PagedList<TEntity> GetAllPagedList(PaginationDto paginationDto, Expression<Func<TEntity,bool>> where);

        IEnumerable<TEntity> GetManyPaging(
           Expression<Func<TEntity, bool>> where,

           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,

           string includeEntity,
           int count,
           int firstCount,
           int page
       );
        #endregion sync



        #region async
        Task<int> CountAsync(Expression<Func<TEntity, bool>> where = null);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> where = null);
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> where,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
             params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);

        Task<PagedList<TEntity>> GetAllPagedListAsync(PaginationDto paginationDto, Expression<Func<TEntity,
            bool>> where);
        Task<IEnumerable<TEntity>> GetManyAsyncPaging(
           Expression<Func<TEntity, bool>> where,

           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,

           string includeEntity,
           int count,
           int firstCount,
           int page
       );

#endregion async
    }
}
