using Bob.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Bob.Tests.Converters
{
    [TestClass]
    public class NullableIntConverterTests
    {
        [TestMethod]
        public void ProvidingNullReturnsNullableFloat()
        {
            var converter = new NullableIntConverter();

            var result = converter.ConvertBack(null, typeof(float?), null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IntStringReturnsParsedInt()
        {
            var converter = new NullableIntConverter();

            const int expected = 42;

            var result = converter.ConvertBack(expected.ToString(), typeof(int?), null, null);

            Assert.AreEqual(expected, result);
        }
    }
}