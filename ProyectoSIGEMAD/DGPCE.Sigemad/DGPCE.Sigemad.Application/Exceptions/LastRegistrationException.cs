using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Exceptions
{ 

    public class LastRegistrationException : ApplicationException
    {
        public LastRegistrationException(string name, object key) : base($"Entity '{name}' con clave ({key})  no es el ultimo registro")
        {
        }
    }
}
