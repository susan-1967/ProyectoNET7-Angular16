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
    public class TipoService : ITipoService
    {
        private readonly ITipoRepository _repository;
        private readonly ILogger<TipoService> _logger;

        public TipoService(ITipoRepository repository, ILogger<TipoService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BaseResponseGeneric<ICollection<TipoDtoResponse>>> ListAsync()
        {
            var response = new BaseResponseGeneric<ICollection<TipoDtoResponse>>();
            try
            {
                var data = await _repository.ListAsync();

                response.Data = data.Select(p => new TipoDtoResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Status = p.Status
                }).ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al listar";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponseGeneric<TipoDtoResponse>> FindByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<TipoDtoResponse>();
            try
            {
                var registro = await _repository.FindByIdAsync(id);
                if (registro is null)
                {
                    response.ErrorMessage = "No se encontro el registro";
                    response.Success = false;
                    return response;
                }

                response.Data = new TipoDtoResponse
                {
                    Id = registro.Id,
                    Name = registro.Name,
                    Status = registro.Status
                };

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(TipoDtoRequest request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var registro = new Tipo
                {
                    Name = request.Name,
                    Status = request.Status
                };

                response.Data = await _repository.AddAsync(registro);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al agregar";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, TipoDtoRequest request)
        {
            var response = new BaseResponse();
            try
            {
                var registro = await _repository.FindByIdAsync(id);
                if (registro is null)
                {
                    response.ErrorMessage = "No se encontro el registro";
                    response.Success = false;
                    return response;
                }

                registro.Name = request.Name;
                registro.Status = request.Status;

                await _repository.UpdateAsync(registro);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al actualizar";
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
                response.ErrorMessage = "Error al eliminar";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }
    }
}
