using System.ServiceProcess;

namespace Service
{
    static class Program
    {
        static void Main()
        {
            ServiceBase.Run(new Service1());
        }
    }
}
