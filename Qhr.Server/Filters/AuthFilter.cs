using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Qhr.Server.Services;

namespace Qhr.Server.Filters;

public class AuthFilter(IAuthService auth) : Microsoft.AspNetCore.Mvc.Filters.IAsyncAuthorizationFilter
{
    private readonly IAuthService _auth = auth;

    async Task IAsyncAuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var headers = context.HttpContext.Request.Headers;
        var auth = headers["Authorization"].FirstOrDefault();

        if (auth == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!auth.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = auth.Substring("Bearer ".Length).Trim();
        var username = await _auth.GetUsernameIfJwtValid(token);

        if (username == null) context.Result = new UnauthorizedResult();
    }
}
