using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;
using VintageStore.Domain.Info;

namespace VintageStore.Repositories.Interfaces
{
    public interface IVentaRepository : IRepositoryBase<Venta>
    {
        Task CreateTransactionAsync();

        Task RollBackAsync();
        Task<IEnumerable<ReportInfo>> GetReportSaleAsync(DateTime dateStart, DateTime dateEnd);
    }
}
