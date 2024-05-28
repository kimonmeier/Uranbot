namespace UranBot.Public.Background;

public interface ITaskManager
{
    Guid RegisterTask(string name, Func<IServiceProvider, Task> action, TimeSpan interval);

    void CancelTask(Guid taskId);
}