namespace DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePeriodos;
public interface IOpePeriodoService
{
    Task ValidarRegistrosDuplicados(int? id, string nombre, DateTime FechaInicioFaseSalida, DateTime FechaFinFaseSalida,
        DateTime FechaInicioFaseRetorno, DateTime FechaFinFaseRetorno);
}