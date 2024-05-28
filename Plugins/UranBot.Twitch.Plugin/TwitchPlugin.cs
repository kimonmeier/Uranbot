using Microsoft.Extensions.DependencyInjection;
using UranBot.Public;
using UranBot.Public.Background;
using UranBot.Twitch.Plugin.Tasks;

namespace UranBot.Twitch.Plugin;

public class TwitchPlugin : IPlugin
{
    public string Name => "Twitch Plugin";

    private readonly ITaskManager _taskManager;

    private readonly List<Guid> _tasks = new();
    
    public TwitchPlugin(ITaskManager taskManager)
    {
        _taskManager = taskManager;
    }

    public void Start()
    {
        _tasks.Add(_taskManager.RegisterTask("Search Twitch Clips", (serviceProvider) => serviceProvider.GetRequiredService<ClipTask>().CheckClips(), TimeSpan.FromMinutes(5)));
    }

    public void Stop()
    {
        foreach (Guid taskId in _tasks)
        {
            _taskManager.CancelTask(taskId);
        }
    }
}