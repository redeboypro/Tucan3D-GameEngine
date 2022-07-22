namespace Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects
{
    public class FilterCombinerShader : Shader
    {
        private static string Vertex = @"
#version 150

in vec2 position;

out vec2 pass_position;
out vec2 textureCoords;

void main(void){

	gl_Position = vec4(position, 0.0, 1.0);
	textureCoords = position * 0.5 + 0.5;
	pass_position = position;
}";

        private static string Fragment = @"
#version 150

in vec2 pass_position;
in vec2 textureCoords;

out vec4 out_Colour;

uniform sampler2D colourTexture;
uniform sampler2D highlightTexture;

const float max = pow(0.3, 1);

void main(void){
   vec4 sceneColour = texture(colourTexture, textureCoords);
   vec4 highlight = texture(highlightTexture, textureCoords);

   vec2 uv = textureCoords;
   uv *=  1.0 - uv.yx;
   float vignette = uv.x * uv.y * 50.0; 
   sceneColour.rgb = sceneColour.rgb * smoothstep(0, max, vignette);

   out_Colour = (sceneColour + highlight);
}";

        public FilterCombinerShader() : base(Vertex, Fragment)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
        }

        public void ConnectTextureUnits()
        {
            SetUniform("colorTexture", 0);
            SetUniform("highlightTexture", 1);
        }
        
    }
}