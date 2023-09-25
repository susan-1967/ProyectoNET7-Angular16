using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Response
{
    public class BaseResponsePagination<T> : BaseResponse
    {
        public ICollection<T>? Data { get; set; }

        public int TotalPages { get; set; }
    }
}
