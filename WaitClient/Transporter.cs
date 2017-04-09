using System.Collections.Generic;
using System.Threading;

namespace Waid
{
    public class Transporter : ITransporter
    {
        private readonly object lockObj = new object();
        private readonly Queue<UserUsage> queue = new Queue<UserUsage>();
        private int numRetries;
        private UserUsage retry;

        public void SendAsync(UserUsage usage)
        {
            lock (lockObj)
            {
                queue.Enqueue(usage);
            }
        }

        public void Start()
        {
            while (true)
            {
                if (SendFailed())
                {
                    Retry();
                }
                else
                {
                    Send();
                }

                Thread.Sleep(20000);
            }
        }

        private bool SendFailed()
        {
            return retry != null;
        }

        private void Send()
        {
            UserUsage firstOne = null;
            lock (lockObj)
            {
                if (queue.Count > 0)
                {
                    firstOne = queue.Dequeue();
                }
            }

            if (firstOne != null)
            {
                bool success = TrySend(firstOne);
                if (!success)
                {
                    retry = firstOne;
                }
            }
        }

        private void Retry()
        {
            bool success = TrySend(retry);
            if (success)
            {
                retry = null;
                numRetries = 0;
            }
            else
            {
                numRetries++;
                if (numRetries > 5)
                {
                    retry = null;
                    numRetries = 0;

                    Logger.Error("Failed to send after 5 times!");
                }
            }
        }

        private static bool TrySend(UserUsage usage)
        {
            // Log to Web Service/Azure/etc

            return true;
        }
    }
}