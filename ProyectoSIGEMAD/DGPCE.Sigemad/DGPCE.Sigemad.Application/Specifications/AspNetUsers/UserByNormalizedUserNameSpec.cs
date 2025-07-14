using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.AspNetUsers
{
    class UserByNormalizedUserNameSpec:BaseSpecification<AspNetUser>
    {
        public UserByNormalizedUserNameSpec(string normalizedUserName)
        : base(u => u.NormalizedUserName == normalizedUserName) { }
    }
}
