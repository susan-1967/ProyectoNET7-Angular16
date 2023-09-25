using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VintageStore.Domain;
using VintageStore.Domain.Info;
using VintageStore.Dto.Request;
using VintageStore.Dto.Response;

namespace VintageStore.Services.Profiles
{
    public  class VentaProfile : Profile

    {
        public static CultureInfo Culture = new("es-PE");
        public VentaProfile()
        {
            CreateMap<VentaDtoRequest, Venta>()
                .ForMember(d => d.ProductoId, o => o.MapFrom(_ => _.ProductoId)) // Opcional
                .ForMember(d => d.Cantidad, o => o.MapFrom(_ => _.Cantidad));

            // Aqui se personalizan unicamente los campos que son diferentes
            CreateMap<Venta, VentaDtoResponse>()
                .ForMember(d => d.SaleId, o => o.MapFrom(_ => _.Id))
                .ForMember(d => d.DateEvent, o => o.MapFrom(_ => _.Producto.DateEvent.ToString("D", Culture)))
                .ForMember(d => d.TimeEvent, o => o.MapFrom(_ => _.Producto.DateEvent.ToString("T", Culture)))
                .ForMember(d => d.Tipo, o => o.MapFrom(_ => _.Producto.Tipo.Name))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(_ => _.Producto.ImageUrl))
                .ForMember(d => d.Title, o => o.MapFrom(_ => _.Producto.Nombre))
                .ForMember(d => d.FullName, o => o.MapFrom(_ => _.Cliente.FullName))
                .ForMember(d => d.SaleDate, o => o.MapFrom(_ => _.FechaVenta.ToString("dd/MM/yyyy HH:mm", Culture)));

            CreateMap<ReportInfo, ReportDtoResponse>();
        }
    }
}
