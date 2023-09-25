using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Dto.Request;
using VintageStore.Dto.Response;

namespace VintageStore.Services.Interfaces
{
    public interface ITipoService
    {
        Task<BaseResponseGeneric<ICollection<TipoDtoResponse>>> ListAsync();

        Task<BaseResponseGeneric<TipoDtoResponse>> FindByIdAsync(int id);

        Task<BaseResponseGeneric<int>> AddAsync(TipoDtoRequest request);

        Task<BaseResponse> UpdateAsync(int id, TipoDtoRequest request);

        Task<BaseResponse> DeleteAsync(int id);
    }
}
