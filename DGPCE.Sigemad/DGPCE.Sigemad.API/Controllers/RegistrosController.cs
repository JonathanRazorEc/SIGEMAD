using Azure.Core;
using DGPCE.Sigemad.API.Models.ActivacionesPlanes;
using DGPCE.Sigemad.API.Models.DireccionCoordinacion;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
using DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands;

using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.Parametros.Manage;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Features.Registros.CreateRegistros;
using DGPCE.Sigemad.Application.Features.Registros.DeleteRegistros;
using DGPCE.Sigemad.Application.Features.Registros.Queries;
using DGPCE.Sigemad.Domain.Constracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Runtime.Intrinsics.X86;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/registros")]
[ApiController]
public class RegistrosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFileService _fileService;
    private readonly ILogger<RegistrosController> _logger;
    private readonly IGeometryValidator _geometryValidator;


    public RegistrosController(IMediator mediator, IFileService fileService)
    {
        _mediator = mediator;
        _fileService = fileService;
    }

    [HttpGet("{idRegistroActualizacion}")]
    public async Task<ActionResult<RegistroEvolucionDto>> GetRegistro(int idRegistroActualizacion)
    {
        var query = new GetRegistrosQuery
        {
            IdRegistroActualizacion = idRegistroActualizacion
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost( Name = "CreateRegistro")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageRegistroResponse>> CreateRegistro([FromBody] CreateRegistroCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("areas-afectadas")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOrUpdateAreaAfectadaResponse>> CreateAreasAfectadas([FromBody] ManageAreaAfectadaCommand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }


    [HttpPost("Parametros")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageParametroResponse>> CreateParametros([FromBody] ManageParametroCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("impactos")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear lista de impactos de una evolucion (Consecuencia/Actuacion)")]
    public async Task<ActionResult<Application.Dtos.Impactos.ManageImpactoResponse>> CreateImpactos([FromBody] ManageImpactosCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete("{idRegistroActualizacion}")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Summary = "Eliminar registro por id Registro de actualizacion")]
    public async Task<ActionResult> Delete(int idRegistroActualizacion)
    {
        var command = new DeleteRegistroCommand { IdRegistroActualizacion = idRegistroActualizacion };
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("intervenciones")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear lista de intervenciones de una evolucion")]
    public async Task<ActionResult<ManageIntervencionMedioResponse>> CreateIntervenciones([FromBody] ManageIntervencionMedioCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    //-------------------------Procesar geoposition-------------------------------------------

    [HttpPost("direccion-coordinacion")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOrUpdateDireccionResponse>> CreateDirecciones([FromForm] DireccionCoordinacionRequest request)
    {
        var command = new CreateOrUpdateDireccionCoordinacionCommand
        {
            IdRegistroActualizacion = request.IdRegistroActualizacion,
            IdSuceso = request.IdSuceso
        };

        // Procesar direcciones (sin cambios)
        foreach (var direccion in request.Direcciones)
        {
            var newDireccion = new CreateOrUpdateDireccionDto
            {
                Id = direccion.Id,
                IdTipoDireccionEmergencia = direccion.IdTipoDireccionEmergencia,
                AutoridadQueDirige = direccion.AutoridadQueDirige,
                FechaInicio = direccion.FechaInicio,
                FechaFin = direccion.FechaFin,
                Archivo = await _fileService.ProcesarArchivoRequest(direccion.Archivo),
                ActualizarFichero = direccion.ActualizarFichero
            };

            command.Direcciones.Add(newDireccion);
        }

        // Procesar CoordinacionesCECOPI con geolocalización
        foreach (var cecopi in request.CoordinacionesCECOPI)
        {
            var newCoordinacionCecopi = new CreateOrUpdateCoordinacionCecopiDto
            {
                Id = cecopi.Id,
                FechaInicio = cecopi.FechaInicio,
                FechaFin = cecopi.FechaFin,
                IdProvincia = cecopi.IdProvincia,
                IdMunicipio = cecopi.IdMunicipio,
                Lugar = cecopi.Lugar,
                Observaciones = cecopi.Observaciones,

                // Procesar GeoPosicion - AJUSTADO para FormData
                GeoPosicion = ProcessGeoJsonFromFormData(cecopi.GeoPosicionJson),

                Archivo = await _fileService.ProcesarArchivoRequest(cecopi.Archivo),
                ActualizarFichero = cecopi.ActualizarFichero
            };

            command.CoordinacionesCECOPI.Add(newCoordinacionCecopi);
        }

        // Procesar CoordinacionesPMA con geolocalización
        foreach (var pma in request.CoordinacionesPMA)
        {
            var newCoordinacionPMA = new CreateOrUpdateCoordinacionPmaDto
            {
                Id = pma.Id,
                FechaInicio = pma.FechaInicio,
                FechaFin = pma.FechaFin,
                IdProvincia = pma.IdProvincia,
                IdMunicipio = pma.IdMunicipio,
                Lugar = pma.Lugar,
                Observaciones = pma.Observaciones,

                // Procesar GeoPosicion - AJUSTADO para FormData
                GeoPosicion = ProcessGeoJsonFromFormData(pma.GeoPosicionJson),

                Archivo = await _fileService.ProcesarArchivoRequest(pma.Archivo),
                ActualizarFichero = pma.ActualizarFichero
            };

            command.CoordinacionesPMA.Add(newCoordinacionPMA);
        }

        var response = await _mediator.Send(command);
        return Ok(response);
    }

    // MÉTODO AJUSTADO: Para procesar JSON que viene como string en FormData
    private Geometry? ProcessGeoJsonFromFormData(string? geoJsonString)
    {
        try
        {
            if (string.IsNullOrEmpty(geoJsonString)) return null;

            // Parsear el JSON string
            using var document = System.Text.Json.JsonDocument.Parse(geoJsonString);
            var root = document.RootElement;

            // Extraer coordenadas
            if (root.TryGetProperty("coordinates", out var coordinatesElement) &&
                coordinatesElement.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                var coordinates = coordinatesElement.EnumerateArray().ToList();
                if (coordinates.Count >= 2)
                {
                    double longitude = coordinates[0].GetDouble();
                    double latitude = coordinates[1].GetDouble();

                    // Crear GeoJSON estándar
                    var standardGeoJson = new
                    {
                        type = "Point",
                        coordinates = new[] { longitude, latitude }
                    };

                    var geoJsonStandardString = System.Text.Json.JsonSerializer.Serialize(standardGeoJson);

                    // Usar tu servicio existente
                    return _geometryValidator.ConvertFromGeoJson(geoJsonStandardString);
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando GeoJSON desde FormData: {GeoJson}", geoJsonString);
            return null;
        }
    }




    //-------------------------------------------------------------------------------------------



    private Geometry? ProcessGeoJsonToGeometry(object geoJsonObject)
    {
        try
        {
            if (geoJsonObject == null) return null;

            string geoJsonString;

            // Convertir a string JSON si no lo es
            if (geoJsonObject is string str)
            {
                geoJsonString = str;
            }
            else
            {
                geoJsonString = System.Text.Json.JsonSerializer.Serialize(geoJsonObject);
            }

            // Usar tu servicio existente para convertir GeoJSON
            return _geometryValidator.ConvertFromGeoJson(geoJsonString);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error procesando GeoJSON: {GeoJson}", geoJsonObject);
            return null;
        }
    }



    //---------------------------------------------------------------------------------------------



    [HttpPost("activaciones-planes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Crear listado de activaciones de planes en Actuacion Relevante")]
    public async Task<ActionResult<ManageActivacionPlanEmergenciaResponse>> CreateActivacionPlan([FromForm] ManageActivacionPlanRequest request)
    {
        // Mapear desde el modelo de API al command
        var command = new ManageActivacionPlanEmergenciaCommand
        {
            IdRegistroActualizacion = request.IdRegistroActualizacion,
            IdSuceso = request.IdSuceso
        };

        // Procesar cada detalle y su archivo
        foreach (var detalle in request.ActivacionPlanes)
        {
            var detalleDto = new ManageActivacionPlanEmergenciaDto
            {
                Id = detalle.Id,
                IdTipoPlan = detalle.IdTipoPlan,
                IdPlanEmergencia = detalle.IdPlanEmergencia,
                TipoPlanPersonalizado = detalle.TipoPlanPersonalizado,
                PlanEmergenciaPersonalizado = detalle.PlanEmergenciaPersonalizado,
                FechaHoraInicio = detalle.FechaHoraInicio,
                FechaHoraFin = detalle.FechaHoraFin,
                Autoridad = detalle.Autoridad,
                Observaciones = detalle.Observaciones,
                Archivo = await _fileService.ProcesarArchivoRequest(detalle.Archivo),
                ActualizarFichero = detalle.ActualizarFichero
            };

            command.ActivacionesPlanes.Add(detalleDto);
        }

        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("activaciones-sistemas/lista")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageActivacionSistemaResponse>> Create([FromBody] ManageActivacionSistemaCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }


    // agregar método para procesar GeoJSON:
    /*private void ProcessGeoJsonFromFrontend(dynamic geoJson,
        CreateOrUpdateCoordinacionCecopiDto cecopi = null,
        CreateOrUpdateCoordinacionPmaDto pma = null)
    {
        if (geoJson?.geoPosicion != null)
        {
            var coordinates = geoJson.geoPosicion.coordinates;
            if (coordinates != null && coordinates.Count == 2)
            {
                double longitude = coordinates[0];
                double latitude = coordinates[1];

                // Crear Point geometry
                var point = new Point(longitude, latitude) { SRID = 4326 };

                if (cecopi != null)
                {
                    cecopi.GeoPosicion = point;
                }
                else if (pma != null)
                {
                    pma.GeoPosicion = point;
                }
            }
        }
    }*/

}
