using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.AspNetUsers
{
    public class AspNetUsersRolesSpec : BaseSpecification<AspNetUserRol>
    {
        // ctor sin parámetros  (CountAsync / traer todo)
        public AspNetUsersRolesSpec() { }

        // ctor con paginación
        public AspNetUsersRolesSpec(int skip, int take)
        {
            ApplyPaging(skip, take);
        }
    }
}