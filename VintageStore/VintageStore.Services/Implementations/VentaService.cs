using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;
using VintageStore.Dto.Request;
using VintageStore.Dto.Response;
using VintageStore.Repositories.Interfaces;
using VintageStore.Services.Interfaces;

namespace VintageStore.Services.Implementations
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _repository;
        private readonly ILogger<VentaService> _logger;
        private readonly IMapper _mapper;
        private readonly IProductoRepository _productoRepository;
        private readonly IClienteRepository _clienteRepository;
        public VentaService(IVentaRepository repository,
        ILogger<VentaService> logger,
        IMapper mapper,
        IProductoRepository productoRepository,
        IClienteRepository clienteRepository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
        }
        public async Task<BaseResponseGeneric<int>> AddAsync(string email, VentaDtoRequest request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                await _repository.CreateTransactionAsync();
                var entity = _mapper.Map<Venta>(request);

                var customer = await _clienteRepository.FindByEmailAsync(email);
                if (customer is null)
                {
                    throw new InvalidOperationException($"La cuenta {email} no está registrado como Cliente");
                }

                entity.ClienteId = customer.Id;

                var concert = await _productoRepository.FindByIdAsync(request.ProductoId);
                if (concert is null)
                {
                    throw new Exception($"El concierto con Id {request.ProductoId} no existe");
                }

                //if (DateTime.Today > concert.DateEvent)
                //{
                //    throw new InvalidOperationException(
                //        $"No se puede comprar tickets para el concierto {concert.Nombre} porque ya pasó");
                //}

                entity.Total = entity.Cantidad * concert.UnitPrice;

                await _repository.AddAsync(entity);
                await _repository.UpdateAsync(entity);

                response.Data = entity.Id;

                _logger.LogInformation("Se creo correctamente la venta para {email}", email);

                response.Success = true;
            }
            catch (Exception e)
            {
                await _repository.RollBackAsync();
                _logger.LogError(e, "Error agregando la venta {Message}", e.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<VentaDtoResponse>> FindByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<VentaDtoResponse>();

            try
            {
                var sale = await _repository.FindByIdAsync(id);
                response.Data = _mapper.Map<VentaDtoResponse>(sale);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener la Venta";
                _logger.LogCritical(ex, "{ErrorMessage} {message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<ReportDtoResponse>>> GetReportSaleAsync(DateTime start,
         DateTime end)
        {
            var response = new BaseResponseGeneric<ICollection<ReportDtoResponse>>();
            try
            {
                var list = await _repository.GetReportSaleAsync(start, end);
                response.Data = _mapper.Map<ICollection<ReportDtoResponse>>(list);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener los datos del reporte";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }


        public async Task<BaseResponsePagination<VentaDtoResponse>> ListAsync(DateTime dateStart, DateTime dateEnd, int page, int rows)
        {
            var response = new BaseResponsePagination<VentaDtoResponse>();
            try
            {
                var end = dateEnd.AddHours(23);

                var tuple = await _repository.ListAsync(p => p.FechaVenta >= dateStart && p.FechaVenta <= end,
                    p => _mapper.Map<VentaDtoResponse>(p),
                    x => x.NumeroOperacion,
                    page, rows);

                response.Data = tuple.Collection;
                response.TotalPages = Utilities.GetTotalPages(tuple.Total, rows);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al Listar las ventas por Rango";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponsePagination<VentaDtoResponse>> ListAsync(string email, string? filter, int page, int rows)
        {
            var response = new BaseResponsePagination<VentaDtoResponse>();
            try
            {

                var tuple = await _repository.ListAsync(p => p.Cliente.Email.Equals(email)
                    && p.Producto.Nombre.Contains(filter ?? string.Empty),
                    p => _mapper.Map<VentaDtoResponse>(p),
                    x => x.FechaVenta,
                    page, rows);

                response.Data = tuple.Collection;
                response.TotalPages = Utilities.GetTotalPages(tuple.Total, rows);
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al Listar las ventas del Usuario";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }
    }
}
