using VintageStore.Domain;
using VintageStore.Services.Interfaces;

namespace VintageStore.Api.Endpoints
{
    public static class HomeEndpoints
    {
        public static void MapHomeEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/Home", async (IProductoService productoService, ITipoService TipoService) =>
            {
                var productos = await productoService.ListAsync(string.Empty, 1, 100);
                var tiposs = await TipoService.ListAsync();

                return Results.Ok(new
                {
                    productos = productos.Data,
                    Tipos = tiposs.Data,
                    Success = true
                });
            }).WithDescription("Permite mostrar los endpoints de la pagina principal")
            .WithOpenApi();
        }
    }
}
