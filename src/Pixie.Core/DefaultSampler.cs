namespace Pixie.Core
{
    using System.Threading;

    public class DefaultSampler : ISampler
    {
        private readonly World world;
        private readonly Camera camera;

        public DefaultSampler(World world, Camera camera)
        {
            this.world = world;
            this.camera = camera;            
        }

        public Ray RayForPixel(int px, int py)
        {
            var pixelSize = this.camera.PixelSize;
            
            var halfWidth = this.camera.HalfWidth;
            var halfHeight = this.camera.HalfHeight;

            var xOffset = (px + 0.5) * pixelSize;
            var yOffset = (py + 0.5) * pixelSize;

            var worldX = halfWidth - xOffset;
            var worldY = halfHeight - yOffset;

            var inv = this.camera.Transform.Inverse();

            var pixel = inv * Double4.Point(worldX, worldY, -1);
            var origin = inv * Double4.Point(0, 0, 0);
            var direction = (pixel - origin).Normalize();

            return new Ray(origin, direction);
        }

        public Color Sample(int x, int y)
        {
            Interlocked.Increment(ref Camera.Stats.PrimaryRays);
            var ray = this.RayForPixel(x, y);
            return world.ColorAt(ray);
        }
    }
}