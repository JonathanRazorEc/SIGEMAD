using DGPCE.Sigemad.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGPCE.Sigemad.Domain.Modelos { 
public class AspNetUserRol
{
      public string UserId { get; set; }

      public string? RoleId { get; set; }
   }
}
