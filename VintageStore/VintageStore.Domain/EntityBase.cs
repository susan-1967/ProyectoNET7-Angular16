using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Domain
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Status { get; set; }

        protected EntityBase()
        {
            CreationDate = DateTime.Now;
            Status = true;
        }
    }
}
