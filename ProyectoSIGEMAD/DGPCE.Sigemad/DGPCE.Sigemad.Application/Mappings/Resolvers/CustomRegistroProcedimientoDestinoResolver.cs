using AutoMapper;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Mappings.Resolvers;
public class CustomRegistroProcedimientoDestinoResolver
    : IValueResolver<CreateRegistroCommand, Registro, List<RegistroProcedenciaDestino>>
{
    public List<RegistroProcedenciaDestino> Resolve(
        CreateRegistroCommand source,
        Registro destination,
        List<RegistroProcedenciaDestino> destMember,
        ResolutionContext context)
    {
        List<RegistroProcedenciaDestino> existingProcedenciaDestinos =
            destination.ProcedenciaDestinos.ToList() ?? new();

        foreach (int IdProcedimientoDto in source.RegistroProcedenciasDestinos)
        {
            RegistroProcedenciaDestino? detalleExistente = existingProcedenciaDestinos
                .FirstOrDefault(d => d.IdProcedenciaDestino == IdProcedimientoDto);

            if (detalleExistente is null)
            {
                // Add new records
                RegistroProcedenciaDestino newDetalle = new()
                {
                    IdProcedenciaDestino = IdProcedimientoDto
                };
                existingProcedenciaDestinos.Add(newDetalle);
            }
            else
            {
                //Update only necessary fields
                detalleExistente.IdProcedenciaDestino = IdProcedimientoDto;
            }
        }

        // Remove old details that are no longer in the DTO
        List<int> idsProcedenciaDestinosDto = source.RegistroProcedenciasDestinos;
        List<RegistroProcedenciaDestino> detallesParaEliminar = existingProcedenciaDestinos
                        .Where(d => !idsProcedenciaDestinosDto.Contains(d.IdProcedenciaDestino) && d.Borrado == false)
                        .ToList();

        existingProcedenciaDestinos.RemoveAll(d => detallesParaEliminar.Contains(d));

        return existingProcedenciaDestinos;
    }
}
