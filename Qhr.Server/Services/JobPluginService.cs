using Qhr.Server.DTO;
using Qhr.PluginCore;

namespace Qhr.Server.Services;

public interface IJobPluginService
{
    IEnumerable<IJobPlugin> GetAllPlugins();

    IJobPlugin? GetPluginByName(string name);
}

class JobPluginService : IJobPluginService
{
    ILogger<JobPluginService> _logger;
    Dictionary<string, IJobPlugin> _plugins;
    public JobPluginService(ILogger<JobPluginService> logger)
    {
        _logger = logger;
        _plugins = PluginLoader.Load(null, _logger);
    }

    public IEnumerable<IJobPlugin> GetAllPlugins()
    {
        return _plugins.Values;
    }

    public IJobPlugin? GetPluginByName(string name)
    {
        return _plugins[name];
    }

}