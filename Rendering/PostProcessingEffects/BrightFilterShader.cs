namespace Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects
{
    public class BrightFilterShader : Shader
    {
        private static string Vertex = @"
#version 150

in vec2 position;

out vec2 textureCoords;

void main(void){

	gl_Position = vec4(position, 0.0, 1.0);
	textureCoords = position * 0.5 + 0.5;
}";

        private static string Fragment = @"
#version 150

in vec2 textureCoords;

out vec4 out_Colour;

uniform sampler2D colourTexture;

void main(void){
  vec4 colour = texture(colourTexture, textureCoords);
  float brightness = (colour.r*0.4) + (colour.g*0.4) + (colour.b*0.01);
  out_Colour = colour * brightness;
}";

        public BrightFilterShader() : base(Vertex, Fragment)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
        }
        
    }
}