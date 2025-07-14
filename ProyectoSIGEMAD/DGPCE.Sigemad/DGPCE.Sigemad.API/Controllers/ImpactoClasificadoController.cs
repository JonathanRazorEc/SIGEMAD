using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetImpactoClasificadoList;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Application.Features.TiposImpactos.Queries;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Queries.GetCamposImpactosById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/")]
public class ImpactoClasificadoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImpactoClasificadoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //[HttpGet("tipos-impactos")]
    //[ProducesResponseType((int)HttpStatusCode.OK)]
    //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    //[SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene los tipos de impactos clasificados")]
    //public async Task<ActionResult<IReadOnlyList<string>>> GetTiposImpactos()
    //{
    //    var query = new GetTiposImpactosListQuery();
    //    var result = await _mediator.Send(query);
    //    return Ok(result);
    //}

    //[HttpGet("grupos-impactos")]
    //[ProducesResponseType((int)HttpStatusCode.OK)]
    //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    //[SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene los grupos de impactos clasificados")]
    //public async Task<ActionResult<IReadOnlyList<string>>> GetGruposImpactos(GetGruposImpactosListQuery query)
    //{
    //    var result = await _mediator.Send(query);
    //    return Ok(result);
    //}



    [HttpGet("impactos-clasificados")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de los impactos clasificados")]
    public async Task<ActionResult<IReadOnlyList<ImpactoClasificadoConTipoImpactoVM>>> GetImpactoClasificado([FromQuery]int? idTipoImpacto, bool? nuclear,string? busqueda)
    {
        var query = new GetImpactoClasificadoListQuery(idTipoImpacto, nuclear, busqueda);
        var listado = await _mediator.Send(query);

        return Ok(listado);
    }


    [HttpGet("tipo-Impacto")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de los tipo impactos")]
    public async Task<ActionResult<IReadOnlyList<ImpactoClasificadoDescripcionVm>>> GetTipoImpacto([FromQuery] bool nuclear)
    {
        var query = new GetTipoImpactoListQuery(nuclear);
        var listado = await _mediator.Send(query);

        if (listado.Count == 0)
            return NotFound();

        return Ok(listado);
    }




    [HttpGet("Validacion-impacto-clasificado/{idImpactoClasificado}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de campos necesarios para el formulario a mostrar")]
    public async Task<ActionResult<IReadOnlyList<ImpactoClasificadoDescripcionVm>>> GetValidacionesByidImpactoClasificado(int idImpactoClasificado)
    {
        var query = new GetCamposImpactosByIdQuery(idImpactoClasificado);
        var listado = await _mediator.Send(query);


        return Ok(listado);
    }

}
