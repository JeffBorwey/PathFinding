using NUnit.Framework;
using System;
using System.Linq;
using ValueRange;
using ValueRange.Extensions;

namespace Common.Tests
{
    [TestFixture]
    public class InclusiveValueRangeTests
    {
        [TestCase(5, 0)]
        [TestCase(2, 1)]
        [TestCase(9, 4)]
        [TestCase(1, 0)]
        public void Constructor_UpperValueIsGreaterThanLower_ConstructsCorrectly(int upperValue, int lowerValue)
        {
            var range = new InclusiveValueRange<int>(upperValue, lowerValue);

            Assert.AreEqual(upperValue, range.UpperValueOfRange);
            Assert.AreEqual(lowerValue, range.LowerValueOfRange);
        }

        [TestCase(0, 5)]
        [TestCase(1, 2)]
        [TestCase(4, 9)]
        [TestCase(0, 1)]
        public void Constructor_UpperValueIsLesserThanLower_ConstructsCorrectly(int upperValue, int lowerValue)
        {
            var range = new InclusiveValueRange<int>(upperValue, lowerValue);

            Assert.AreEqual(lowerValue, range.UpperValueOfRange);
            Assert.AreEqual(upperValue, range.LowerValueOfRange);
        }

        [TestCase(13, 10, 10, ExpectedResult = true)]
        [TestCase(25, 11, 25, ExpectedResult = true)]
        [TestCase(98, 43, 40, ExpectedResult = false)]
        [TestCase(100, 0, -1, ExpectedResult = false)]
        public bool Contains_ValueIsInValueRange_ReturnsRightCondition(int upperValue,
            int lowerValue, int testValue)
        {
            var range = new InclusiveValueRange<int>(upperValue, lowerValue);

            return range.Contains(testValue);
        }

        [TestCase(13, 10, 15)]
        [TestCase(25, 11, 27)]
        [TestCase(98, 43, 100)]
        [TestCase(100, 0, 104)]
        public void ReturnInRange_ValueIsGreaterThanUpperValue_ReturnsUpperValue(int upperValue,
            int lowerValue, int outOfRangeValue)
        {
            var range = new InclusiveValueRange<int>(upperValue, lowerValue);

            var valueInRange = range.ReturnInRange(outOfRangeValue);

            Assert.AreEqual(range.UpperValueOfRange, valueInRange);
        }

        [TestCase(13, 10, -15)]
        [TestCase(25, 11, -100)]
        [TestCase(98, 43, -4)]
        [TestCase(100, 0, -1)]
        public void ReturnInRange_ValueIsLesserThanUpperValue_ReturnsLowerValue(int upperValue,
            int lowerValue, int outOfRangeValue)
        {
            var range = new InclusiveValueRange<int>(upperValue, lowerValue);

            var valueInRange = range.ReturnInRange(outOfRangeValue);

            Assert.AreEqual(range.LowerValueOfRange, valueInRange);
        }

        [TestCase(13, 10, 11)]
        [TestCase(25, 11, 18)]
        [TestCase(98, 43, 76)]
        [TestCase(100, 0, 45)]
        public void ReturnInRange_ValueIsInRange_ReturnsUnchangedValue(int upperValue,
            int lowerValue, int withinRangeValue)
        {
            var range = new InclusiveValueRange<int>(upperValue, lowerValue);

            var valueInRange = range.ReturnInRange(withinRangeValue);

            Assert.AreEqual(valueInRange, withinRangeValue);
        }

        [Test]
        public void Amplitude_ExtremeValuesOfInt_ReturnsUintMaxValue()
        {
            var range = new InclusiveValueRange<int>(int.MaxValue, int.MinValue);

            uint amplitude = range.Amplitude();

            Assert.AreEqual(amplitude, uint.MaxValue);
        }

        [TestCase((int)short.MaxValue, (int)short.MinValue)]
        public void GetAllValuesInRange(int maxValue, int minValue)
        {
            var range = new InclusiveValueRange<int>(maxValue, minValue);

            var values = range.GetAllValuesInRange().ToArray();

            Assert.AreEqual(minValue, values.First());
            Assert.AreEqual(maxValue, values.Last());
        }

        [Test]
        public void GetAllValuesInRange_IntValueRange_DoesntThrow()
        {
            var range = new InclusiveValueRange<int>(int.MaxValue, int.MinValue);
            Assert.DoesNotThrow(() => range.GetAllValuesInRange());
        }
    }
}