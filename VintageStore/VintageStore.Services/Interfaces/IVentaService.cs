using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Dto.Request;
using VintageStore.Dto.Response;

namespace VintageStore.Services.Interfaces
{
    public interface IVentaService
    {
        Task<BaseResponseGeneric<int>> AddAsync(string email, VentaDtoRequest request);

        Task<BaseResponseGeneric<VentaDtoResponse>> FindByIdAsync(int id);

        Task<BaseResponsePagination<VentaDtoResponse>> ListAsync(DateTime dateStart, DateTime dateEnd, int page, int rows);

        Task<BaseResponsePagination<VentaDtoResponse>> ListAsync(string email, string? filter, int page, int rows);

        Task<BaseResponseGeneric<ICollection<ReportDtoResponse>>> GetReportSaleAsync(DateTime start,
            DateTime end);
    }
}
