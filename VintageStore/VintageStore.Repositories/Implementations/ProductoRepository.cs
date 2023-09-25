using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
    public class ProductoRepository : RepositoryBase<Producto>, IProductoRepository
    {
        private readonly IMapper _mapper;

        public ProductoRepository(VintageStoreDbContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public override async Task<Producto?> FindByIdAsync(int id)
        {
            return await Context.Set<Producto>()
                .Include(p => p.Tipo)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task FinalizeAsync(int id)
        {
            var registro = await FindByIdAsync(id);
            if (registro is not null)
            {
                registro.Finalized = true;
                await UpdateAsync(registro);
            }
        }
        public async Task<(ICollection<ProductoInfo> Collection, int Total)> ListAsync(string? filter, int page, int rows)
        {
            Expression<Func<Producto, bool>> predicate = p =>
                p.Status
                && p.Nombre.Contains(filter ?? string.Empty);

            Expression<Func<Producto, string>> orderBy = p => p.Nombre;
            //Expression<Func<Concert, ConcertInfo>> selector = p => _mapper.Map<ConcertInfo>(p);

            // Eager Loading
            //var collection = await Context.Set<Concert>()
            //    .Include(p => p.Genre)
            //    .Where(predicate)
            //    .OrderBy(orderBy)
            //    .Skip((page - 1) * rows)
            //    .Take(rows)
            //    .Select(selector)
            //    .ToListAsync();

            //var total = await Context.Set<Concert>()
            //    .Where(predicate)
            //    .CountAsync();


            // Lazy Loading

            return await base.ListAsync(predicate, p => new ProductoInfo()
            {
                Id = p.Id,
                Tipo = p.Tipo.Name,
                TipoId = p.TipoId,
                Nombre = p.Nombre,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                DateEvent = p.DateEvent.ToString("yyyy-MM-dd"),
                TimeEvent = p.DateEvent.ToString("HH:mm"),
                Cantidad = p.Cantidad,
                ImageUrl = p.ImageUrl,
                Status = p.Status ? "Activo" : "Inactivo"
            }, orderBy, page, rows);


            //return (collection, total);
        }
    }
}
