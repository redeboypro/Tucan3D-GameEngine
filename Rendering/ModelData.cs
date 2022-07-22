using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Tucan3D_GameEngine;
using Tucan3D_GameEngine.Core;


namespace Tucan3D_GameEngine
{
    public class ModelData : Asset
    {
        private Vector3[] Vertices;
        private Vector2[] TexCoords;
        private Vector3[] Normals;
        private Vector3[] Tangents;
        private Vector3[] Bitangents;
        private Triangle[] triangles;
        private int[] Indices;
        
        private Vbo<Vector3> VerticesVbo;
        private Vbo<Vector2> TexCoordsVbo;
        private Vbo<Vector3> NormalsVbo;
        private Vbo<Vector3> TangentsVbo;
        private Vbo<Vector3> BitangentsVbo;
        private Vbo<int> IndicesVbo;

        private int TriangleCount, DataId;

        public Vector3[] GetVertices() => Vertices;
        public Vector2[] GetTexCoords() => TexCoords;
        public Vector3[] GetNormals() => Normals;
        public Vector3[] GetTangents() => Tangents;
        public Vector3[] GetBitangents() => Bitangents;
        public int[] GetIndices() => Indices;
        
        public Triangle[] Triangles => triangles;

        public Vbo<Vector3> GetBufferedVertices() => VerticesVbo;
        public Vbo<Vector2> GetBufferedTexCoords() => TexCoordsVbo;
        public Vbo<Vector3> GetBufferedNormals() => NormalsVbo;
        public Vbo<Vector3> GetBufferedTangents() => TangentsVbo;
        public Vbo<Vector3> GetBufferedBitangents() => BitangentsVbo;
        public Vbo<int> GetBufferedIndices() => IndicesVbo;

        public int GetBufferedGeometryId() => DataId;
        public int GetBufferedVerticesId() => VerticesVbo.Id;
        public int GetBufferedTexCoordsId() => TexCoordsVbo.Id;
        public int GetBufferedNormalsId() => NormalsVbo.Id;
        public int GetBufferedIndicesId() => IndicesVbo.Id;
        
        public int GetTriangleCount() => TriangleCount;

        public ModelData(Triangle[] triangles, Vector3[] verts, Vector2[] textures, Vector3[] normals, Vector3[] tangents, Vector3[] bitangents, int[] tris)
        {
            GL.GenVertexArrays(1,out DataId);
            GL.BindVertexArray(DataId);

            this.triangles = triangles;

            Indices = tris;
            Vertices = verts;
            TexCoords = textures;
            Normals = normals;
            Tangents = tangents;
            Bitangents = bitangents;

            IndicesVbo = new Vbo<int>(-1,1, tris, BufferTarget.ElementArrayBuffer);
            VerticesVbo = new Vbo<Vector3>(0, 3, verts, BufferTarget.ArrayBuffer);
            TexCoordsVbo = new Vbo<Vector2>(1, 2, textures, BufferTarget.ArrayBuffer);
            NormalsVbo = new Vbo<Vector3>(2, 3, normals, BufferTarget.ArrayBuffer);
            NormalsVbo = new Vbo<Vector3>(3, 3, tangents, BufferTarget.ArrayBuffer);
            NormalsVbo = new Vbo<Vector3>(4, 3, bitangents, BufferTarget.ArrayBuffer);
            
            GL.BindVertexArray(0);
            TriangleCount = tris.Length;
        }
    }
}