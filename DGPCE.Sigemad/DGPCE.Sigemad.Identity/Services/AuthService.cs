using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Application.Contracts.Identity;
using DGPCE.Sigemad.Application.Models.Identity;
using DGPCE.Sigemad.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DGPCE.Sigemad.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly SigemadIdentityDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Guid SystemUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");


        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<JwtSettings> jwtSettings,
            SigemadIdentityDbContext cleanArchitectureDbContext,
            TokenValidationParameters tokenValidationParameters,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _context = cleanArchitectureDbContext;
            _tokenValidationParameters = tokenValidationParameters;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new Exception($"El usuario con email {request.Email} no existe");
            }

            var resultado = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!resultado.Succeeded)
            {
                throw new Exception($"Las credenciales son incorrectas");
            }

            var token = await GenerateToken(user);
            var authResponse = new AuthResponse
            {
                Id = user.Id,
                Token = token.Item1,
                Email = user.Email,
                Username = user.UserName,
                RefreshToken = token.Item2,
                Success = true
            };

            return authResponse;
        }

        public async Task<AuthResponse> RefreshToken(TokenRequest request)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // 1️⃣ Primero, verifica si el Refresh Token existe en la base de datos
                var storedToken = await _context.RefreshTokens!.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);
                if (storedToken is null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Errors = new List<string> { "El Refresh Token no existe" }
                    };
                }

                // 2️⃣ Verificar si el Refresh Token ya expiró
                if (storedToken.ExpireDate < DateTime.UtcNow)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Errors = new List<string> { "El Refresh Token ha expirado" }
                    };
                }

                // 3️⃣ Validar que el Refresh Token no haya sido revocado o ya usado
                if (storedToken.IsUsed)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Errors = new List<string> { "El Refresh Token ya fue usado" }
                    };
                }

                if (storedToken.IsRevoked)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Errors = new List<string> { "El Refresh Token ha sido revocado" }
                    };
                }

                // 4️⃣ Intentar validar el Access Token SIN verificar expiración
                var tokenValidationParamsClone = _tokenValidationParameters.Clone();
                tokenValidationParamsClone.ValidateLifetime = false; // No validar expiración aquí

                var tokenVerification = jwtTokenHandler.ValidateToken(
                    request.Token, tokenValidationParamsClone, out var validatedToken);

                // 5️⃣ Verificar si el token es de tipo JWT
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return new AuthResponse
                        {
                            Success = false,
                            Errors = new List<string> { "El Access Token tiene errores de encriptación" }
                        };
                    }
                }

                // 6️⃣ Verificar que el ID del Access Token coincida con el Refresh Token almacenado
                var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                if (storedToken.JwtId != jti)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Errors = new List<string> { "El Token no concuerda con el valor inicial" }
                    };
                }

                // 7️⃣ Marcar el Refresh Token como usado y guardar cambios
                storedToken.IsUsed = true;
                _context.RefreshTokens!.Update(storedToken);
                await _context.SaveChangesAsync();

                // 8️⃣ Generar nuevo Access Token y Refresh Token
                var user = await _userManager.FindByIdAsync(storedToken.UserId);
                var token = await GenerateToken(user);

                return new AuthResponse
                {
                    Id = user.Id,
                    Token = token.Item1,
                    Email = user.Email,
                    Username = user.UserName,
                    RefreshToken = token.Item2,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Errors = new List<string> { "Error procesando el refresh token: " + ex.Message }
                };
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeval = dateTimeval.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTimeval;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            // 1️⃣ Validación de unicidad de username y email
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
                throw new Exception($"El username '{request.Username}' ya fue tomado por otra cuenta");

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
                throw new Exception($"El email '{request.Email}' ya fue tomado por otra cuenta");

            // 2️⃣ Creación del usuario en AspNetUsers (IdentityUser)
            var user = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new Exception($"Error creando el usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            // 3️⃣ Persistir en ApplicationUsers (tabla de dominio)
            var applicationUser = new DGPCE.Sigemad.Domain.Modelos.ApplicationUser
            {
                IdentityId = user.Id,
                Nombre = request.Nombre,
                Apellidos = request.Apellidos,
                Email = request.Email,
                Telefono = request.Telefono
            };
            _context.ApplicationUsers.Add(applicationUser);
            await _context.SaveChangesAsync();  // Guarda ApplicationUser

            // 4️⃣ Sincronizar los campos de perfil en AspNetUsers
            //    (asumiendo que tu DbContext expone un DbSet<AspNetUser> llamado AspNetUsers)
            var aspNetUser = await _context.Set<DGPCE.Sigemad.Domain.Modelos.AspNetUser>()
                                           .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (aspNetUser != null)
            {
                aspNetUser.Nombre = request.Nombre;
                aspNetUser.Apellidos = request.Apellidos;
                aspNetUser.Email = request.Email;
                aspNetUser.PhoneNumber = request.Telefono;
                _context.Update(aspNetUser);
                await _context.SaveChangesAsync();  // Guarda AspNetUser
            }

            // 5️⃣ Generamos tokens y devolvemos respuesta
            var token = await GenerateToken(user);
            return new RegistrationResponse
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Token = token.Item1,
                RefreshToken = token.Item2
            };
        }



        private async Task<Tuple<string, string>> GenerateToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));

            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);


            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var appUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.IdentityId == (user.Id));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(CustomClaimTypes.Id, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }.Union(userClaims).Union(roleClaims)),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenExpireTime),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenExpireTime),
                Token = $"{GenerateRandomTokenCharacters(35)}{Guid.NewGuid()}"
            };

            await _context.RefreshTokens!.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new Tuple<string, string>(jwtToken, refreshToken.Token);
        }

        private string GenerateRandomTokenCharacters(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
        }

        public Guid GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.Items[HttpContextItems.UserId]?.ToString();

            return string.IsNullOrWhiteSpace(userId) ? SystemUserId : Guid.Parse(userId);
        }
    }
}
