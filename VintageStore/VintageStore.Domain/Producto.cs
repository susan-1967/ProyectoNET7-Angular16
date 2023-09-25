using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Domain
{
    public class Producto : EntityBase
    {
        [StringLength(20)] // Esto no tiene prioridad sobre el Fluent API
        public required string Nombre { get; set; }
        public string Description { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public Tipo Tipo { get; set; } = default!;
        public int TipoId { get; set; }
        public DateTime DateEvent { get; set; }
        public string? ImageUrl { get; set; }
        public int Cantidad { get; set; }
        public bool Finalized { get; set; }
    }
}
