namespace DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosEmbarquesDiarios;
public interface IOpeDatoEmbarqueDiarioService
{
    Task ValidarRegistrosDuplicados(int? id, int idOpeLineaMaritima, DateTime fecha);
}