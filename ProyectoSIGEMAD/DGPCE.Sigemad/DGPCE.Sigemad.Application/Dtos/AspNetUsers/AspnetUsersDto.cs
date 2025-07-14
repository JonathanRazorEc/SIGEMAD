using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.AspNetUsers;
public class AspnetUsersDto
{
    public string Id { get; set; }
    public string? UserName { get; set; }
    public string? NormalizedUserName { get; set; }
    public string? Nombre { get; set; }
    public string? Apellidos { get; set; }
}
