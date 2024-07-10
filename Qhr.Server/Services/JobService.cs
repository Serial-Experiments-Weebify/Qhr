

using Microsoft.EntityFrameworkCore;
using Qhr.Server.DTO;
using Qhr.Server.Models;

namespace Qhr.Server.Services;

public interface IJobService
{
    public Task<Job> CreateJob(CreateJobDTO crJob);
    public Task<IEnumerable<Job>> ListAllJobs();

}

public class JobService : IJobService
{
    private readonly QhrContext _ctx;

    public JobService(QhrContext context)
    {
        _ctx = context;
    }

    public async Task<Job> CreateJob(CreateJobDTO crJob)
    {
        var job = new Job()
        {
            Name = crJob.Name,
            Content = crJob.Content
        };

        _ctx.Add(job);
        await _ctx.SaveChangesAsync();

        return job;
    }

    public async Task<IEnumerable<Job>> ListAllJobs()
    {
        return await _ctx.Jobs.ToListAsync();
    }
}