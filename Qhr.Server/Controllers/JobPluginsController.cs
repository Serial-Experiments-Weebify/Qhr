using Microsoft.AspNetCore.Mvc;
using Qhr.Server.Services;
using Qhr.Server.DTO;
using Qhr.Server.Filters;

namespace Qhr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(AuthFilter))]
public class JobPluginsController(IJobPluginService jps) : ControllerBase
{
    private readonly IJobPluginService _jobPluginService = jps;

    [HttpGet]
    public IEnumerable<JobPluginDto> ListPlugins()
    {
        return _jobPluginService.GetAllPlugins().Select(JobPluginDto.FromPlugin);
    }
};
