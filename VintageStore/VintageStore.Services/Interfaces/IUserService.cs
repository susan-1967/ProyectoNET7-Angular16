using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Dto.Request;
using VintageStore.Dto.Response;

namespace VintageStore.Services.Interfaces
{
    public  interface IUserService
    {
        Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request);

        Task<BaseResponseGeneric<string>> RegisterAsync(RegisterDtoRequest request);
        Task<BaseResponse> RequestTokenToResetPasswordAsync(DtoResetPasswordRequest request);
        Task<BaseResponse> ResetPasswordAsync(DtoConfirmPasswordRequest request);

        Task<BaseResponse> ChangePasswordAsync(string email, ChangePasswordRequest request);
    }
}
