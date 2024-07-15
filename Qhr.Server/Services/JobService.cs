

using Microsoft.EntityFrameworkCore;
using Qhr.Server.DTO;
using Qhr.Server.Models;

namespace Qhr.Server.Services;

public interface IJobService
{
    public Task<IEnumerable<Job>> ListAllJobs();
    public Task<Job> CreateJob(CreateJobDTO crJob);
    public Task<bool> RemoveJobById(long id);
    public Task<Job?> GetJobById(long id);
}

public class JobService(QhrContext context, IJobPluginService jps) : IJobService
{
    private readonly QhrContext _ctx = context;
    private readonly IJobPluginService _plugins = jps;

    public async Task<Job> CreateJob(CreateJobDTO crJob)
    {
        var plugin = _plugins.GetPluginByName(crJob.Type) ?? throw new Exception("Invalid plugin type");

        try
        {
            await plugin.ParseJobArgs(crJob.Content);
        } catch (Exception e) {
            throw new Exception("Job content invalid", e);
        }

        var job = new Job()
        {
            Name = crJob.Name,
            Content = crJob.Content,
            Type = crJob.Type,
            Status = JobStatus.Created,
        };

        _ctx.Add(job);
        await _ctx.SaveChangesAsync();

        return job;
    }

    public async Task<IEnumerable<Job>> ListAllJobs()
    {
        return await _ctx.Jobs.ToListAsync();
    }

    public async Task<Job?> GetJobById(long id)
    {
        return await _ctx.Jobs.FindAsync(id);
    }

    public async Task<bool> RemoveJobById(long id)
    {
        var job = await _ctx.Jobs.FindAsync(id);

        if (job == null) return false;

        _ctx.Jobs.Remove(job);
        await _ctx.SaveChangesAsync();
        return true;
    }
}