using DGPCE.Sigemad.Domain.Enums;
using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.UpdateOpePeriodos;

public class UpdateOpePeriodoCommand : IRequest
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public int IdOpePeriodoTipo { get; set; }

    public DateTime FechaInicioFaseSalida { get; set; }
    public DateTime FechaFinFaseSalida { get; set; }

    public DateTime FechaInicioFaseRetorno { get; set; }
    public DateTime FechaFinFaseRetorno { get; set; }

}
