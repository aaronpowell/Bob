using Bob.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Bob.Tests.Converters
{
    [TestClass]
    public class NullableFloatConverterTests
    {
        [TestMethod]
        public void ProvidingNullReturnsNullableFloat()
        {
            var converter = new NullableFloatConverter();

            var result = converter.ConvertBack(null, typeof (float?), null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void FloatStringReturnsParsedFloat()
        {
            var converter = new NullableFloatConverter();

            const float expected = 12.34f;

            var result = converter.ConvertBack(expected.ToString(), typeof (float?), null, null);

            Assert.AreEqual(expected, result);
        }
    }
}
