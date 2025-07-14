namespace DGPCE.Sigemad.Application.Models.Identity;

public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public double DurationInMinutes { get; set; }
    public TimeSpan TokenExpireTime { get; set; }
    public TimeSpan RefreshTokenExpireTime { get; set; }
}
