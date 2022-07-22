using OpenTK.Graphics.OpenGL;

namespace Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects
{
    public class VerticalBlur
    {
        private ImageRenderer renderer;
        private VerticalBlurShader shader;

        public ImageRenderer Renderer => renderer;
 
        public VerticalBlur(int targetFboWidth, int targetFboHeight) {
            shader = new VerticalBlurShader();
            shader.Start();
            shader.SetTargetHeight(targetFboHeight);
            shader.Stop();
            renderer = new ImageRenderer(targetFboWidth, targetFboHeight);
        }
 
        public void Render(int tex) {
            shader.Start();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tex);
            renderer.Render();
            shader.Stop();
        }

        public int OutputTexture => renderer.OutputTexture;
 
        public void Clear() {
            renderer.Clear();
            shader.Clear();
        }
    }
}