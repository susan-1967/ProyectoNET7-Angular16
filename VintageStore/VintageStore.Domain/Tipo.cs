using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Domain
{
    public class Tipo : EntityBase
    {
        public required string Name { get; set; }
    }
}
