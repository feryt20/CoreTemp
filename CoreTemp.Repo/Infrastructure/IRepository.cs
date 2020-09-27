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
        int Count();
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> where);

        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeEntity);
        PagedList<TEntity> GetAllPagedList(PaginationDto paginationDto);

        TEntity Get(Expression<Func<TEntity, bool>> where);
        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);


        Task<int> CountAsync();
        Task InsertAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> GetAsNoTrackingByIdAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeEntity);
        Task<PagedList<TEntity>> GetAllPagedListAsync(PaginationDto paginationDto, Expression<Func<TEntity,
            bool>> filter);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetManyAsyncPaging(
           Expression<Func<TEntity, bool>> filter,

           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,

           string includeEntity,
           int count,
           int firstCount,
           int page
       );
    }
}
