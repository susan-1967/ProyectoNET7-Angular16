using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Dto.Request;
using VintageStore.Dto.Response;

namespace VintageStore.Services.Interfaces
{
    public interface IProductoService
    {
        Task<BaseResponsePagination<ProductoDtoResponse>> ListAsync(string? filter, int page = 1, int rows = 5);

        Task<BaseResponseGeneric<ProductoDtoResponse>> FindByIdAsync(int id);

        Task<BaseResponseGeneric<int>> AddAsync(ProductoDtoRequest request);
        Task<BaseResponse> UpdateAsync(int id, ProductoDtoRequest request);

        Task<BaseResponse> DeleteAsync(int id);
        Task<BaseResponse> FinalizeAsync(int id);
    }
}
