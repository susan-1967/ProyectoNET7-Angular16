using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Response
{
    public class VentaDtoResponse
    {
        public int SaleId { get; set; }
        public string DateEvent { get; set; } = default!;
        public string TimeEvent { get; set; } = default!;
        public string Tipo { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public string Title { get; set; } = default!;
        public string NumeroOperacion { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public short Cantidad { get; set; }
        public string SaleDate { get; set; } = default!;
        public decimal Total { get; set; }
    }
}
