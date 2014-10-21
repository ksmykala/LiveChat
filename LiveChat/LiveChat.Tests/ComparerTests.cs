using System.Linq;
using LiveChat.Domain.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveChat.Tests
{
    [TestClass]
    public class ComparerTests
    {
        [TestMethod]
        public void ArraysEqualPositive()
        {
            var array1 = new[] { 1, 2, 3 };
            var array2 = new[] { 1, 2, 3 };

            var result = ComparerHelper.ArraysEqual(array1, array2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ArraysEqualNegative()
        {
            var array1 = new[] { 1, 2, 3 };
            var array2 = new[] { 1, 2, 4 };

            var result = ComparerHelper.ArraysEqual(array1, array2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ArraysEqualChangeOrderPositive()
        {
            var array1 = new[] { 1, 2, 3 };
            var array2 = new[] { 3, 2, 1 };

            var result = ComparerHelper.ArraysEqual(array1, array2.OrderBy(x => x).ToArray());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ArraysEqualChangeOrderNegative()
        {
            var array1 = new[] { 1, 2, 3 };
            var array2 = new[] { 3, 2, 1 };

            var result = ComparerHelper.ArraysEqual(array1, array2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ArraysEqualDiffrentLength()
        {
            var array1 = new[] { 1, 2, 3 };
            var array2 = new[] { 1, 2, 3, 4 };

            var result = ComparerHelper.ArraysEqual(array1, array2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ArraysEqualTheSameReference()
        {
            var array1 = new[] { 1, 2, 3 };
            var array2 = array1;

            var result = ComparerHelper.ArraysEqual(array1, array2);

            Assert.IsTrue(result);
        }
    }
}
