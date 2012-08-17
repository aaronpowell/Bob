using System;
using Bob.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Bob.Tests.Converters
{
    [TestClass]
    public class NullableDateTimeConverterTests
    {
        [TestMethod]
        public void ProvidingNullReturnsNullableFloat()
        {
            var converter = new NullableDateTimeConverter();

            var result = converter.ConvertBack(null, typeof(float?), null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DateTimeStringReturnsParsedDateTime()
        {
            var converter = new NullableDateTimeConverter();

            var expected = DateTime.Now;

            var result = (DateTime)converter.ConvertBack(expected.ToString(), typeof(DateTime?), null, null);

            Assert.AreEqual(expected.ToString("dd MMMM yyyy hh:mm:ss"), result.ToString("dd MMMM yyyy hh:mm:ss"));
        }
    }
}