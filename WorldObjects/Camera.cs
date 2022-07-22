using System;
using OpenTK;
using Tucan3D_GameEngine;
using Tucan3D_GameEngine.WorldObjects;

namespace Tucan3D_GameEngine
{
    public class Camera : WorldObject
    {
        private static Camera _current;
        public static Camera Current => _current;

        public Camera(float fov = MathHelper.PiOver4) : base(string.Empty)
        {
            _current = this;
            projection = Matrix4.CreatePerspectiveFieldOfView(fov, 800 / (float)600, 0.01f, 1000.0f);
        }

        private Vector3 _forward = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;
        
        public Vector3 ForwardDirectionRelativeCamera => _forward;
        public Vector3 RightDirectionRelativeCamera => _right;
        public Vector3 UpDirectionRelativeCamera => _up;
        
        private float _pitch;
        private float _yaw = MathHelper.DegreesToRadians(90);
        
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -90f, 90f);

                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateDirectionVectors();
            }
        }
        
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateDirectionVectors();
            }
        }
        
        private Vector3 GetWorldCoords(Vector4 eyeCoords) {
            Matrix4 invertedView = ViewMatrix.Inverted();
            Vector4 rayWorld = eyeCoords * invertedView;
            Vector3 mouseRay = new Vector3(rayWorld.X, rayWorld.Y, rayWorld.Z);
            mouseRay.Normalized();
            return mouseRay;
        }
        
        private Vector4 GetEyeCoords(Vector4 clipCoords) {
            Matrix4 invertedProjection = projection.Inverted();
            Vector4 eyeCoords = clipCoords * invertedProjection;
            return new Vector4(eyeCoords.X, eyeCoords.Y, -1f, 0f);
        }
        
        public Vector3 ScreenToWorldCoords(float x, float y)
        {
            Vector2 normalizedCoords = MathUtils.GetNormalisedCoordinates(x, y);
            Vector4 clipCoords = new Vector4(normalizedCoords.X, normalizedCoords.Y, -1.0f, 1.0f);
            Vector4 eyeCoords = GetEyeCoords(clipCoords);
            Vector3 worldRay = GetWorldCoords(eyeCoords);
            return worldRay;
        }
        
        private void UpdateDirectionVectors()
        {
            _forward.X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw);
            _forward.Y = (float)Math.Sin(_pitch);
            _forward.Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw);
            _forward.Normalize();
            _right = Vector3.Normalize(Vector3.Cross(_forward, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _forward));
            
            Vector3 euler;
            euler.X = Pitch;
            euler.Y = Yaw;
            euler.Z = 0;
            localEulerAngles = euler;
        }
        
        public Matrix4 ViewMatrix => Matrix4.LookAt(globalPosition, globalPosition + _forward, _up);
        
        private Matrix4 projection;
        public Matrix4 Projection => projection;
    }
}