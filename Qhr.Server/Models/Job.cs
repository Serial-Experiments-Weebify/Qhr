
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qhr.Server.Models;
public class Job
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Content { get; set; }
}