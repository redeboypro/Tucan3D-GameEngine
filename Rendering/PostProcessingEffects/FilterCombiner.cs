using OpenTK.Graphics.OpenGL;

namespace Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects
{
    public class FilterCombiner
    {
        private ImageRenderer renderer;
        private FilterCombinerShader shader;

        public ImageRenderer Renderer => renderer;
 
        public FilterCombiner() {
            shader = new FilterCombinerShader();
            shader.Start();
            shader.ConnectTextureUnits();
            shader.Stop();
            renderer = new ImageRenderer();
        }
 
        public void Render(int tex, int highlightTexture) {
            shader.Start();
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tex);
            
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, highlightTexture);
            
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