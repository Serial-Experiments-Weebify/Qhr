using Microsoft.AspNetCore.Mvc;
using Qhr.Server.Services;
using Qhr.Server.Models;
using Qhr.Server.DTO;

namespace Qhr.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class JobController(IJobService js) : ControllerBase
{
    private readonly IJobService _jobService = js;

    [HttpPost]
    public async Task<ActionResult<Job>> CreateJob(CreateJobDTO cr)
    {
        Console.WriteLine(cr);
        return await _jobService.CreateJob(cr);
    }

    [HttpGet]
    public async Task<IEnumerable<Job>> ListJobs()
    {
        return await _jobService.ListAllJobs();
    }
};