using System.Diagnostics;
using UranBot.Public.Background;

namespace UranBot.Plugins;

public class TaskManager : ITaskManager, IDisposable
{
    private class TaskEntry
    {
        public required long LastExecution { get; set; }
        
        public required TimeSpan Interval { get; init; }
        
        public required string Name { get; init; }
        
        public required Func<IServiceProvider, Task> Action { get; init; }
    }

    private readonly Dictionary<Guid, TaskEntry> _taskEntries = new();
    private readonly ILogger<TaskManager> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timer;
    private volatile bool _isExecuting;
    private bool _isInitialized;

    public TaskManager(ILogger<TaskManager> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public void StartTaskManager()
    {
        _timer = new Timer(ExecuteQueue, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }
    
    public Guid RegisterTask(string name, Func<IServiceProvider, Task> action, TimeSpan interval)
    {
        Guid guid = Guid.NewGuid();
        
        _taskEntries.Add(guid, new TaskEntry
        {
            Name = name,
            Action = action,
            Interval = interval,
            LastExecution = 0
        });

        return guid;
    }

    internal void StartTask(Guid taskId)
    {
        _taskEntries[taskId].LastExecution = DateTime.MinValue.Ticks;
    }
    
    public void CancelTask(Guid taskId)
    {
        _taskEntries.Remove(taskId);
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void ExecuteQueue(object? _)
    {
        if (_isExecuting || !_isInitialized)
        {
            return;
        }

        _isExecuting = true;

        try
        {
            foreach (TaskEntry value in _taskEntries.Values.Where(value => value.Interval.Ticks < DateTime.Now.Ticks - value.LastExecution))
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        _logger.LogDebug("Task {0} started", value.Name);

                        Stopwatch stopwatch = Stopwatch.StartNew();
                        value.Action(scope.ServiceProvider).ConfigureAwait(false).GetAwaiter().GetResult();
                        stopwatch.Stop();

                        _logger.LogDebug("The Task {0} run for {1}", value.Name, stopwatch.Elapsed.ToString());

                        value.LastExecution = DateTime.Now.Ticks;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "During the execution of {0} an exception occured", value.Name);
                    }
                }
            }
        }
        finally
        {
            _isExecuting = false;
        }
    }

    public void SetInitialized()
    {
        if (_isInitialized)
        {
            throw new ArgumentException("The TaskManager was already initialized");
        }

        _isInitialized = true;
    }
}