using System.Windows;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Tucan3D_GameEngine.Rendering;

namespace Tucan3D_GameEngine.Gui
{
    public class Image2D : UIElement
    {
        private TextureData textureData;
        private int vao;
        private GUIShader shader;

        public Image2D(GUIShader shader, TextureData textureData)
        {
            this.shader = shader;
            this.vao = Gui.QuadVertexData.Id;
            this.textureData = textureData;
        }
        
        public override void Draw()
        {
            shader.Start();
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureData.Id);
            shader.SetUniform("imageTexture",0);

            Vector3 positionT;
            positionT.X = Position.X;
            positionT.Y = Position.Y;
            positionT.Z = 0;
            
            Vector3 scaleT;
            scaleT.X = scale.X;
            scaleT.Y = scale.Y;
            scaleT.Z = 1;
            
            var transformation = Matrix4.CreateScale(scaleT) * Matrix4.CreateTranslation(positionT);
            shader.SetUniform("transformation", transformation);
            
            GL.BindVertexArray(vao);
            GL.EnableVertexAttribArray(0);
            
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
            
            shader.Stop();
        }
    }
}