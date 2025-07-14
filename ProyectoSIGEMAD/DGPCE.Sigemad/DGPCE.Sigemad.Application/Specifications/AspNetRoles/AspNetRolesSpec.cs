

using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.AspNetRoles
{
    /// <summary>Devuelve todos los roles; sin filtros.</summary>
    public sealed class AspNetRolesSpec : BaseSpecification<AspNetRole>
    {
    }

    /// <summary>Filtra por lista de Ids (case–insensitive).</summary>
    public sealed class AspNetRolesByIdsSpec : BaseSpecification<AspNetRole>
    {
        public AspNetRolesByIdsSpec(IEnumerable<string> ids)
        {
            var normalized = ids
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.ToUpperInvariant())
                .ToList();

            //  👇  Ahora sí tienes el builder 'Query'
            AddCriteria(r => normalized.Contains(r.Id.ToUpper()));
        }
    }
}