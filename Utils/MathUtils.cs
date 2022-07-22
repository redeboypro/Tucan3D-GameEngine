using System;
using OpenTK;

namespace Tucan3D_GameEngine
{
    public class MathUtils
    {
        public static Vector2 GetNormalisedCoordinates(float x, float y, float w = 800, float h = 600) {
            float X = (2.0f * x) / w - 1f;
            float Y = (2.0f * y) / h - 1f;
            return new Vector2(X, -Y);
        }
        
        public static Vector2 GetUnormalisedCoordinates(float x, float y, float w = 800, float h = 600) {
            float X = (x + 1) * w / 2;
            float Y = (-y + 1) * h / 2;
            return new Vector2(X, Y);
        }
        
        public static Vector3 VectorAxesToRadians(Vector3 value) => new Vector3(MathHelper.DegreesToRadians(value.X),
            MathHelper.DegreesToRadians(value.Y), MathHelper.DegreesToRadians(value.Z));

        public static Vector3 ToEulerAngles(Quaternion rot)
        {
            var q1 = rot;
            float sqw = q1.W * q1.W;
            float sqx = q1.X * q1.X;
            float sqy = q1.Y * q1.Y;
            float sqz = q1.Z * q1.Z;
            float unit = sqx + sqy + sqz + sqw;
            float test = q1.X * q1.W - q1.Y * q1.Z;
            var v = new Vector3();

            if (test > 0.4995f * unit)
            {
                v.Y = 2f * (float) Math.Atan2(q1.Y, q1.X);
                v.X = (float) Math.PI / 2f;
                v.Z = 0;
                return NormalizeAngles(v);
            }
            if (test < -0.4995f * unit)
            {
                v.Y = -2f * (float)Math.Atan2(q1.Y, q1.X);
                v.X = (float)-Math.PI / 2;
                v.Z = 0;
                return NormalizeAngles(v);
            }
 
            rot = new Quaternion(q1.W, q1.Z, q1.X, q1.Y);
            v.Y = (float)Math.Atan2(2f * rot.X * rot.W + 2f * rot.Y * rot.Z, 1 - 2f * (rot.Z * rot.Z + rot.W * rot.W));
            v.X = (float)Math.Asin(2f * (rot.X * rot.Z - rot.W * rot.Y));
            v.Z = (float)Math.Atan2(2f * rot.X * rot.Y + 2f * rot.Z * rot.W, 1 - 2f * (rot.Y * rot.Y + rot.Z * rot.Z));
            return NormalizeAngles(v);
        }
 
        private static Vector3 NormalizeAngles(Vector3 angles)
        {
            angles.X = MathHelper.RadiansToDegrees(NormalizeAngle(angles.X));
            angles.Y = MathHelper.RadiansToDegrees(NormalizeAngle(angles.Y));
            angles.Z = MathHelper.RadiansToDegrees(NormalizeAngle(angles.Z));
            return angles;
        }
 
        private static float NormalizeAngle(float angle)
        {
            while (angle > Math.PI * 2f)
                angle -= (float)Math.PI * 2f;
            while (angle < 0)
                angle += (float)Math.PI * 2f;
            return angle;
        }
    }
}