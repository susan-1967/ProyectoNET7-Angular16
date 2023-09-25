﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;
using VintageStore.Repositories.Interfaces;

namespace VintageStore.Repositories.Implementations
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
where TEntity : EntityBase
    {
        protected readonly DbContext Context;

        protected RepositoryBase(DbContext context)
        {
            Context = context;
        }

        public async Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<TEntity>> ListAsync()
        {
            return await Context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>
            (Expression<Func<TEntity, bool>> predicate,
             Expression<Func<TEntity, TInfo>> selector,
             Expression<Func<TEntity, TKey>> orderBy,
             int page, int rows)
        {
            var collection = await Context.Set<TEntity>()
                .Where(predicate)
                .OrderBy(orderBy)
                .Skip((page - 1) * rows)
                .Take(rows)
                .Select(selector)
                .ToListAsync();

            var total = await Context.Set<TEntity>()
                .Where(predicate)
                .CountAsync();

            return (collection, total);
        }

        public async Task<ICollection<TInfo>> ListAsync<TInfo>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TInfo>> selector)
        {
            return await Context.Set<TEntity>()
                .Where(predicate)
                .Select(selector)
                .ToListAsync();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();

            return entity.Id;
        }

        public virtual async Task<TEntity?> FindByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task UpdateAsync(TEntity? entity = default)
        {
            if (entity is not null)
                entity.ModifiedDate = DateTime.Now;

            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await FindByIdAsync(id);
            if (entity is not null)
            {
                entity.Status = false;
                entity.ModifiedDate = DateTime.Now;
                await UpdateAsync();
            }
            else
                throw new InvalidOperationException($"No se encontró el registro con el Id {id}");
        }
    }
}
