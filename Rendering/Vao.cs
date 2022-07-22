using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Tucan3D_GameEngine.Rendering.Common
{
    public class VertexArrayData
    {
        private Int32 id;
        public Int32 Id => id;
        
        private Int32 vertexCount;
        public Int32 VertexCount => vertexCount;
        
        public VertexArrayData(float[] pos, int dim)
        {
            int vaoID;
            GL.GenVertexArrays(1, out vaoID);
            GL.BindVertexArray(vaoID);
            
            StoreDataInAttributeList(0, dim, pos);
            
            GL.BindVertexArray(0);
            id = vaoID;
            vertexCount = pos.Length/dim;
        }
        
        public VertexArrayData(float[] pos, float[] tex)
        {
            int vaoID;
            GL.GenVertexArrays(1, out vaoID);
            GL.BindVertexArray(vaoID);
            StoreDataInAttributeList(0, 2, pos);
            StoreDataInAttributeList(1, 2, tex);
            GL.BindVertexArray(0);
            id = vaoID;
        }
        
        public static int QuicklyLoadToVao(Vector2[] pos, Vector2[] tex)
        {
            int vaoID;
            GL.GenVertexArrays(1, out vaoID);
            GL.BindVertexArray(vaoID);
            StoreDataInAttributeList(0, 2, pos);
            StoreDataInAttributeList(1, 2, tex);
            GL.BindVertexArray(0);
            return vaoID;
        }


        private static void StoreDataInAttributeList<T>(int attributeNumber, int coordinateSize,T[] data) where T : struct
        {
            int vboID;
            GL.GenBuffers(1, out vboID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (data.Length * Marshal.SizeOf(typeof(T))), data, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(attributeNumber,coordinateSize,VertexAttribPointerType.Float,false,0,0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}