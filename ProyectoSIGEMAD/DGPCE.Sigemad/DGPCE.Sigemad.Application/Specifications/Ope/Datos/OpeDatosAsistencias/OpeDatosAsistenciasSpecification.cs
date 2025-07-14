using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Vms;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosAsistencias;

public class OpeDatosAsistenciasSpecification : BaseSpecification<OpeDatoAsistencia>
{
    public OpeDatosAsistenciasSpecification(OpeDatosAsistenciasSpecificationParams request)
        : base(opeDatoAsistencia =>
        (!request.Id.HasValue || opeDatoAsistencia.Id == request.Id) &&
        //&&
        //(!request.FechaHoraInicioIntervalo.HasValue || DateOnly.FromDateTime(opeDatoAsistencia.FechaHoraFinIntervalo) >= request.FechaHoraInicioIntervalo) &&
        //    (!request.FechaHoraFinIntervalo.HasValue || DateOnly.FromDateTime(opeDatoAsistencia.FechaHoraFinIntervalo) <= request.FechaHoraFinIntervalo) &&
        //(!request.IdOpePuerto.HasValue || opeDatoAsistencia.IdOpePuerto == request.IdOpePuerto) &&
        (request.IdsOpePuertos == null || request.IdsOpePuertos.Contains(opeDatoAsistencia.IdOpePuerto)) &&

        //
        (!request.IdOpeFase.HasValue || opeDatoAsistencia.OpePuerto.OpeFase.Id == request.IdOpeFase) &&
        //

        (!request.FechaInicio.HasValue || DateOnly.FromDateTime(opeDatoAsistencia.Fecha) >= request.FechaInicio) &&
        (!request.FechaFin.HasValue || DateOnly.FromDateTime(opeDatoAsistencia.Fecha) <= request.FechaFin)

        && opeDatoAsistencia.Borrado != true
        )
    {
        AddInclude(i => i.OpePuerto);
        AddInclude(i => i.OpeDatosAsistenciasSanitarias.Where(d => !d.Borrado));
        AddInclude("OpeDatosAsistenciasSanitarias.OpeAsistenciaSanitariaTipo");
        AddInclude(i => i.OpeDatosAsistenciasSociales.Where(d => !d.Borrado));
        AddInclude("OpeDatosAsistenciasSociales.OpeAsistenciaSocialTipo");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesTareas");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesTareas.OpeAsistenciaSocialTareaTipo");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesOrganismos");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesOrganismos.OpeAsistenciaSocialOrganismoTipo");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesUsuarios");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesUsuarios.OpeAsistenciaSocialEdad");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesUsuarios.OpeAsistenciaSocialSexo");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesUsuarios.OpeAsistenciaSocialNacionalidad");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesUsuarios.PaisResidencia");
        AddInclude(i => i.OpeDatosAsistenciasTraducciones.Where(d => !d.Borrado));
 
        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.Fecha);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.Fecha);
                    break;
                case "denominacionasc":
                    //AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    //AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    AddOrderByDescending(i => i.Fecha); // Orden por defecto
                    break;
            }
        }
    }
}
