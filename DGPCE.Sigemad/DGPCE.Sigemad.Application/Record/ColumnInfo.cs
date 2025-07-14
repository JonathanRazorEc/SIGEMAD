using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Record
{
    public record ColumnInfo(
    string Columna,
    string Etiqueta,
    bool Nulo,
    bool Duplicado,
    string Tipo,
    string? TablaRelacionada,
    string? CampoRelacionado);

}
