namespace DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePuertos;
public interface IOpePuertoService
{
    Task ValidarRegistrosDuplicados(int? id, string nombre, DateTime fechaValidezDesde, DateTime? fechaValidezHasta);
}