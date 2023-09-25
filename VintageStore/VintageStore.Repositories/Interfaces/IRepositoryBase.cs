using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;

namespace VintageStore.Repositories.Interfaces
{
    public interface IRepositoryBase<TEntity>
    where TEntity : EntityBase
    {
        /// <summary>
        /// Lista de objetos basados en el Entity
        /// </summary>
        Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Lista de objetos basados en el Entity pero sin filtros
        /// </summary>
        Task<ICollection<TEntity>> ListAsync();

        /// <summary>
        /// Lista de objetos transformados en otro objeto y que ademas contenga paginacion
        /// </summary>
        Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TInfo>> selector,
            Expression<Func<TEntity, TKey>> orderBy,
            int page, int rows);

        /// <summary>
        /// Listar los objetos con un selector
        /// </summary>
        Task<ICollection<TInfo>> ListAsync<TInfo>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TInfo>> selector);

        Task<int> AddAsync(TEntity entity);

        Task<TEntity?> FindByIdAsync(int id);

        Task UpdateAsync(TEntity? entity);

        Task DeleteAsync(int id);
    }
}
