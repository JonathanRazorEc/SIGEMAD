using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorByIdMunicipioList;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipioByIdProvincia;
using DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipiosBySearch;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class MunicipiosController : ControllerBase
{
    private readonly IMediator _mediator;
    public MunicipiosController(IMediator mediator)
    {
        _mediator = mediator;

    }


    [HttpGet]
    [Route("{idProvincia}")]
    [Obsolete("Este método está obsoleto. Utiliza la versión v2 (/api/v2/Municipios/Busqueda) para obtener el listado de municipios según los parámetros de búsqueda opcionales.")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de los municipios para una determinada provincia")]
    public async Task<ActionResult<IReadOnlyList<Municipio>>> GetMunicipiosByIdProvincia(int idProvincia)
    {
        var query = new GetMunicipioByIdProvinciaQuery(idProvincia);
        var listado = await _mediator.Send(query);

        if (listado.Count == 0)
            return NotFound();

        return Ok(listado);
    }


    [HttpGet("{IdMunicipio}/entidades-menores")]
    [ProducesResponseType(typeof(IReadOnlyList<EntidadMenorVm>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas la lista general de tipos de entidad menor por Id de municipio")]

    public async Task<IReadOnlyList<EntidadMenorVm>> GetEntidadesMenoresByIdMunicipio(int IdMunicipio)
    {
        var query = new GetEntidadMenorByIdMunicipioListQuery(IdMunicipio);
        var result = await _mediator.Send(query);
        return result;
    }
}
