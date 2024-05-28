namespace UranBot.Public;

public interface IPlugin
{
    string Name { get; }
    
    void Start();

    void Stop();
}