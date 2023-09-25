using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Persistence
{
    public class VintageStoreUserIdentity : IdentityUser
     {
        [StringLength(100)]
        public string FirstName { get; set; } = default!;

        [StringLength(100)]
        public string LastName { get; set; } = default!;

        public int Age { get; set; }
        public DocumentTypeEnum DocumentType { get; set; }

        [StringLength(20)]
        public string DocumentNumber { get; set; } = default!;
    }
}
