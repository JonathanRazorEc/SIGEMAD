using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Queries.GetOpeLineaMaritimaById
{
    public class GetOpeLineaMaritimaByIdQuery : IRequest<OpeLineaMaritima>
    {
        public int Id { get; set; }

        public GetOpeLineaMaritimaByIdQuery(int id)
        {
            Id = id;
        }
    }
}
