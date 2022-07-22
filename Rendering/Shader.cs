using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Tucan3D_GameEngine
{
    public abstract class Shader
    {
        private int Id, VertexShader, FragmentShader;

        public Shader(string vertex, string fragment)
        {
            VertexShader = LoadShaderFromSource(vertex, ShaderType.VertexShader);
            FragmentShader = LoadShaderFromSource(fragment, ShaderType.FragmentShader);
            Id = GL.CreateProgram();
            GL.AttachShader(Id, VertexShader);
            GL.AttachShader(Id, FragmentShader);
            BindAttributes();
            GL.LinkProgram(Id);
            GL.ValidateProgram(Id);
        }

        public void Clear()
        {
            Stop();
            GL.DetachShader(Id, VertexShader);
            GL.DetachShader(Id, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteProgram(Id);
        }

        public int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(Id, uniformName);
        }

        public void Start()
        {
            GL.UseProgram(Id);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        public abstract void BindAttributes();

        public void BindAttribute(int attribute, string variableName)
        {
            GL.BindAttribLocation(Id, attribute, variableName);
        }

        public void SetUniform(string location, float value)
        {
            GL.Uniform1(GetUniformLocation(location), value);
        }

        public void SetUniform(string location, int value)
        {
            GL.Uniform1(GetUniformLocation(location), value);
        }

        public void SetUniform(string location, double value)
        {
            GL.Uniform1(GetUniformLocation(location), value);
        }

        public void SetUniform(string location, Vector3 value)
        {
            GL.Uniform3(GetUniformLocation(location), value);
        }

        public void SetUniform(string location, Vector4 value)
        {
            GL.Uniform4(GetUniformLocation(location), value);
        }

        public void SetUniform(string location, Vector2 value)
        {
            GL.Uniform2(GetUniformLocation(location), value);
        }

        public void SetUniform(string location, Color4 value)
        {
            GL.Uniform4(GetUniformLocation(location), value);
        }

        public void SetUniform(string location, Matrix4 value)
        {
            GL.UniformMatrix4(GetUniformLocation(location), false, ref value);
        }

        public void SetUniform(string location, Matrix3 value)
        {
            GL.UniformMatrix3(GetUniformLocation(location), false, ref value);
        }

        public void SetUniform(string location, bool value)
        {
            float FinalValue = value ? 1 : 0;
            GL.Uniform1(GetUniformLocation(location), FinalValue);
        }

        private static int LoadShaderFromSource(string source, ShaderType type)
        {
            int ShaderID = GL.CreateShader(type);
            GL.ShaderSource(ShaderID, source);
            GL.CompileShader(ShaderID);
            string log = GL.GetShaderInfoLog(ShaderID);

            if (!string.IsNullOrEmpty(log))
            {
                throw new Exception(log);
            }

            return ShaderID;
        }
    }
}