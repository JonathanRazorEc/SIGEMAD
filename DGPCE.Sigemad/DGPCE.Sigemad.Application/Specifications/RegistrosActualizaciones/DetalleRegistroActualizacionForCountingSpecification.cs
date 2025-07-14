using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class DetalleRegistroActualizacionForCountingSpecification : BaseSpecification<DetalleRegistroActualizacion>
{
    private static readonly List<int> idsApartadoNoPermitido = new List<int> { 1, 2, 3 };
    private static readonly List<int> idsEstadosPermitidos = new List<int> {
        (int)EstadoRegistroEnum.Creado,
        (int)EstadoRegistroEnum.Modificado,
        (int)EstadoRegistroEnum.CreadoYModificado,
        (int)EstadoRegistroEnum.CreadoYEliminado,
        (int)EstadoRegistroEnum.Eliminado,
    };

    public DetalleRegistroActualizacionForCountingSpecification(DetalleRegistroActualizacionParams @params)
        : base(d =>
        d.Borrado == false &&
        d.RegistroActualizacion.IdSuceso == @params.IdSuceso &&
        !idsApartadoNoPermitido.Contains(d.IdApartadoRegistro) &&
        idsEstadosPermitidos.Contains((int)d.IdEstadoRegistro)
        )
    {
        AddOrderByDescending(r => r.FechaCreacion);
    }
}
