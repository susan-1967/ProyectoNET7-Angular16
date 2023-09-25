using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;

namespace VintageStore.Repositories.Interfaces
{
    public interface IClienteRepository : IRepositoryBase<Cliente>
    {
        Task<Cliente?> FindByEmailAsync(string email);
    }
    
}
