using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Tucan3D_GameEngine.Core;
using Tucan3D_GameEngine.WorldObjects;

namespace Tucan3D_GameEngine.Rendering
{
    public static class MeshRenderer{

        public static void Draw(ShaderData shaderData, WorldObject worldObject)
        {
            shaderData.Start();
            
            shaderData.SetUniform("projectionMatrix", Camera.Current.Projection);
            shaderData.SetUniform("cameraPos", Camera.Current.globalPosition);
            shaderData.SetUniform("lightPos", new Vector3(20000,20000,2000));

            if (worldObject.ModelData != null)
            {
                GL.BindVertexArray(worldObject.ModelData.GetBufferedGeometryId());
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);

                var baseTexture = worldObject.BaseTextureData != null ? worldObject.BaseTextureData : AssetManager.DebugTexture;
                
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, baseTexture.Id);
                shaderData.SetUniform("modelTexture", 0);

                shaderData.SetUniform("hasNormalMap", worldObject.NormalTextureData != null);
                
                if (worldObject.NormalTextureData != null)
                {
                    GL.ActiveTexture(TextureUnit.Texture1);
                    GL.BindTexture(TextureTarget.Texture2D, worldObject.NormalTextureData.Id);
                    shaderData.SetUniform("normalTexture",1);
                }

                if (worldObject.Parent is Camera)
                {
                    shaderData.SetUniform("transformationRelativeLight", Camera.Current.ViewMatrix);
                    shaderData.SetUniform("viewMatrix", Matrix4.Identity);
                    shaderData.SetUniform("transformationMatrix", worldObject.localTransformation);
                    shaderData.SetUniform("lightMatrix", Camera.Current.ViewMatrix);
                }
                else
                {
                    shaderData.SetUniform("transformationRelativeLight", worldObject.globalTransformation);
                    shaderData.SetUniform("viewMatrix", Camera.Current.ViewMatrix);
                    shaderData.SetUniform("transformationMatrix", worldObject.globalTransformation);
                    shaderData.SetUniform("lightMatrix", worldObject.globalTransformation);
                }

                GL.Enable(EnableCap.CullFace);
                GL.CullFace(CullFaceMode.Back);
                GL.DrawElements(PrimitiveType.Triangles, worldObject.ModelData.GetTriangleCount(), DrawElementsType.UnsignedInt,
                    IntPtr.Zero);

                GL.DisableVertexAttribArray(0);
                GL.DisableVertexAttribArray(1);
                GL.DisableVertexAttribArray(2);
                GL.BindVertexArray(0);
            }

            shaderData.Stop();
        }
        
    }
}