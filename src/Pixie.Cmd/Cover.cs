namespace Pixie.Cmd
{
    using System;
    using Pixie.Core;

    public class Cover
    {
        public static Tuple<World, Camera> Create(int width, int height)
        {
            var camera = new Camera(width, height, 0.785)
            {
                Transform =
                    Transform.View(
                        Double4.Point(-6, 6, -10),
                        Double4.Point(6, 0, 6),
                        Double4.Point(-0.45, 1, 0)),

                ProgressMonitor = 
                    new ParallelConsoleProgressMonitor(height),
            };

            var light1 = new PointLight(
                Double4.Point(50, 100, -50),
                new Color(1, 1, 1));

            var light2 = new PointLight(
                Double4.Point(-400, 50, -10),
                new Color(0.2, 0.2, 0.2));

            var whiteMaterial = new Material()
            {
                Color = new Color(1, 1, 1),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0.0,
                Reflective = 0.1,
            };

            var blueMaterial = whiteMaterial.Extend(m =>
            {
                m.Color = new Color(0.537, 0.831, 0.914);
            });

            var redMaterial = whiteMaterial.Extend(m =>
            {
                m.Color = new Color(0.941, 0.322, 0.388);
            });

            var purpleMaterial = whiteMaterial.Extend(m =>
            {
                m.Color = new Color(0.373, 0.404, 0.550);
            });

            var standardTransform =
                Transform.Translate(1, -1, 1) *
                Transform.Scale(0.5, 0.5, 0.5);

            var largeObject =
                standardTransform * Transform.Scale(3.5, 3.5, 3.5);

            var mediumObject =
                standardTransform * Transform.Scale(3, 3, 3);

            var smallObject =
                standardTransform * Transform.Scale(2, 2, 2);

            var plane = new Plane()
            {
                Material = new Material()
                {
                    Color = new Color(1, 1, 1),
                    Ambient = 1,
                    Diffuse = 0,
                    Specular = 0,
                },

                Transform = 
                    Transform.Translate(0, 0, 500) *
                    Transform.RotateX(Math.PI / 2),
            };

            var sphere = new Sphere()
            {
                Material = new Material()
                {
                    Color = new Color(0.373, 0.404, 0.550),
                    Diffuse = 0.2,
                    Ambient = 0.0,
                    Specular = 1.0,
                    Shininess = 200,
                    Reflective = 0.7,
                    Transparency = 0.7,
                    RefractiveIndex = 1.5,
                },

                Transform = largeObject,
            };

            var cube1 = new Cube()
            {
                Material = whiteMaterial,
                Transform = 
                    Transform.Translate(4, 0, 0) * mediumObject,
            };

            var cube2 = new Cube()
            {
                Material = blueMaterial,
                Transform =
                    Transform.Translate(8.5, 1.5, -0.6) * largeObject,
            };

            var cube3 = new Cube()
            {
                Material = redMaterial,
                Transform =
                    Transform.Translate(0, 0, 4) * largeObject,
            };

            var cube4 = new Cube()
            {
                Material = whiteMaterial,
                Transform =
                    Transform.Translate(4, 0, 4) * smallObject,
            };

            var cube5 = new Cube()
            {
                Material = purpleMaterial,
                Transform =
                    Transform.Translate(7.5, 0.5, 4) * mediumObject,
            };

            var cube6 = new Cube()
            {
                Material = whiteMaterial,
                Transform =
                    Transform.Translate(-0.25, 0.25, 8) * mediumObject,
            };

            var cube7 = new Cube()
            {
                Material = blueMaterial,
                Transform =
                    Transform.Translate(4, 1, 7.5) * largeObject,
            };

            var cube8 = new Cube()
            {
                Material = redMaterial,
                Transform =
                    Transform.Translate(10, 2, 7.5) * mediumObject,
            };

            var cube9 = new Cube()
            {
                Material = whiteMaterial,
                Transform =
                    Transform.Translate(8, 2, 12) * smallObject,
            };

            var cube10 = new Cube()
            {
                Material = whiteMaterial,
                Transform =
                    Transform.Translate(20, 1, 9) * smallObject,
            };

            var cube11 = new Cube()
            {
                Material = blueMaterial,
                Transform =
                    Transform.Translate(-0.5, -5, 0.25) * largeObject,
            };

            var cube12 = new Cube()
            {
                Material = redMaterial,
                Transform =
                    Transform.Translate(4, -4, 0) * largeObject,
            };

            var cube13 = new Cube()
            {
                Material = whiteMaterial,
                Transform =
                    Transform.Translate(8.5, -4, 0) * largeObject,
            };

            var cube14 = new Cube()
            {
                Material = whiteMaterial,
                Transform =
                    Transform.Translate(0, -4, 4) * largeObject,
            };

            var cube15 = new Cube()
            {
                Material = purpleMaterial,
                Transform = 
                    Transform.Translate(-0.5, -4.5, 8) * largeObject,
            };

            var cube16 = new Cube()
            {
                Material = whiteMaterial,
                Transform =
                    Transform.Translate(0, -8, 4) * largeObject,
            };

            var cube17 = new Cube()
            {
                Material = whiteMaterial,
                Transform =
                    Transform.Translate(-0.5, -8.5, 8) * largeObject,
            };

            var world = new World();
            
            world.Objects.Add(plane);            
            world.Objects.Add(sphere);
            world.Objects.Add(cube1);
            world.Objects.Add(cube2);
            world.Objects.Add(cube3);
            world.Objects.Add(cube4);
            world.Objects.Add(cube5);
            world.Objects.Add(cube6);
            world.Objects.Add(cube7);
            world.Objects.Add(cube8);
            world.Objects.Add(cube9);
            world.Objects.Add(cube10);
            world.Objects.Add(cube11);
            world.Objects.Add(cube12);
            world.Objects.Add(cube13);
            world.Objects.Add(cube14);
            world.Objects.Add(cube15);
            world.Objects.Add(cube16);
            world.Objects.Add(cube17);
            world.Lights.Add(light1);
            // world.Lights.Add(light2);

            return Tuple.Create(world, camera);
        }
    }
}