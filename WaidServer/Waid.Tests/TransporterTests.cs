using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Waid.Tests
{
    [TestFixture]
    public class TransporterTests
    {
        [Test]
        public void DateTimeTest()
        {
            var date = DateTime.Parse("2012-12-05T17:24:22.5760043Z");
            
            Assert.AreEqual(date.ToUniversalTime().ToString("o"), "2012-12-05T17:24:22.5760043Z");
        }
    }
}
