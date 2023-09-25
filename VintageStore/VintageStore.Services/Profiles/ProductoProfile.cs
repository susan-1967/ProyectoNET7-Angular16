using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain.Info;
using VintageStore.Dto.Response;
using AutoMapper;
using VintageStore.Domain;
using System.Globalization;
using VintageStore.Dto.Request;

namespace VintageStore.Services.Profiles
{
    public class ProductoProfile: Profile
    {
        public ProductoProfile()
        {

            CreateMap<ProductoInfo, ProductoDtoResponse>();

            CreateMap<Producto, ProductoDtoResponse>()
                .ForMember(d => d.Tipo, o => o.MapFrom(_ => _.Tipo.Name))
                .ForMember(d => d.DateEvent, o => o.MapFrom(_ => _.DateEvent.ToString("yyyy-MM-dd")))
                .ForMember(d => d.TimeEvent, o => o.MapFrom(_ => _.DateEvent.ToString("HH:mm")));

            CreateMap<ProductoDtoRequest, Producto>()
                .ForMember(d => d.TipoId, o => o.MapFrom(_ => _.IdTipo))
                .ForMember(d => d.DateEvent, o => o.MapFrom(_ => $"{_.DateEvent} {_.TimeEvent}"));
        }
    }
}
