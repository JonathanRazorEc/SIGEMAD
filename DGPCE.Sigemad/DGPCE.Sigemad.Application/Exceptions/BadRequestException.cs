using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message, Microsoft.EntityFrameworkCore.DbUpdateException dbEx) : base(message)
        {
        }

        // nuevo overload
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
