using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Request
{
    public record LoginDtoRequest(string UserName, string Password);
}
