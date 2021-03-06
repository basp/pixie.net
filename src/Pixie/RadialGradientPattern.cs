// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Pixie
{
    using System;
    using Linsi;

    public class RadialGradientPattern : Pattern
    {
        public RadialGradientPattern(Color a, Color b)
        {
            this.A = a;
            this.B = b;
        }

        public Color A { get; set; }

        public Color B { get; set; }

        public override Color GetColor(Vector4 point)
        {
            var distance = this.B - this.A;
            var fraction = CalculateFraction(point.X, point.Z);
            return this.A + distance * fraction;
        }

        private static double CalculateFraction(double x, double z)
        {
            var r = Math.Sqrt(x * x + z * z);
            var ir = Math.Floor(r);
            return r - ir;
        }

        // Not a radial gradient but it is a cool effect so
        // save for later.
        //
        // private static double CalculateFraction(double x, double z)
        // {
        //     var ix = Math.Floor(x);
        //     var iz = Math.Floor(z);
        //     var r = Math.Sqrt(x * x + z * z);
        //     var ir = Math.Sqrt(ix * ix + iz * iz);
        //     return (r - ir) / Math.Sqrt(2);
        // }
    }
}
