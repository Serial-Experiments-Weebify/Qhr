using Microsoft.AspNetCore.Mvc;
using Qhr.Server.Services;
using Qhr.Server.Models;
using Qhr.Server.DTO;
using Qhr.Server.Filters;

namespace Qhr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(AuthFilter))]
public class JobController(IJobService js) : ControllerBase
{
    private readonly IJobService _jobService = js;

    [HttpPost]
    public async Task<ActionResult<Job>> CreateJob(CreateJobDTO cr)
    {
        return await _jobService.CreateJob(cr);
    }

    [HttpGet]
    public async Task<IEnumerable<Job>> ListJobs()
    {
        return await _jobService.ListAllJobs();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Job>> GetJobById(long id)
    {
        var r = await _jobService.GetJobById(id);
        return r != null ? Ok(r) : NotFound();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteJob(long id)
    {
        var r = await _jobService.RemoveJobById(id);
        return r ? Ok() : NotFound();
    }

};
