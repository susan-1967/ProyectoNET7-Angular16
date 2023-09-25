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
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;
        private readonly ILogger<ProductoService> _logger;
        private readonly IMapper _mapper;
        private readonly IFileUploader _fileUploader;
        public ProductoService(IProductoRepository repository,
       ILogger<ProductoService> logger,
       IMapper mapper,
       IFileUploader fileUploader)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _fileUploader = fileUploader;
        }
        public async Task<BaseResponsePagination<ProductoDtoResponse>> ListAsync(string? filter,
         int page = 1, int rows = 5)
        {
            var response = new BaseResponsePagination<ProductoDtoResponse>();
            try
            {
                var tupla = await _repository.ListAsync(filter, page, rows);
                response.Data = tupla.Collection
                    .Select(p => _mapper.Map<ProductoDtoResponse>(p))
                    .ToList();


                response.TotalPages = Utilities.GetTotalPages(tupla.Total, rows);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al Listar los conciertos";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ProductoDtoResponse>> FindByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<ProductoDtoResponse>();
            try
            {
                var producto = await _repository.FindByIdAsync(id);

                if (producto == null)
                {
                    response.ErrorMessage = "No se encontró el concierto";
                    return response;
                }

                response.Data = _mapper.Map<ProductoDtoResponse>(producto);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al Obtener el concierto";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(ProductoDtoRequest request)
        {
            var response = new BaseResponseGeneric<int>();

            try
            {
                var concert = _mapper.Map<Producto>(request);

                // Aca colocamos la funcionalidad de subida de archivos
                concert.ImageUrl = await _fileUploader.UploadFileAsync(request.Base64Image, request.FileName);
                await _repository.AddAsync(concert);
                response.Data = concert.Id;
                response.Success = true;

                _logger.LogInformation("Concierto Agregado con exito");
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al Agregar el concierto";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, ProductoDtoRequest request)
        {
            var response = new BaseResponse();
            try
            {
                var producto = await _repository.FindByIdAsync(id);

                if (producto is null)
                {
                    response.ErrorMessage = "No se encontró el concierto";
                    return response;
                }

                _mapper.Map(request, producto);

                if (!string.IsNullOrEmpty(request.FileName))
                {
                    // Agregar funcionalidad de cambio de imagen
                    producto.ImageUrl = await _fileUploader.UploadFileAsync(request.Base64Image, request.FileName);
                }

                await _repository.UpdateAsync(producto);

                response.Success = true;

                _logger.LogInformation("Concierto actualizado con exito");
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al Actualizar el concierto";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }
        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                await _repository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al Eliminar el concierto";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }
        public async Task<BaseResponse> FinalizeAsync(int id)
        {
            var response = new BaseResponse();

            try
            {
                await _repository.FinalizeAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al finalizar el concierto";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }
    }
}
