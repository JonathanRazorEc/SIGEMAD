using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Infrastructure.Caching;
using DGPCE.Sigemad.Infrastructure.Options;
using DGPCE.Sigemad.Infrastructure.Persistence;
using DGPCE.Sigemad.Infrastructure.Repositories;
using DGPCE.Sigemad.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuracion de cache
        services.AddMemoryCache();
        services.AddScoped<ISIGEMemoryCache, SIGEMemoryCache>();

        // Configuración de la base de datos
        DatabaseSettings? dbSettings = configuration.GetSection("DatabaseSettings")
            .Get<DatabaseSettings>();

        services.AddDbContext<SigemadDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString"),
            options => options.UseNetTopologySuite())
            .EnableSensitiveDataLogging(dbSettings.EnableSensitiveDataLogging) // Incluye datos sensibles en los logs (¡solo para desarrollo!)
            .EnableDetailedErrors(dbSettings.EnableDetailedErrors) // Habilita errores detallados
        );

        services.AddSingleton<GeometryFactory>(NetTopologySuite.Geometries.GeometryFactory.Default);

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IGeometryValidator, GeometryValidator>();
        services.AddScoped<ICoordinateTransformationService, CoordinateTransformationService>();
        services.AddTransient<IFileService, LocalFileService>(provider => new LocalFileService(configuration));

        return services;
    }   

}
