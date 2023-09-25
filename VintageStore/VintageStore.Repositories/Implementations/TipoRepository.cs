using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;
using VintageStore.Persistence;
using VintageStore.Repositories.Interfaces;

namespace VintageStore.Repositories.Implementations
{
    public class TipoRepository : RepositoryBase<Tipo>, ITipoRepository
    {
        public TipoRepository(VintageStoreDbContext context)
        : base(context)
        {

        }

    }
}
