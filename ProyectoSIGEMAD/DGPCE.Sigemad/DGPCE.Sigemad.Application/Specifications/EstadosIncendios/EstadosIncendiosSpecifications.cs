using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.EstadosIncendios;
public class EstadosIncendiosSpecifications : BaseSpecification<EstadoIncendio>
{
    public EstadosIncendiosSpecifications()
    : base(estadoIncendio =>!estadoIncendio.Borrado) 
    {
    }
}
