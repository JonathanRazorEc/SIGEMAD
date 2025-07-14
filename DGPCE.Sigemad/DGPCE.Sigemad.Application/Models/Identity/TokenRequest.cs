using System.ComponentModel.DataAnnotations;

namespace DGPCE.Sigemad.Application.Models.Identity;

public class TokenRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string RefreshToken { get; set; }
}
