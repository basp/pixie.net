namespace Pixie.Tests
{
    using System;
    using Xunit;
    using Linsi;

    public class BoundsTests
    {
        [Fact]
        public void CreatingAnEmptyBoundingBox()
        {
            var box = BoundingBox.Empty;
            Assert.Equal(
                Vector4.CreatePosition(
                    double.PositiveInfinity,
                    double.PositiveInfinity,
                    double.PositiveInfinity),
                box.Min);

            Assert.Equal(
                Vector4.CreatePosition(
                    double.NegativeInfinity,
                    double.NegativeInfinity,
                    double.NegativeInfinity),
                box.Max);
        }

        [Fact]
        public void CreateBoundingBoxWithVolume()
        {
            var box = new BoundingBox(
                Vector4.CreatePosition(-1, -2, -3),
                Vector4.CreatePosition(3, 2, 1));

            Assert.Equal(
                Vector4.CreatePosition(-1, -2, -3),
                box.Min);

            Assert.Equal(
                Vector4.CreatePosition(3, 2, 1),
                box.Max);
        }

        [Fact]
        public void AddingPointsToAnEmptyBoundingBox()
        {
            var box = BoundingBox.Empty;
            var p1 = Vector4.CreatePosition(-5, 2, 0);
            var p2 = Vector4.CreatePosition(7, 0, -3);

            box = box.Add(p1);
            box = box.Add(p2);

            Assert.Equal(
                Vector4.CreatePosition(-5, 0, -3),
                box.Min);

            Assert.Equal(
                Vector4.CreatePosition(7, 2, 0),
                box.Max);
        }

        [Fact]
        public void AddBoundingBoxToAnother()
        {
            var box1 = new BoundingBox(
                Vector4.CreatePosition(-5, -2, 0),
                Vector4.CreatePosition(7, 4, 4));
            var box2 = new BoundingBox(
                Vector4.CreatePosition(8, -7, -2),
                Vector4.CreatePosition(14, 2, 8));

            box1 += box2;

            Assert.Equal(
                Vector4.CreatePosition(-5, -7, -2),
                box1.Min);
            Assert.Equal(
                Vector4.CreatePosition(14, 4, 8),
                box1.Max);
        }

        [Theory]
        [InlineData(5, -2, 0, true)]
        [InlineData(11, 4, 7, true)]
        [InlineData(8, 1, 3, true)]
        [InlineData(3, 0, 3, false)]
        [InlineData(8, -4, 3, false)]
        [InlineData(8, 1, -1, false)]
        [InlineData(13, 1, 3, false)]
        [InlineData(8, 5, 3, false)]
        [InlineData(8, 1, 8, false)]
        public void CheckBoxContainsGivenPoint(
            double px,
            double py,
            double pz,
            bool expected)
        {
            var box = new BoundingBox(
                Vector4.CreatePosition(5, -2, 0),
                Vector4.CreatePosition(11, 4, 7));

            var p = Vector4.CreatePosition(px, py, pz);
            Assert.Equal(expected, box.Contains(p));
        }

        [Theory]
        [InlineData(5, -2, 0, 11, 4, 7, true)]
        [InlineData(6, -1, 1, 10, 3, 6, true)]
        [InlineData(4, -3, -1, 10, 3, 6, false)]
        [InlineData(6, -1, 1, 12, 5, 8, false)]
        public void CheckBoxContainsBox(
            double x0,
            double y0,
            double z0,
            double x1,
            double y1,
            double z1,
            bool expected)
        {
            var box = new BoundingBox(
                Vector4.CreatePosition(5, -2, 0),
                Vector4.CreatePosition(11, 4, 7));
            var min = Vector4.CreatePosition(x0, y0, z0);
            var max = Vector4.CreatePosition(x1, y1, z1);
            var box2 = new BoundingBox(min, max);
            Assert.Equal(expected, box.Contains(box2));
        }

        [Fact]
        public void TransformBoundingBox()
        {
            var cmp = Vector4.GetEqualityComparer(0.0001);

            var box = new BoundingBox(
                Vector4.CreatePosition(-1, -1, -1),
                Vector4.CreatePosition(1, 1, 1));

            var m =
                Transform.RotateX(Math.PI / 4) *
                Transform.RotateY(Math.PI / 4);

            var box2 = box * m;

            Assert.Equal(
                Vector4.CreatePosition(-1.4142, -1.7071, -1.7071),
                box2.Min,
                cmp);

            Assert.Equal(
                Vector4.CreatePosition(1.4142, 1.7071, 1.7071),
                box2.Max,
                cmp);
        }

        [Theory]
        [InlineData(5, 0.5, 0, -1, 0, 0, true)]
        [InlineData(-5, -0.5, 0, 1, 0, 0, true)]
        [InlineData(0.5, 5, 0, 0, -1, 0, true)]
        [InlineData(0.5, -5, 0, 0, 1, 0, true)]
        [InlineData(0.5, 0, 5, 0, 0, -1, true)]
        [InlineData(0.5, 0, -5, 0, 0, 1, true)]
        [InlineData(0, 0.5, 0, 0, 0, 1, true)]
        [InlineData(-2, 0, 0, 2, 4, 6, false)]
        [InlineData(0, -2, 0, 6, 2, 4, false)]
        [InlineData(0, 0, -2, 4, 6, 2, false)]
        [InlineData(2, 0, 2, 0, 0, -1, false)]
        [InlineData(0, 2, 2, 0, -1, 0, false)]
        [InlineData(2, 2, 0, -1, 0, 0, false)]

        public void IntersectingRayWithBoundingBox(
            double ox,
            double oy,
            double oz,
            double dx,
            double dy,
            double dz,
            bool expected)
        {
            var box = new BoundingBox(
                Vector4.CreatePosition(-1, -1, -1),
                Vector4.CreatePosition(1, 1, 1));

            var origin = Vector4.CreatePosition(ox, oy, oz);
            var direction = Vector4.CreateDirection(dx, dy, dz).Normalize();
            var r = new Ray(origin, direction);
            var result = box.Intersect(r);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(15, 1, 2, -1, 0, 0, true)]
        [InlineData(-5, -1, 4, 1, 0, 0, true)]
        [InlineData(7, 6, 5, 0, -1, 0, true)]
        [InlineData(9, -5, 6, 0, 1, 0, true)]
        [InlineData(8, 2, 12, 0, 0, -1, true)]
        [InlineData(6, 0, -5, 0, 0, 1, true)]
        [InlineData(8, 1, 3.5, 0, 0, 1, true)]
        [InlineData(9, -1, -8, 2, 4, 6, false)]
        [InlineData(8, 3, -4, 6, 2, 4, false)]
        [InlineData(9, -1, -2, 4, 6, 2, false)]
        [InlineData(4, 0, 9, 0, 0, -1, false)]
        [InlineData(8, 6, -1, 0, -1, 0, false)]
        [InlineData(12, 5, 4, -1, 0, 0, false)]
        public void IntersectingRayWithNonCubicBoundingBox(
            double ox,
            double oy,
            double oz,
            double dx,
            double dy,
            double dz,
            bool expected)
        {
            var box = new BoundingBox(
                Vector4.CreatePosition(5, -2, 0),
                Vector4.CreatePosition(11, 4, 7));
            var origin = Vector4.CreatePosition(ox, oy, oz);
            var direction = Vector4.CreateDirection(dx, dy, dz).Normalize();
            var r = new Ray(origin, direction);
            var result = box.Intersect(r);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SplitPerfectCube()
        {
            var box = new BoundingBox(
                Vector4.CreatePosition(-1, -4, -5),
                Vector4.CreatePosition(9, 6, 5));

            box.Split(out var left, out var right);
            Assert.Equal(Vector4.CreatePosition(-1, -4, -5), left.Min);
            Assert.Equal(Vector4.CreatePosition(4, 6, 5), left.Max);
            Assert.Equal(Vector4.CreatePosition(4, -4, -5), right.Min);
            Assert.Equal(Vector4.CreatePosition(9, 6, 5), right.Max);
        }

        [Fact]
        public void SplitXWideBox()
        {
            var box = new BoundingBox(
                Vector4.CreatePosition(-1, -2, -3),
                Vector4.CreatePosition(9, 5.5, 3));
            box.Split(out var left, out var right);
            Assert.Equal(Vector4.CreatePosition(-1, -2, -3), left.Min);
            Assert.Equal(Vector4.CreatePosition(4, 5.5, 3), left.Max);
            Assert.Equal(Vector4.CreatePosition(4, -2, -3), right.Min);
            Assert.Equal(Vector4.CreatePosition(9, 5.5, 3), right.Max);
        }

        [Fact]
        public void SplitYWideBox()
        {
            var box = new BoundingBox(
                Vector4.CreatePosition(-1, -2, -3),
                Vector4.CreatePosition(5, 8, 3));
            box.Split(out var left, out var right);
            Assert.Equal(Vector4.CreatePosition(-1, -2, -3), left.Min);
            Assert.Equal(Vector4.CreatePosition(5, 3, 3), left.Max);
            Assert.Equal(Vector4.CreatePosition(-1, 3, -3), right.Min);
            Assert.Equal(Vector4.CreatePosition(5, 8, 3), right.Max);
        }

        [Fact]
        public void SplitZWideBox()
        {
            var box = new BoundingBox(
                Vector4.CreatePosition(-1, -2, -3),
                Vector4.CreatePosition(5, 3, 7));
            box.Split(out var left, out var right);
            Assert.Equal(Vector4.CreatePosition(-1, -2, -3), left.Min);
            Assert.Equal(Vector4.CreatePosition(5, 3, 2), left.Max);
            Assert.Equal(Vector4.CreatePosition(-1, -2, 2), right.Min);
            Assert.Equal(Vector4.CreatePosition(5, 3, 7), right.Max);
        }
    }
}