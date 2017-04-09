using System;
using System.Web;
using System.Web.Http;
using Waid.WindowsAzure;
using WaidWeb.Models;
using WaidWeb.Transformations;

namespace WaidWeb.Controllers
{
    // Note to self: floats are translated to JSON numerics. Doubles are transmitted as strings.
    public class DataController : ApiController
    {
        public void Post(UserUsage usage)
        {
            var repo = new UsageRepository();
            repo.Save(new UsageRow(usage));
        }

        public Data GetByDay(long msSinceEpoch, int minutesOffset)
        {
            Guid userId;

            if (!UserIdRepository.TryGetUserId(out userId))
            {
                return new Data();
            }

            DateTime utcStart = new DateTime(1970, 1, 1).AddTicks(msSinceEpoch * 10000);
            DateTime utcEnd = utcStart.AddDays(1);

            var repo = new UsageRepository();
            var dataRows = repo.GetRows(userId, utcStart, utcEnd);
            var dailyUsage = dataRows.ToDaily(minutesOffset); 
            var uiUsage = dailyUsage.ToUI();
            return uiUsage;
        }
    }
}