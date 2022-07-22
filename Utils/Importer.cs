using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Assimp;
using Assimp.Configs;
using OpenTK;
using Tucan3D_GameEngine;

namespace OpenGL_3D_GameEngine
{
    public class Importer
    {
        public static ModelData LoadFromFile(string filename)
        {
            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            var scene = importer.ImportFile(filename, PostProcessSteps.FlipUVs | PostProcessSteps.CalculateTangentSpace);
            var mesh = scene.Meshes[0];
            var faces = mesh.Faces;

            var triangles = new List<Triangle>();

            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var tangents = new List<Vector3>();
            var bitantents = new List<Vector3>();
            var texCoords = new List<Vector2>();
            var elements = new List<int>();

            foreach (var face in faces)
            {
                var v1 = FromVector(mesh.Vertices[face.Indices[0]]);
                var v2 = FromVector(mesh.Vertices[face.Indices[1]]);
                var v3 = FromVector(mesh.Vertices[face.Indices[2]]);
                
                var n = FromVector(mesh.Normals[face.Indices[0]]);
                
                triangles.Add(new Triangle(v1, v2, v3, n));

                for (int i = 0; i < face.IndexCount; i++)
                {
                    int index = face.Indices[i];
                    Vector3 uv = FromVector(mesh.TextureCoordinateChannels[0][index]);
                    Vector3 position = FromVector(mesh.Vertices[index]);
                    Vector3 normal = FromVector(mesh.Normals[index]);
                    Vector3 tangent = FromVector(mesh.Tangents[index]);
                    Vector3 bitangent = FromVector(mesh.BiTangents[index]);

                    vertices.Add(position);
                    normals.Add(normal);
                    texCoords.Add(new Vector2(uv.X, uv.Y));
                    elements.Add(index);
                    tangents.Add(tangent);
                    bitantents.Add(bitangent);
                }
            }

            return new ModelData(triangles.ToArray(), vertices.ToArray(), 
                texCoords.ToArray(), normals.ToArray(), tangents.ToArray(), bitantents.ToArray(), elements.ToArray());
        }

        private static Vector3 FromVector(Vector3D vec)
        {
            Vector3 v;
            v.X = vec.X;
            v.Y = vec.Y;
            v.Z = vec.Z;
            return v;
        }
    }
}