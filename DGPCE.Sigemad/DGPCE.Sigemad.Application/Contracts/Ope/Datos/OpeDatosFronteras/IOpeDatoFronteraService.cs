namespace DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosFronteras;
public interface IOpeDatoFronteraService
{
    Task ValidarHoraInicioMenorHoraFinIntervaloHorarioPersonalizado(TimeSpan? inicio, TimeSpan? fin);
    Task ValidarHoraDentroIntervaloHorario(TimeSpan? hora, int idOpeDatoFronteraIntervaloHorario);
    Task ValidarRegistrosDuplicados(int? id, int idOpeFrontera, DateTime fecha, int idOpeDatoFronteraIntervaloHorario, TimeSpan horaInicio, TimeSpan horaFin);
}