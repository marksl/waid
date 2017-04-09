namespace Waid
{
    public interface IOperatingSystem
    {
        string GetCurrentProcessName();
        ulong GetMillisecondsSinceLastInput();
    }
}