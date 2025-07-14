using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Helpers;
public static class IncendioUtils
{
    public static string ObtenerUbicacion(Incendio item)
    {
        if (item == null)
            return "Incendio no definido";

        switch (item.IdTerritorio)
        {
            case (int)TipoTerritorio.Nacional:
                {
                    // Si falta municipio o provincia ponemos un texto por defecto
                    var munDesc = item.Municipio?.Descripcion ?? "Municipio no asignado";
                    var provDesc = item.Provincia?.Descripcion ?? "Provincia no asignada";
                    return $"{munDesc} ({provDesc})";
                }

            case (int)TipoTerritorio.Extranjero:
            case (int)TipoTerritorio.Transfronterizo:
                {
                    // Nombre del país (o por defecto)
                    var paisDesc = item.Pais?.Descripcion ?? "País no asignado";

                    // Si es Francia o Portugal, intentamos usar MunicipioExtranjero
                    if ((item.IdPais == (int)PaisesEnum.Francia || item.IdPais == (int)PaisesEnum.Portugal)
                        && item.MunicipioExtranjero != null)
                    {
                        var extMun = item.MunicipioExtranjero.Descripcion ?? "Municipio ext. no asignado";
                        return $"{paisDesc} ({extMun})";
                    }

                    // En el resto de países usamos el campo Ubicacion, si está
                    var ubic = !string.IsNullOrWhiteSpace(item.Ubicacion)
                        ? item.Ubicacion
                        : "Ubicación no especificada";
                    return $"{paisDesc} ({ubic})";
                }

            default:
                // Territorio desconocido
                return "Territorio desconocido";
        }
    }

}
