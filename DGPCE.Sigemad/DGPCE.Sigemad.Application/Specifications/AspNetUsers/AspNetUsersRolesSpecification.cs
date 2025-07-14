using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;

public class AspNetUsersRolesByUserIdsSpec : BaseSpecification<AspNetUserRol>
{

    /// <summary>
    /// Paginación genérica sobre AspNetUserRol
    /// </summary>
    /// 
    public AspNetUsersRolesByUserIdsSpec(IEnumerable<string> userIds)
        : base(r => userIds.Contains(r.UserId))
    {
    }
}
