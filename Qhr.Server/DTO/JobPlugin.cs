using Qhr.PluginCore;

namespace Qhr.Server.DTO;

public class JobPluginDto
{
    required public string Name { get; set; }
    required public string Id { get; set; }

    public static JobPluginDto FromPlugin(IJobPlugin pl)
    {
        return new JobPluginDto
        {
            Name = pl.FriendlyName,
            Id = pl.Id
        };
    }
}
