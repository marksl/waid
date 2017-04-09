using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using NUnit.Framework;

namespace Waid.Tests
{
    [TestFixture]
    public class CollectorTests
    {
        
        [Test]
        public void IntPtrToString()
        {
            var foo = new IntPtr(5);

            string fooString = foo.ToString();

            Assert.IsNotNull(fooString);
        }


        [Test]
        public void Start_Overflow_WorksCorrectly()
        {
            
            //var initialOS = Substitute.For<IOperatingSystem>();
            //var initialTransport = Substitute.For<ITransporter>();

            //var collector = new Collector(initialOS, initialTransport);

            //collector.RunOnce();


            //int runNumTimes = 1;
            //os.GetCurrentProcessName().Returns("Visual Studio");

            //var transporter = Substitute.For<ITransporter>();
//            Collector.RunOnce(os, transporter);
           
            //os.Received().
        }
    }
}
