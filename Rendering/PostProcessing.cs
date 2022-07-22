using Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Tucan3D_GameEngine.Core;
using Tucan3D_GameEngine.Gui;

namespace Tucan3D_GameEngine.Rendering.Common
{
    public class PostProcessing
    {
        private static float[] QUAD_VERTICES = { -1,1,  -1,-1,  1,1,  1,-1 };
        private static VertexArrayData vertexData;
        private static HorizontalBlur horizontalBlur;
        private static VerticalBlur verticalBlur;
        private static BrightFilter brightness;
        private static FilterCombiner combiner;
        private static Contraster contraster;

        public static void Initialize()
        {
            vertexData = new VertexArrayData(QUAD_VERTICES, 2);
            contraster = new Contraster(800, 600);
            horizontalBlur = new HorizontalBlur(800/6,600/6);
            verticalBlur = new VerticalBlur(800/6,600/6);
            brightness = new BrightFilter(800/5, 600/5);
            combiner = new FilterCombiner();
        }
        
        public static void Update(int colourTexture){
            contraster.Render(colourTexture);
            brightness.Render(contraster.OutputTexture);
            horizontalBlur.Render(brightness.OutputTexture);
            verticalBlur.Render(horizontalBlur.OutputTexture);
            combiner.Render(colourTexture, verticalBlur.OutputTexture);
            GL.BindVertexArray(0);
        }
	
        public static void Clear(){
            horizontalBlur.Clear();
            verticalBlur.Clear();
            brightness.Clear();
            combiner.Clear();
        }
	
        public static void Start(){
            GL.BindVertexArray(vertexData.Id);
            GL.EnableVertexAttribArray(0);
            GL.Disable(EnableCap.DepthTest);
        }
	
        public static void End(){
            GL.DisableVertexAttribArray(0);
            GL.Enable(EnableCap.DepthTest);
        }
    }
}