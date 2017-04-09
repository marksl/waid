using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Waid.WindowsAzure
{
    public class UsageRepository
    {
        private readonly CloudStorageAccount _storageAccount;

        public UsageRepository(CloudStorageAccount account)
        {
            _storageAccount = account;
        }

        public UsageRepository()
            : this(CloudStorageAccount.FromConfigurationSetting("DataConnectionString"))
        {
        }

        public const string noActivity = "No Activity";

        public List<UsageRow> GetRows(Guid uploadId, DateTime utcStart, DateTime utcEnd)
        {
            var utcStartTicks = utcStart.Ticks.ToString(CultureInfo.InvariantCulture);
            var utcEndTicks = utcEnd.Ticks.ToString(CultureInfo.InvariantCulture);

            string key = string.Format("{0}{1}{2}", uploadId, utcStartTicks, utcEndTicks);

            object cachedValue = HttpContext.Current.Cache[key];
            if (cachedValue != null)
            {
                return (List<UsageRow>) cachedValue;
            }

            UsageDataContext dc = CreateDataContext();
            List<UsageRow> usageRows = dc.Usage.Where(u => u.PartitionKey == uploadId.ToString()
                                                           &&
                                                           String.Compare(u.RowKey, utcStartTicks, StringComparison.InvariantCulture) >= 0
                                                           &&
                                                           String.Compare(u.RowKey, utcEndTicks, StringComparison.InvariantCulture) <= 0)
                .AsTableServiceQuery().ToList();

            HttpContext.Current.Cache.Add(key, usageRows,
                                              null,
                                              DateTime.Now.AddMinutes(9),
                                              Cache.NoSlidingExpiration,
                                              CacheItemPriority.High, null);

            //HttpContext.Current.Cache[key] = usageRows;

            return usageRows;
        }

        
        public void Save(UsageRow usage)
        {
            UsageDataContext dc = CreateDataContext();

            dc.AddObject(UsageDataContext.UsageTable,usage);
            dc.SaveChanges();
        }

        private UsageDataContext CreateDataContext()
        {
            return new UsageDataContext(_storageAccount);
        }
    }
}