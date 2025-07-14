using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.AspNetUsersRoles
{
    public class AspNetUserRolesByUserSpec:BaseSpecification<AspNetUserRol>
    {
        public AspNetUserRolesByUserSpec(string userId)
            : base(r => r.UserId == userId)
        {
        }
    }
}
