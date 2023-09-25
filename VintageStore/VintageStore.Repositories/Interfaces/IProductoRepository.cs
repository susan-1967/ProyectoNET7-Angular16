using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;
using VintageStore.Domain.Info;

namespace VintageStore.Repositories.Interfaces
{
    public interface IProductoRepository : IRepositoryBase<Producto>
    {
        Task<(ICollection<ProductoInfo> Collection, int Total)> ListAsync(string? filter, int page, int rows);


        Task FinalizeAsync(int id);
    }
}
