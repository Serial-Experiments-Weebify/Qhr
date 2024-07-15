using Microsoft.AspNetCore.Mvc;
using Qhr.Server.DTO;
using Qhr.Server.Services;

namespace Qhr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(QhrContext ctx, IAuthService auth) : ControllerBase
{
    private readonly QhrContext _ctx = ctx;
    private readonly IAuthService _auth = auth;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO data)
    {
        var user = await _auth.Authenticate(data.Username, data.Password);

        if (user != null)
        {
            var token = _auth.CreateSessionToken(user);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}
