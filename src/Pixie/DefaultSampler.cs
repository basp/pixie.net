// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Pixie
{
    using System.Threading;
    using Linie;

    public class DefaultSampler : ISampler
    {
        private readonly World world;
        private readonly Camera camera;

        public DefaultSampler(World world, Camera camera)
        {
            this.world = world;
            this.camera = camera;        
        }

        public Ray4 Ray4ForPixel(int px, int py)
        {
            var pixelSize = this.camera.PixelSize;
            
            var halfWidth = this.camera.HalfWidth;
            var halfHeight = this.camera.HalfHeight;

            var xOffset = (px + 0.5) * pixelSize;
            var yOffset = (py + 0.5) * pixelSize;

            var worldX = halfWidth - xOffset;
            var worldY = halfHeight - yOffset;

            var inv = this.camera.TransformInv;

            var pixel = inv * Vector4.CreatePosition(worldX, worldY, -1);
            var origin = inv * Vector4.CreatePosition(0, 0, 0);
            var direction = (pixel - origin).Normalize();

            return new Ray4(origin, direction);
        }

        public Color Sample(int x, int y)
        {
            Interlocked.Increment(ref Stats.PrimaryRay4s);
            var ray = this.Ray4ForPixel(x, y);
            return world.Trace(ray);
        }
    }
}
