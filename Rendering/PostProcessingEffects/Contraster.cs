using OpenTK.Graphics.OpenGL;

namespace Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects
{
    public class Contraster
    {
        private ImageRenderer renderer;
        private ContrastShader shader;

        public ImageRenderer Renderer => renderer;
 
        public Contraster(int width, int height) {
            shader = new ContrastShader();
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