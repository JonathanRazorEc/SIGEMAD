using AutoMapper;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Mappings.Resolvers;
public class CustomDetalleIntervencionMedioResolver
    : IValueResolver<CreateOrUpdateIntervencionMedioDto, IntervencionMedio, List<DetalleIntervencionMedio>>
{
    public List<DetalleIntervencionMedio> Resolve(
        CreateOrUpdateIntervencionMedioDto source,
        IntervencionMedio destination,
        List<DetalleIntervencionMedio> destMember,
        ResolutionContext context)
    {
        // Get existing list (if null, initialize)
        var existingDetails = destination.DetalleIntervencionMedios.ToList() ?? new List<DetalleIntervencionMedio>();

        foreach (var detalleDto in source.DetalleIntervencionMedios)
        {
            var detalleExistente = existingDetails
                .FirstOrDefault(d => d.IdMediosCapacidad == detalleDto.IdMediosCapacidad);

            if (detalleExistente != null)
            {
                //Update only necessary fields
                detalleExistente.NumeroIntervinientes = detalleDto.NumeroIntervinientes;    
            }
            else
            {
                // Add new records
                var newDetalle = context.Mapper.Map<DetalleIntervencionMedio>(detalleDto);
                existingDetails.Add(newDetalle);
            }
        }

        // Remove old details that are no longer in the DTO
        var idsDetallesDto = source.DetalleIntervencionMedios.Select(d => d.IdMediosCapacidad).ToList();
        var detallesParaEliminar = existingDetails
            .Where(d => !idsDetallesDto.Contains(d.IdMediosCapacidad) && d.Borrado == false)
            .ToList();

        foreach (var detalle in detallesParaEliminar)
        {
            existingDetails.Remove(detalle);
        }

        return existingDetails;
    }
}

