using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Domain.Info
{
    public class ProductoInfo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Place { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public string Tipo { get; set; } = default!;
        public int TipoId { get; set; }
        public string DateEvent { get; set; } = default!;
        public string TimeEvent { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public int Cantidad { get; set; }
        public bool Finalized { get; set; }
        public string Status { get; set; } = default!;
    }
}
