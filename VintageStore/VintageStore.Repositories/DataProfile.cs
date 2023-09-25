using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;
using VintageStore.Domain.Info;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VintageStore.Repositories
{
    public  class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<Producto, ProductoInfo>()
                .ForMember(d => d.Id, o => o.MapFrom(x => x.Id))
                .ForMember(d => d.Nombre, o => o.MapFrom(x => x.Nombre))
                .ForMember(d => d.Description, o => o.MapFrom(x => x.Description))
                .ForMember(d => d.UnitPrice, o => o.MapFrom(x => x.UnitPrice))
                .ForMember(d => d.Tipo, o => o.MapFrom(x => x.Tipo.Name))
                .ForMember(d => d.TipoId, o => o.MapFrom(x => x.TipoId))
                .ForMember(d => d.Cantidad, o => o.MapFrom(x => x.Cantidad))
                .ForMember(d => d.DateEvent, o => o.MapFrom(x => x.DateEvent.ToString("yyyy-MM-dd")))
                .ForMember(d => d.TimeEvent, o => o.MapFrom(x => x.DateEvent.ToString("HH:mm:ss")))
                .ForMember(d => d.Finalized, o => o.MapFrom(x => x.Finalized));
        }
    }
}
