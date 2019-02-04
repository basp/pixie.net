namespace Pixie.Test
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Pixie.Core;

    public class Float3x3Tests
    {
        const float epsilon = 0.000000001f;

        private static readonly IEqualityComparer<Float2x2> Float2x2Comparer =
         new ApproxFloat2x2EqualityComparer(epsilon);

        [Fact]
        public void TestSubmatrix()
        {
            var a = new Float3x3(
                1, 5, 0,
                -3, 2, 7,
                0, 6, -3);

            var expected = new Float2x2(
                -3, 2, 
                0, 6);

            Assert.Equal(expected, a.Submatrix(0, 2), Float2x2Comparer);
        }

        [Fact]
        public void TestCalculateMinorOf3x3Matrix()
        {
            var a = new Float3x3(
                3, 5, 0,
                2, -1, -7,
                6, -1, 5);

            var b = a.Submatrix(1, 0);

            Assert.Equal(25, b.Determinant());
            Assert.Equal(25, a.Minor(1, 0));
        }

        [Fact]
        public void TestCalculateCofactorOf3x3Matrix()
        {
            var a = new Float3x3(
                3, 5, 0,
                2, -1, -7,
                6, -1, 5);

            Assert.Equal(-12, a.Minor(0, 0));
            Assert.Equal(-12, a.Cofactor(0, 0));
            Assert.Equal(25, a.Minor(1, 0));
            Assert.Equal(-25, a.Cofactor(1, 0));
        }

        [Fact]
        public void TestCalculateDeterminantOf3x3Matrix()
        {
            var a = new Float3x3(
                1, 2, 6,
                -5, 8, -4,
                2, 6, 4);

            Assert.Equal(56, a.Cofactor(0, 0));
            Assert.Equal(12, a.Cofactor(0, 1));
            Assert.Equal(-46, a.Cofactor(0, 2));
            Assert.Equal(-196, a.Determinant());
        }
    }
}