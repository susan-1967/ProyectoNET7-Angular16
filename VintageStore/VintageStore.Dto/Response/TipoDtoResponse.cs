using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Response
{
    public class TipoDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public bool Status { get; set; }
    }
}
