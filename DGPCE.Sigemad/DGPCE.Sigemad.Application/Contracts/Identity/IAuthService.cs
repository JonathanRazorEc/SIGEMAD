using DGPCE.Sigemad.Application.Models.Identity;

namespace DGPCE.Sigemad.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(AuthRequest request);
        Task<RegistrationResponse> Register(RegistrationRequest request);
        Task<AuthResponse> RefreshToken(TokenRequest request);
        Guid GetCurrentUserId();
    }
}
