using DGPCE.Sigemad.API.Models.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.ActuacionesRelevantes;
using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
using DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands;
using DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Commands.DeleteActuacionByIdRegistroActualizacion;
using DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Queries;
using DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
using DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
using DGPCE.Sigemad.Application.Features.MovilizacionMedios.Commands.ManageMovilizacionMedios;
using DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Text.RegularExpressions;


namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/actuaciones-relevantes")]
[ApiController]
public class ActuacionesRelevantesDGPCEController : ControllerBase
{
    private readonly IMediator _mediator;
    public ActuacionesRelevantesDGPCEController(IMediator mediator)
    {
        _mediator = mediator;

    }

    [HttpDelete("{idRegistroActualizacion:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Eliminar actuación relevante por id")]
    public async Task<ActionResult> Delete(int idRegistroActualizacion)
    {
        //var command = new DeleteActuacionRelevanteCommand { Id = id };
        var command = new DeleteActuacionByIdRegistroActualizacionCommand { IdRegistroActualizacion = idRegistroActualizacion };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("emergencia-nacional")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageEmergenciaNacionalResponse>> Create([FromBody] ManageEmergenciasNacionalesCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }




    [HttpGet]
    public async Task<ActionResult<ActuacionRelevanteDGPCEDto>> GetEvolucion(
    [FromQuery] int? idRegistroActualizacion,
    [FromQuery] int idSuceso)
    {
        var query = new GetActuacionRelevanteQuery
        {
            IdRegistroActualizacion = idRegistroActualizacion,
            IdSuceso = idSuceso
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("declaraciones-zagep/lista")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageDeclaracionZAGEPResponse>> CreateDeclaracionZAGEP([FromBody] ManageDeclaracionesZAGEPCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }


    [HttpPost("convocatoria-cecod/lista")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageDeclaracionZAGEPResponse>> CreateConvocatoriaCECOD([FromBody] ManageConvocatoriaCECODCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("movilizacion-medios/lista")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageMovilizacionMediosResponse>> Create([FromForm] string data)
    {
        // Deserializar el JSON recibido en el campo "data"
        var command = JsonConvert.DeserializeObject<ManageMovilizacionMediosCommand>(data);

        // Obtener los archivos enviados
        var archivos = HttpContext.Request.Form.Files;

        // Expresión regular para extraer índices
        var regex = new Regex(@"Movilizaciones\[(\d+)\]\.Pasos\[(\d+)\]\.Archivo");

        foreach (var archivo in archivos)
        {
            var match = regex.Match(archivo.Name);
            if (match.Success)
            {
                // Extraer índices de la movilización y del paso
                var movilizacionIndex = int.Parse(match.Groups[1].Value);
                var pasoIndex = int.Parse(match.Groups[2].Value);

                // Validar que los índices sean válidos
                if (movilizacionIndex < command.Movilizaciones.Count &&
                    pasoIndex < command.Movilizaciones[movilizacionIndex].Pasos.Count)
                {
                    // Obtener el paso correspondiente
                    var paso = command.Movilizaciones[movilizacionIndex].Pasos[pasoIndex];

                    // Mapear el archivo al paso
                    if (paso is ManageSolicitudMedioDto solicitudPaso)
                    {
                        //solicitudPaso.Archivo = archivo; // Asignar el archivo
                        using (var memoryStream = new MemoryStream())
                        {
                            await archivo.CopyToAsync(memoryStream); // Copia el contenido al MemoryStream
                            solicitudPaso.Archivo = new FileDto
                            {
                                Extension = Path.GetExtension(archivo.FileName),
                                Length = archivo.Length,
                                FileName = archivo.FileName,
                                ContentType = archivo.ContentType,
                                Content = memoryStream.ToArray() // Convierte el contenido a un arreglo de bytes
                            };
                        }
                    }
                }
            }
        }

        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("notificaciones/lista")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageNotificacionEmergenciaResponse>> CreateNotificacionesEmergencia([FromBody] ManageNotificacionEmergenciaCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

}
