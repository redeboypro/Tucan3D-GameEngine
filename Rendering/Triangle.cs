using System;
using OpenTK;

namespace Tucan3D_GameEngine
{
    public class Triangle
    {
        private Vector3[] vertices = new Vector3[3];
        private Vector3 normal;

        public Vector3 P1
        {
            get => vertices[0];
            set => vertices[0] = value;
        }
        
        public Vector3 P2
        {
            get => vertices[1];
            set => vertices[1] = value;
        }
        
        public Vector3 P3
        {
            get => vertices[2];
            set => vertices[2] = value;
        }
        
        public Vector3 Normal
        {
            get => normal;
            set => normal = value;
        }

        public Triangle(Vector3[] vertices, Vector3 normal)
        {
            this.normal = normal;
            this.vertices = vertices;
        }

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 normal)
        {
            this.normal = normal;
            this.vertices = new Vector3[] {v1, v2, v3};
        }

        public bool Raycast(Vector3 origin, Vector3 dir, out Vector3 hitpoint)
        {
            var N = normal;
            var orig = origin;

            float NdotRayDirection = Vector3.Dot(N, dir);
            if (Math.Abs(NdotRayDirection) < float.Epsilon)
            {
                hitpoint = origin + dir;
                return false;
            }

            float d = -Vector3.Dot(N, vertices[0]);
            var t = -(Vector3.Dot(N, orig) + d) / NdotRayDirection;

            if (t < 0)
            {
                hitpoint = origin + dir;
                return false;
            }

            hitpoint = orig + t * dir;
            Vector3 C;

            Vector3 edge0 = vertices[1] - vertices[0];
            Vector3 vp0 = hitpoint - vertices[0];
            C = Vector3.Cross(edge0, vp0);
            if (Vector3.Dot(N, C) < 0) 
                return false;

            Vector3 edge1 = vertices[2] - vertices[1];
            Vector3 vp1 = hitpoint - vertices[1];
            C = Vector3.Cross(edge1, vp1);
            if (Vector3.Dot(N, C) < 0) 
                return false;

            Vector3 edge2 = vertices[0] - vertices[2];
            Vector3 vp2 = hitpoint - vertices[2];
            C = Vector3.Cross(edge2, vp2);
            if (Vector3.Dot(N, C) < 0) 
                return false;

            return true;
        }
        
        public float CalculateHeightAtPoint(Vector3 translation)
        {
            float a = 
                -(vertices[2].Z * vertices[1].Y
                  - vertices[0].Z * vertices[1].Y
                  - vertices[2].Z * vertices[0].Y
                  + vertices[0].Y * vertices[1].Z
                  + vertices[2].Y * vertices[0].Z
                  - vertices[1].Z * vertices[2].Y);

            float b = 
                (vertices[0].Z * vertices[2].X
                 + vertices[1].Z * vertices[0].X
                 + vertices[2].Z * vertices[1].X 
                 - vertices[1].Z * vertices[2].X 
                 - vertices[0].Z * vertices[1].X 
                 - vertices[2].Z * vertices[0].X);

            float c = 
                (vertices[1].Y * vertices[2].X
                 + vertices[0].Y * vertices[1].X
                 + vertices[2].Y * vertices[0].X
                 - vertices[0].Y * vertices[2].X
                 - vertices[1].Y * vertices[0].X
                 - vertices[1].X * vertices[2].Y);

            float d = 
                - a * vertices[0].X
                - b * vertices[0].Y
                - c * vertices[0].Z;

            return -(a * translation.X + c * translation.Z + d) / b;
        }
    }
}