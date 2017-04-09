namespace Waid
{
    public interface ITransporter
    {
        void SendAsync(UserUsage usage);
    }
}