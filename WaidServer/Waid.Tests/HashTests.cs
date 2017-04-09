using NUnit.Framework;
using Waid.WindowsAzure;

namespace Waid.Tests
{
    [TestFixture]
    public class HashTests
    {
        [Test]
        public void NoActivityHash()
        {
            var hash = StringUtils.HashString("No Activity");
            var hash2 = StringUtils.HashString("Idle");

            Assert.AreNotEqual(hash,hash2);


        }
         
    }
}