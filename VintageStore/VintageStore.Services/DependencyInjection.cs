using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Repositories.Implementations;
using VintageStore.Repositories.Interfaces;
using VintageStore.Services.Implementations;
using VintageStore.Services.Interfaces;

namespace VintageStore.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddTransient<ITipoService, TipoService>()
                .AddTransient<IProductoService, ProductoService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IVentaService, VentaService>()
                .AddTransient<IEmailService, EmailService>();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddTransient<ITipoRepository, TipoRepository>()
                .AddTransient<IProductoRepository, ProductoRepository>()
                .AddTransient<IVentaRepository, VentaRepository>()
                .AddTransient<IClienteRepository, ClienteRepository>();
        }

        public static IServiceCollection AddUploader(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetSection("StorageConfiguration:Path").Value!.Contains("core.windows.net"))
            {
                //services.AddTransient<IFileUploader, AzureBlobStorageUploader>();
            }
            else
                services.AddTransient<IFileUploader, FileUploader>();

            return services;
        }

    }
}
