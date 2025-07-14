using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.AspNetUsers
{
    public class AspNetUserByIdSpec : BaseSpecification<AspNetUser>
    {
        public AspNetUserByIdSpec(string id)
            : base(user => user.Id == id)
        {
        }
    }

}
