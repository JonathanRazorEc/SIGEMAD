namespace DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpeLineasMaritimas;
public interface IOpeLineaMaritimaService
{
    Task ValidarRegistrosDuplicados(int? id, int idOpePuertoOrigen, int idOpePuertoDestino, DateTime fechaValidezDesde, DateTime? fechaValidezHasta);
}