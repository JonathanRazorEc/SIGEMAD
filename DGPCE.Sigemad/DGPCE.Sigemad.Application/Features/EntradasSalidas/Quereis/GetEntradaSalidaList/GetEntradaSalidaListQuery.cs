using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.EntradasSalidas.Quereis.GetEntradaSalidaList;
public class GetEntradaSalidaListQuery : IRequest<IReadOnlyList<EntradaSalida>>
{
}
