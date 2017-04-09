using System;
using System.Collections.Generic;
using System.Web.Http;
using Waid.WindowsAzure;
using WaidWeb.Models;
using WaidWeb.Transformations;

namespace WaidWeb.Controllers
{
    public class TableController : ApiController
    {
        public IEnumerable<TableData> GetByDay(long msSinceEpoch, int minutesOffset)
        {
            Guid userId;
            List<UsageRow> dataRows;

            if (!UserIdRepository.TryGetUserId(out userId))
            {
                dataRows = new List<UsageRow>();
            }
            else
            {
                DateTime utcStart = new DateTime(1970, 1, 1).AddTicks(msSinceEpoch * 10000);
                DateTime utcEnd = utcStart.AddDays(1);

                var repo = new UsageRepository();
                dataRows = repo.GetRows(userId, utcStart, utcEnd);    
            }

            return dataRows.ToTableData();
        }
    }
}