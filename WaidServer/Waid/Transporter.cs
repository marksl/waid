using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Waid.WindowsAzure;

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
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(new Uri(Config.Urls), "api/data"));
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = GetJson(usage.Start, usage.UserId, usage.AppNames, usage.AppUsedNameHashCodes,
                                          usage.AppUsedSeconds);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    if (httpResponse.StatusCode != HttpStatusCode.OK &&
                        httpResponse.StatusCode != HttpStatusCode.NoContent)
                    {
                        Logger.ErrorFormat("Response code was no 200 or 204: {0}", httpResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occured during SendAsync", ex);

                return false;
            }

            return true;
        }


        private static string GetJson(DateTime startTime, Guid userId, IEnumerable<string> appNames,
                                      IEnumerable<uint> appsUsed, IEnumerable<float> appUsedTimes)
        {
            var builder = new StringBuilder(200);
            builder.Append('{')
                   .Append("\"UserId\":\"").Append(userId).Append("\",")
                   .Append("\"Start\":\"").Append(startTime.ToString("o")).Append("\",")
                   .Append("\"AppNames\":[");

            bool first = true;

            foreach (string appName in appNames)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(",");
                }

                builder.Append("\"").Append(appName).Append("\"");
            }

            builder.Append("],\"AppUsedSeconds\":[");

            first = true;
            foreach (float appUsedTime in appUsedTimes)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(",");
                }

                builder.Append(appUsedTime);
            }

            builder.Append("],\"AppUsedNameHashCodes\":[");

            first = true;
            foreach (uint appUsedHashcode in appsUsed)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(",");
                }

                builder.Append(appUsedHashcode);
            }

            builder.Append("]");

            builder.Append("}");

            return builder.ToString();
        }
    }
}