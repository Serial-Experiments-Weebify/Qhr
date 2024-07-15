
namespace Qhr.Server.Models;
public class Job
{
    public long Id { get; set; }
    public required string Name { get; set; }

    public required string Type { get; set; }
    public required string Content { get; set; }

    public required JobStatus Status { get; set; }
}