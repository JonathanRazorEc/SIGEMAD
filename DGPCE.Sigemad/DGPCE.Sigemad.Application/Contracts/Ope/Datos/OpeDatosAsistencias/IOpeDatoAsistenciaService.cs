namespace DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosAsistencias;
public interface IOpeDatoAsistenciaService
{
    Task ValidarRegistrosDuplicados(int? id, int idOpePuerto, DateTime fecha);
}