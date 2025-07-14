namespace DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePorcentsOcAE;
public interface IOpePorcentOcAEService
{
    Task ValidarRegistrosDuplicados(int? id, int idOpeOcupacion);
}