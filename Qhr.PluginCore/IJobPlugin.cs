namespace Qhr.PluginCore;

public interface IJobPlugin
{
    string FriendlyName { get; }
    string Id { get; }
    Task<object> ParseJobArgs(string argsJson);
    Task<bool> ExecuteJob(object args, Action<double> reportProgress, Action<string> reportStatus, CancellationToken c);
}