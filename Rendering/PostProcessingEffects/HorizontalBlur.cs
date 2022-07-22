using OpenTK.Graphics.OpenGL;

namespace Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects
{
    public class HorizontalBlur
    {
        private ImageRenderer renderer;
        private HorizontalBlurShader shader;

        public ImageRenderer Renderer => renderer;
 
        public HorizontalBlur(int targetFboWidth, int targetFboHeight) {
            shader = new HorizontalBlurShader();
            shader.Start();
            shader.SetTargetWidth(targetFboWidth);
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