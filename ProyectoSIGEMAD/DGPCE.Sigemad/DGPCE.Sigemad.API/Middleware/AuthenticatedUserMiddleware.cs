using DGPCE.Sigemad.Application.Constants;
using System.Security.Claims;

namespace DGPCE.Sigemad.API.Middleware;

public class AuthenticatedUserMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticatedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null)
                {
                    context.Items[HttpContextItems.UserId] = idClaim.Value;
                }
            }
        }

        await _next(context);
    }
}
