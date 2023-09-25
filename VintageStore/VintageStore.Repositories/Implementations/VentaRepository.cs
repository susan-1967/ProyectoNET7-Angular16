using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;
using VintageStore.Domain.Info;
using VintageStore.Persistence;
using VintageStore.Repositories.Interfaces;

namespace VintageStore.Repositories.Implementations
{
    public class VentaRepository : RepositoryBase<Venta>, IVentaRepository
    {
        public VentaRepository(VintageStoreDbContext context) : base(context)
        {
        }

        public async Task CreateTransactionAsync()
        {
            await Context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        }

        public override async Task UpdateAsync(Venta? entity = default)
        {
            await Context.Database.CommitTransactionAsync();
            await base.UpdateAsync(entity);
        }

        public async Task RollBackAsync()
        {
            await Context.Database.RollbackTransactionAsync();
        }

        public override async Task<int> AddAsync(Venta entity)
        {
            entity.FechaVenta = DateTime.Now;
            var lastNumber = await Context.Set<Venta>().CountAsync() + 1;
            entity.NumeroOperacion = $"{lastNumber:00000}";

            // Agregar una entidad al Context de forma implicita
            await Context.AddAsync(entity);
            // La forma explicita
            //await Context.Set<Sale>().AddAsync(entity);

            return entity.Id;
        }

        public override async Task<Venta?> FindByIdAsync(int id)
        {
            //// Eager Loading
            //return await Context.Set<Sale>()
            //    .Include(p => p.Customer)
            //    .Include(p => p.Concert)
            //    .ThenInclude(p => p.Genre)
            //    .Where(p => p.Id == id)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync();

            // Lazy Loading
            var query = Context.Set<Venta>()
                .Where(p => p.Id == id)
                .Select(p => new Venta
                {
                    Id = p.Id,
                    Cliente = new Cliente()
                    {
                        FullName = p.Cliente.FullName
                    },
                    Producto = new Producto()
                    {
                        DateEvent = p.Producto.DateEvent,
                        ImageUrl = p.Producto.ImageUrl,
                        Nombre = p.Producto.Nombre,
                        Tipo = new Tipo
                        {
                            Name = p.Producto.Tipo.Name
                        }
                    },
                    NumeroOperacion = p.NumeroOperacion,
                    Cantidad = p.Cantidad,
                    FechaVenta = p.FechaVenta,
                    Total = p.Total
                });

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ReportInfo>> GetReportSaleAsync(DateTime dateStart, DateTime dateEnd)
        {
            var query = await Context.Database.GetDbConnection()
                .QueryAsync<ReportInfo>(sql: "uspReportSales", commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DateStart = dateStart,
                        DateEnd = dateEnd
                    });

            return query;
        }

        public override async Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>
            (Expression<Func<Venta, bool>> predicate,
            Expression<Func<Venta, TInfo>> selector,
            Expression<Func<Venta, TKey>> orderBy,
            int page, int rows)
        {
            var collection = await Context.Set<Venta>()
                .Include(p => p.Producto)
                .ThenInclude(p => p.Tipo)
                .Include(p => p.Cliente)
                .Where(predicate)
                .OrderBy(orderBy)
                .Skip((page - 1) * rows)
                .Take(rows)
                .Select(selector)
                .ToListAsync();

            var total = await Context.Set<Venta>()
                .Where(predicate)
                .CountAsync();

            return (collection, total);
        }
    }
}
