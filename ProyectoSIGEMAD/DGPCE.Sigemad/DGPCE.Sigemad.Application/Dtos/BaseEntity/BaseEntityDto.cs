using DGPCE.Sigemad.Application.Dtos.AspNetUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.BaseEntity;
public class BaseEntityDto
{
    public DateTime FechaCreacion { get; set; }
    public virtual AspnetUsersDto CreadoPorNavigation { get; set; } = null!;
    public DateTime? FechaModificacion { get; set; }
    public virtual AspnetUsersDto ModificadoPorNavigation { get; set; } = null!;
    public DateTime? FechaEliminacion { get; set; }
    public virtual AspnetUsersDto EliminadoPorNavigation { get; set; } = null!;
    public bool Borrado { get; set; } = false;
}
