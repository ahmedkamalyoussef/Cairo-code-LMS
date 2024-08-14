using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace LMS.Data.IGenericRepository_IUOW
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> orderBy = null, string direction = null, List<Expression<Func<T, object>>> includes = null);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task RemoveAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        Task<int> CountAsync();

        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, object>>? orderBy = null,
            string direction = null,
            List<Expression<Func<T, object>>> includes = null,
            List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> thenIncludes = null);
        Task<IEnumerable<T>> FilterAsync(int pageSize, int pageIndex, List<Expression<Func<T, bool>>> expressions, Expression<Func<T, object>> orderBy = null, string direction = null, List<Expression<Func<T, object>>> includes = null);

        Task<IEnumerable<T>> FindTopAsync(
            Expression<Func<T, int>> countSelector,
            List<Expression<Func<T, object>>> includes = null,
            int take = 5);

        Task<T> FindFirstAsync(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>> includes = null);
    }
}
