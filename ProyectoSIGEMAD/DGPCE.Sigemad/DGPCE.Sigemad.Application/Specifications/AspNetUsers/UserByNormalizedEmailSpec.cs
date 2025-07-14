using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DGPCE.Sigemad.Domain.Modelos;
namespace DGPCE.Sigemad.Application.Specifications.AspNetUsers
{
    class UserByNormalizedEmailSpec:BaseSpecification<AspNetUser>
    {
        public UserByNormalizedEmailSpec(string normalizedEmail)
        : base(u => u.NormalizedEmail == normalizedEmail) { }
    }
}
