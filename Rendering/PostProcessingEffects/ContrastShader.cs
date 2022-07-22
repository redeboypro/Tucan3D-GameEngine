namespace Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects
{
    public class ContrastShader : Shader
    {
        private static string Vertex = @"
#version 140

in vec2 position;

out vec2 textureCoords;

void main(void){

	gl_Position = vec4(position, 0.0, 1.0);
	textureCoords = position * 0.5 + 0.5;
	
}";

        private static string Fragment = @"
#version 140

in vec2 textureCoords;

out vec4 out_Colour;

uniform sampler2D colourTexture;

const float contrast = 1.3;

void main(void){

	out_Colour = texture(colourTexture, textureCoords);
	out_Colour.rgb = (out_Colour.rgb -0.5) * (1.0+contrast) + 0.5;

}";

        public ContrastShader() : base(Vertex, Fragment)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
        }
    }
}