namespace Pixie
{
    using System;
    using System.Collections.Generic;

    // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.graphics?view=netframework-4.7.2
    public interface IGraphics
    {
        float Width { get; }

        float Height { get; }

        void DrawLine(IPen pen, float x1, float y1, float x2, float y2);
    }

    public interface IPen
    {
        Color Color { get; }

        float Width { get; }
    }

    public class Turtle : IDisposable
    {
        const float DefaultWidth = 3.5f;

        private bool disposed;

        private IGraphics gfx;

        private Stack<Tuple<Vector2, Vector2>> memory = new Stack<Tuple<Vector2, Vector2>>();
        
        private Vector2 heading = Vector2.UnitX;

        private Vector2 position = Vector2.Zero;

        private bool isPenDown = false;

        private Func<float, Color, IPen> penFactory;

        public Vector2 Heading => this.heading;

        public Vector2 Position => this.position;

        public IPen Pen
        {
            get;
            set;
        }

        public bool IsPenDown => this.isPenDown;

        public Turtle(IGraphics gfx)
            : this(gfx, (w, c) => new WhitePen(w))
        {
        }

        public Turtle(IGraphics gfx, Func<float, Color, IPen> penFactory)
        {
            this.gfx = gfx;
            this.penFactory = penFactory;
            this.Pen = penFactory(1.5f, Color.White);
        }

        public Turtle Turn(float degrees)
        {
            var rad = ToRadians(degrees);
            var rotm = Matrix2.CreateRotation(rad);
            this.heading = heading.Multiply(rotm);
            return this;
        }

        public Turtle Forward(float distance)
        {
            var dx = this.heading.X * distance;
            var dy = this.heading.Y * distance;
            var newLocation = position.Translate(dx, dy);
            if (this.isPenDown)
            {
                this.gfx.DrawLine(
                    this.Pen,
                    this.position.X,
                    this.position.Y,
                    newLocation.X,
                    newLocation.Y);
            }

            this.position = newLocation;
            return this;
        }

        public Turtle PenDown()
        {
            this.isPenDown = true;
            return this;
        }

        public Turtle PenUp()
        {
            this.isPenDown = false;
            return this;
        }

        public Turtle Push()
        {
            var t = Tuple.Create(this.position, this.heading);
            this.memory.Push(t);
            return this;
        }

        public Turtle Pop()
        {
            var t = this.memory.Pop();
            this.position = t.Item1;
            this.heading = t.Item2;
            return this;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        static float ToRadians(float deg) => (float)(deg * 2 * Math.PI / 360);
    }

    internal class WhitePen : IPen
    {
        private float width;

        public WhitePen(float width)
        {
            this.width = width;
        }

        public Color Color => new Color(255, 255, 255, 255);

        public float Width => this.width;
    }

    internal static class Extensions
    {
        [Obsolete("Use matrix operations instead")]
        public static Vector2 Rotate(this Vector2 a, double theta)
        {
            var x = (float)Math.Round(a.X * Math.Cos(theta) - a.Y * Math.Sin(theta), 1);
            var y = (float)Math.Round(a.X * Math.Sin(theta) + a.Y * Math.Cos(theta), 1);
            return new Vector2(x, y);
        }

        public static Vector2 Multiply(this Vector2 a, Matrix2 b)
        {
            var x = b.M11 * a.X + b.M12 * a.Y;
            var y = b.M21 * a.X + b.M22 * a.Y;
            return new Vector2(x, y);
        }

        public static Vector2 Translate(this Vector2 a, float dx, float dy) =>
            new Vector2(a.X + dx, a.Y + dy);
    }
}