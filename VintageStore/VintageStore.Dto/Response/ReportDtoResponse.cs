using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Response
{
    public class ReportDtoResponse
    {
        public string ProductoName { get; set; } = default!;
        public decimal Total { get; set; }
    }
}
