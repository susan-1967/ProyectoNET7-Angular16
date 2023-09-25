using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Request
{
    public class ProductoDtoRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Nombre { get; set; } = default!;

        public string Description { get; set; } = default!;

        public int IdTipo { get; set; }

        public string DateEvent { get; set; } = default!;
        public string TimeEvent { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int Cantidad { get; set; }
        public string? Base64Image { get; set; }
        public string? FileName { get; set; }
    }
}
