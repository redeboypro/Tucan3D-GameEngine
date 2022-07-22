using OpenTK.Graphics.OpenGL;

namespace Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects
{
    public class BrightFilter
    {
        private ImageRenderer renderer;
        private BrightFilterShader shader;

        public ImageRenderer Renderer => renderer;
 
        public BrightFilter(int width, int height) {
            shader = new BrightFilterShader();
            renderer = new ImageRenderer(width, height);
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