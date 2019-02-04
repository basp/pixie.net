﻿namespace Pixie.Core
{
    using System;

    public struct Float4
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;
        public readonly float W;

        public Float4(float v)
        {
            this.X = v;
            this.Y = v;
            this.Z = v;
            this.W = v;
        }

        public Float4(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public bool IsPoint => this.W == 1.0;

        public bool IsVector => this.W == 0.0;

        public static Float4 Point(float x, float y, float z) =>
            new Float4(x, y, z, 1);

        public static Float4 Vector(float x, float y, float z) =>
            new Float4(x, y, z, 0);

        public static Float4 operator +(Float4 a, Float4 b) =>
            new Float4(
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z,
                a.W + b.W);

        public static Float4 operator -(Float4 a, Float4 b) =>
            new Float4(
                a.X - b.X,
                a.Y - b.Y,
                a.Z - b.Z,
                a.W - b.W);

        public static Float4 operator -(Float4 a) =>
            new Float4(-a.X, -a.Y, -a.Z, -a.W);

        public static Float4 operator *(Float4 a, float s) =>
            new Float4(
                a.X * s,
                a.Y * s,
                a.Z * s,
                a.W * s);

        public static Float4 operator *(float s, Float4 a) => a * s;

        public static Float4 operator /(Float4 a, float s) =>
            new Float4(
                a.X / s,
                a.Y / s,
                a.Z / s,
                a.W / s);

        public override string ToString() =>
            $"({this.X}, {this.Y}, {this.Z}, {this.W})";
    }

    public static class Float4Extensions
    {
        public static float MagnitudeSquared(this Float4 a) =>
            a.X * a.X + a.Y * a.Y + a.Z * a.Z + a.W * a.W;

        public static float Magnitude(this Float4 a) =>
            (float)Math.Sqrt(a.MagnitudeSquared());

        public static Float4 Normalize(this Float4 a)
        {
            var mag = a.Magnitude();
            return new Float4(
                a.X / mag,
                a.Y / mag,
                a.Z / mag,
                a.W / mag);
        }

        public static float Dot(this Float4 a, Float4 b) =>
            a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;


        public static Float4 Cross(this Float4 a, Float4 b) =>
            Float4.Vector(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
    }
}
