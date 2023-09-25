using Microsoft.EntityFrameworkCore;
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
    public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
    {
        public ClienteRepository(VintageStoreDbContext context)
            : base(context)
        {
        }

        public async Task<Cliente?> FindByEmailAsync(string email)
        {
            return await Context.Set<Cliente>().FirstOrDefaultAsync(p => p.Email == email);
        }
    }
}
