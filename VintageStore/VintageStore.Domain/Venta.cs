using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Domain
{
    public  class Venta : EntityBase
    {
        public Cliente Cliente { get; set; } = default!;
        public int ClienteId { get; set; }

        public Producto Producto { get; set; } = default!;
        public int ProductoId { get; set; }

        public DateTime FechaVenta { get; set; }
        public string NumeroOperacion { get; set; } = default!;
        public decimal Total { get; set; }
        public short Cantidad { get; set; } // 255
    }
}
