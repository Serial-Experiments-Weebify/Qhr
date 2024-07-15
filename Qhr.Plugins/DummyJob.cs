using Qhr.PluginCore;

namespace Qhr.Plugins;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

public class DummyJob : IJobPlugin
{
    public string FriendlyName
    {
        get
        {
            return "Dummy";
        }
    }

    public string Id
    {
        get
        {
            return "qhr-dummy";
        }
    }

    public async Task<bool> ExecuteJob(object args, Action<double> reportProgress, Action<string> reportStatus, CancellationToken c)
    {
        reportStatus("Working");
        reportProgress(0);

        for (int i = 1; i <= 100; i++)
        {
            reportProgress(i / 100.0);
            await Task.Delay(200, c);
            if (c.IsCancellationRequested) return false;
        }

        Console.WriteLine((string)args);
        reportStatus("Done");
        return true;
    }

    public async Task<object> ParseJobArgs(string argsJson)
    {
        return (object)argsJson;
    }
}
