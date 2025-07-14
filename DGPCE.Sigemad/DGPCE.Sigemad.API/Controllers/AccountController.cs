using DGPCE.Sigemad.Application.Contracts.Identity;
using DGPCE.Sigemad.Application.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            return Ok(await _authService.Login(request));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest request)
        {
            return Ok(await _authService.Register(request));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] TokenRequest request)
        {

            AuthResponse response = await _authService.RefreshToken(request);

            if (response == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


    }
}
