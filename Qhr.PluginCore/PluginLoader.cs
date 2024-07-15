using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Qhr.PluginCore;

public class PluginLoader
{
    public static Dictionary<string, IJobPlugin> Load(string? path, ILogger? logger)
    {
        Dictionary<string, IJobPlugin> plugins = [];
        var time = new Stopwatch();
        time.Start();
        path ??= Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (!Path.Exists(path))
        {
            logger?.LogError("Plugin path does not exist; skipping load.");
            return plugins;
        }

        logger?.LogInformation("Loading plugins from: {Path}", path);

        var dllFiles = Directory.GetFiles(path, "*.dll");

        foreach (var file in dllFiles)
        {
            var asm = Assembly.LoadFrom(file);
            var types = asm.GetTypes().Where(t => typeof(IJobPlugin).IsAssignableFrom(t) && !t.IsInterface);
            if (types == null) continue;

            foreach (var type in types)
            {
                try
                {
                    var pluginInstance = (IJobPlugin)(Activator.CreateInstance(type) ?? throw new Exception("Instance was null"));
                    var id = pluginInstance.Id;

                    if (plugins.ContainsKey(id)) throw new Exception("Duplicate plugin");
                    plugins[id] = pluginInstance;
                }
                catch (Exception e)
                {
                    logger?.LogError("Failed to load '{TypeName}' from '{AsmName}': {Error}", type.Name, asm.FullName, e.Message);
                }
            }
        }
        time.Stop();

        logger?.LogInformation("Loaded {Count} plugin(s) in {Time} ms", plugins.Count, time.ElapsedMilliseconds);
        return plugins;
    }
}