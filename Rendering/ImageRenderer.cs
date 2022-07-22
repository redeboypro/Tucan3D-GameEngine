using OpenTK.Graphics.OpenGL;

namespace Tucan3D_GameEngine.Rendering.Common
{
    public class ImageRenderer
    {
        public Fbo fbo;

        public ImageRenderer(int width, int height) {
            this.fbo = new Fbo(width, height, Fbo.NONE);
        }

        public ImageRenderer() {}

        public void Render() {
            fbo?.BindFrameBuffer();
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            fbo?.UnbindFrameBuffer();
        }

        public int OutputTexture => fbo.ColourTexture;

        public void Clear() => fbo.Clear();

    }
}