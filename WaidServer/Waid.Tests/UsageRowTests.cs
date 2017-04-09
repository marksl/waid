
using System;
using NUnit.Framework;

namespace Waid.Tests
{
    [TestFixture]
    public class UsageRowTests
    {
        //[Test]
        //public void ToUsage_HappyPathTest()
        //{
        //    var expectedUsage = new HourlyUsage
        //                    {
        //                        //AppNames = new string[] {"One", "Two", "Three"},
        //                        AppUsedNameHashCodes = new uint[] {2, 234, 222, 114541},
        //                        AppUsedSeconds = new float[] {234.0f, 333.0f, 0.003f, 0.04f}
        //                    };

        //    var usageRow = new UsageRow(expectedUsage);
        //    var actualUsage = usageRow.ToUsage();

        //   // Assert.AreEqual(expectedUsage.AppNames, actualUsage.AppNames, "AppNames are not equal.");
        //    Assert.AreEqual(expectedUsage.AppUsedNameHashCodes, actualUsage.AppUsedNameHashCodes,"AppUsedNameHashCodes are not equal.");
        //    Assert.AreEqual(expectedUsage.AppUsedSeconds, actualUsage.AppUsedSeconds,"AppUsedSeconds are not equal.");
        //}

        [Test]
        public void Basic_Array_Comparison()
        {
            var floatArray1 = new float[] { 123.45f, 123f, 45f, 1.2f, 34.5f };

            // create a byte array and copy the floats into it...
            var byteArray = new byte[floatArray1.Length * 4];
            Buffer.BlockCopy(floatArray1, 0, byteArray, 0, byteArray.Length);

            // create a second float array and copy the bytes into it...
            var floatArray2 = new float[byteArray.Length / 4];
            Buffer.BlockCopy(byteArray, 0, floatArray2, 0, byteArray.Length);

            Assert.AreEqual(floatArray1, floatArray2);

        }
         
    }
}