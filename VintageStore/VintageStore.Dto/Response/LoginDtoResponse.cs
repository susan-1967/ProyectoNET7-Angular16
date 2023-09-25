using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Response
{
    public  class LoginDtoResponse : BaseResponse
    {
        public string FullName { get; set; } = default!;
        public List<string> Roles { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
