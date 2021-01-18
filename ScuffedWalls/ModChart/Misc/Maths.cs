﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ModChart
{
    public struct Point
    {
        public double x { get; set; }
        public double y { get; set; }
    }
    public struct IntVector2
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    static class Maths
    {
        public static float toFloat(this object v)
        {
            return Convert.ToSingle(v.ToString());
        }
        

        public static Point PolarToCartesian(double angle, double radius)
        {
            double angleRad = (Math.PI / 180) * (angle - 90);
            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);
            return new Point() { x = x, y = y };
        }

        public static float Coterminal(this float angle)
        {
            while (angle > 360)
            {
                angle -= 360;
            }
            return angle;
        }

        public static Vector3 QuaternionToEuler(this Quaternion q)
        {
            
            Vector3 euler;

            // if the input quaternion is normalized, this is exactly one. Otherwise, this acts as a correction factor for the quaternion's not-normalizedness
            float unit = (q.X * q.X) + (q.Y * q.Y) + (q.Z * q.Z) + (q.W * q.W);

            // this will have a magnitude of 0.5 or greater if and only if this is a singularity case
            float test = q.X * q.W - q.Y * q.Z;

            if (test > 0.4995f * unit) // singularity at north pole
            {
                euler.X = MathF.PI / 2;
                euler.Y = 2f * MathF.Atan2(q.Y, q.X);
                euler.Z = 0;
            }
            else if (test < -0.4995f * unit) // singularity at south pole
            {
                euler.X = -MathF.PI / 2;
                euler.Y = -2f * MathF.Atan2(q.Y, q.X);
                euler.Z = 0;
            }
            else // no singularity - this is the majority of cases
            {
                euler.X = MathF.Asin(2f * (q.W * q.X - q.Y * q.Z));
                euler.Y = MathF.Atan2(2f * q.W * q.Y + 2f * q.Z * q.X, 1 - 2f * (q.X * q.X + q.Y * q.Y));
                euler.Z = MathF.Atan2(2f * q.W * q.Z + 2f * q.X * q.Y, 1 - 2f * (q.Z * q.Z + q.X * q.X));
            }

            // all the math so far has been done in radians. Before returning, we convert to degrees...
            euler.X *= (180 / (float)Math.PI);
            euler.Y *= (180 / (float)Math.PI);
            euler.Z *= (180 / (float)Math.PI);

            //...and then ensure the degree values are between 0 and 360
            euler.X %= 360;
            euler.Y %= 360;
            euler.Z %= 360;

            return euler;
        }

        public static Quaternion EulerToQuaternion(this Vector3 euler)
        {
            float xOver2 = euler.X * ((float)Math.PI / 180) * 0.5f;
            float yOver2 = euler.Y * ((float)Math.PI / 180) * 0.5f;
            float zOver2 = euler.Z * ((float)Math.PI / 180) * 0.5f;

            float sinXOver2 = MathF.Sin(xOver2);
            float cosXOver2 = MathF.Cos(xOver2);
            float sinYOver2 = MathF.Sin(yOver2);
            float cosYOver2 = MathF.Cos(yOver2);
            float sinZOver2 = MathF.Sin(zOver2);
            float cosZOver2 = MathF.Cos(zOver2);

            Quaternion result;
            result.X = cosYOver2 * sinXOver2 * cosZOver2 + sinYOver2 * cosXOver2 * sinZOver2;
            result.Y = sinYOver2 * cosXOver2 * cosZOver2 - cosYOver2 * sinXOver2 * sinZOver2;
            result.Z = cosYOver2 * cosXOver2 * sinZOver2 - sinYOver2 * sinXOver2 * cosZOver2;
            result.W = cosYOver2 * cosXOver2 * cosZOver2 + sinYOver2 * sinXOver2 * sinZOver2;

            return result;
        }
    }
}