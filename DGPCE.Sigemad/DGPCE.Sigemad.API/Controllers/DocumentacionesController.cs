using DGPCE.Sigemad.API.Models;
using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentacionByRegistro;
using DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentaciones;
using DGPCE.Sigemad.Application.Features.Documentaciones.Commands.ManageDocumentaciones;
using DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDocumentacion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;


namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class DocumentacionesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentacionesController(IMediator mediator)
    {
        _mediator = mediator;

    }

    [HttpPost("lista")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Crea la documentacion asociada a un incendio")]
    public async Task<ActionResult<CreateOrUpdateDocumentacionResponse>> Create([FromForm] ManageDocumentacionesRequest request)
    {
        // Mapear desde el modelo de API al command
        var command = new ManageDocumentacionesCommand
        {
            IdRegistroActualizacion = request.IdRegistroActualizacion,
            IdSuceso = request.IdSuceso
        };

        // Procesar cada detalle y su archivo
        foreach (var detalle in request.Detalles)
        {
            var detalleDto = new DetalleDocumentacionDto
            {
                Id = detalle.Id,
                FechaHora = detalle.FechaHora,
                FechaHoraSolicitud = detalle.FechaHoraSolicitud,
                IdTipoDocumento = detalle.IdTipoDocumento,
                IdsProcedenciasDestinos = detalle.IdsProcedenciasDestinos,
                Descripcion = detalle.Descripcion,
            };

            if (detalle.Archivo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await detalle.Archivo.CopyToAsync(memoryStream); // Copia el contenido al MemoryStream
                    detalleDto.Archivo = new FileDto
                    {
                        Extension = Path.GetExtension(detalle.Archivo.FileName),
                        Length = detalle.Archivo.Length,
                        FileName = detalle.Archivo.FileName,
                        ContentType = detalle.Archivo.ContentType,
                        Content = memoryStream.ToArray() // Convierte el contenido a un arreglo de bytes
                    };
                }
            }

            command.DetallesDocumentaciones.Add(detalleDto);
        }

        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<DocumentacionDto>> GetDocumentaciones(
    [FromQuery] int? idRegistroActualizacion,
    [FromQuery] int idSuceso)
    {
        var query = new GetDoumentacionQuery
        {
            IdRegistroActualizacion = idRegistroActualizacion,
            IdSuceso = idSuceso
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("{idRegistroActualizacion:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Elimina la documentacion por IdRegistroActualizacion")]
    public async Task<ActionResult> Delete(int idRegistroActualizacion)
    {
        //var command = new DeleteDocumentacionCommand { Id = id };
        var command = new DeleteDocumentacionByIdRegistroCommand{ IdRegistroActualizacion = idRegistroActualizacion };
        await _mediator.Send(command);
        return NoContent();
    }

}