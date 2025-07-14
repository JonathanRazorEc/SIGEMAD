using DGPCE.Sigemad.Application.Behaviours;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpeLineasMaritimas;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePeriodos;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePorcentsOcAE;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosAsistencias;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetRegistrosPorSuceso;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DGPCE.Sigemad.Application;

public static class ApplicationServiceRegistration
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        // Registrar GetRegistrosPorSucesoQueryHandler
        services.AddTransient<GetRegistrosPorSucesoQueryHandler>();

        services.AddTransient<IRegistroActualizacionService, RegistroActualizacionService>();

        // OPE - ADMINISTRACIÓN
        services.AddTransient<IOpePeriodoService, OpePeriodoService>();
        services.AddTransient<IOpePuertoService, OpePuertoService>();
        services.AddTransient<IOpeLineaMaritimaService, OpeLineaMaritimaService>();
        services.AddTransient<IOpePorcentOcAEService, OpePorcentOcAEService>();
        // OPE - DATOS
        services.AddTransient<IOpeDatoFronteraService, OpeDatoFronteraService>();
        services.AddTransient<IOpeDatoEmbarqueDiarioService, OpeDatoEmbarqueDiarioService>();
        services.AddTransient<IOpeDatoAsistenciaService, OpeDatoAsistenciaService>();
        // FIN OPE


        return services;
    }

}
