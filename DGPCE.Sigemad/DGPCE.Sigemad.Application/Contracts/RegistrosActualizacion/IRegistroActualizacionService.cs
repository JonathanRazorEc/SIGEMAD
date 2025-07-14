using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
public interface IRegistroActualizacionService
{
    Task ValidarSuceso(int idSuceso);
    Task<RegistroActualizacion> GetOrCreateRegistroActualizacion<T>(int? idRegistroActualizacion, int idSuceso, TipoRegistroActualizacionEnum tipoRegistro) where T : BaseDomainModel<int>;

    Task SaveRegistroActualizacion<T, E, G>(
        RegistroActualizacion registroActualizacion,
        T entidad,
        ApartadoRegistroEnum apartadoRegistro,
        List<int> referenciasParaEliminar,
        Dictionary<int, G> entidadesOriginales) 
        where T : BaseDomainModel<int>
        where E : BaseDomainModel<int>
        where G : class;

    Task ReflectNewRegistrosInFuture<T>(
        RegistroActualizacion registroActualizacion,
        ApartadoRegistroEnum apartadoRegistro,
        List<int> nuevasReferenciasIds) where T : BaseDomainModel<int>;
}