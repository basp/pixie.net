namespace Pixie.Cmd.Examples
{
    using System;
    using Pixie.Core;

    public class TestPattern1 : Pattern
    {
        const double eps = 0.0001;

        public override Color PatternAt(Double4 point)
        {
            var ix = Math.Floor(point.X);
            var f = Math.Abs(point.X - ix);

            if((f - eps) < 0.2)
            {
                return new Color(0.22, 0.24, 0.24);                
            }

            if((f - eps) < 0.5)
            {
                return new Color(0, 0.67, 0.71);
            }

            if((f - eps) < 0.7)
            {
                return new Color(0.97, 0.31, 0);
            }

            if((f - eps) < 0.8)
            {
                return new Color(0.99, 0.50, 0.38);
            }
            
            return new Color(1, 0.99, 0.95);
        }
    }

    public static class Test01
    {
        public static Tuple<World, Camera> Create(int width, int height)
        {
            // var pat1 = new TestPattern1()
            // {
            //     Transform = Double4x4.Identity * 
            //         Transform.Scale(2.1, 1, 1.7) *
            //         Transform.RotateY(Math.PI / 4),
            // };

            var floor = new Plane()
            {
                Material = new Material()
                {
                    Color = new Color(0.1, 0.3, 0.2),
                    Specular = 0,
                    Diffuse = 0.8,
                    Ambient = 0.2,
                },
            };

            var s1 = new Sphere()
            {
                Material = new Material()
                {
                    Color = new Color(0.1, 0.4, 0.72),
                },

                Transform = Double4x4.Identity *
                    Transform.Scale(0.5, 0.5, 0.5) *
                    Transform.Translate(0, 1, 0),
            };

            var s2 = new Sphere()
            {
                Material = new Material()
                {
                    Color = new Color(0.52, 0.4, 0.32),
                },

                Transform = Double4x4.Identity *
                    Transform.Scale(0.5, 0.5, 0.5) *
                    Transform.Translate(0, 1, -2),
            };

            var l1 = new PointLight(
                Double4.Point(-100, 40, -20),
                new Color(1, 1, 1));

            var world = new World();
            world.Objects.Add(floor);
            world.Objects.Add(s1);
            world.Objects.Add(s2);
            world.Lights.Add(l1);

            var cam = new Camera(width, height, Math.PI / 4)
            {
                Transform = Transform.View(
                    Double4.Point(0, 2, -3),
                    Double4.Point(0, 0, 0),
                    Double4.Vector(0, 1, 0)),

                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            return Tuple.Create(world, cam);
        }
    }
}