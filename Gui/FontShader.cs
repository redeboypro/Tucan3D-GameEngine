namespace Tucan3D_GameEngine.Gui
{
    public class FontShader : Shader
    {
        private static string Vertex = @"
#version 150

in vec2 position;
in vec2 textureCoordinates;

uniform mat4 charMatrix;

out vec2 pass_textureCoordinates;

void main(void){

	gl_Position = charMatrix * vec4(position, 0.0, 1.0);
	pass_textureCoordinates = textureCoordinates;
	
}";

        private static string Fragment = @"
#version 150

in vec2 pass_textureCoordinates;

out vec4 out_Color;

uniform bool fillBG;
uniform sampler2D charTexture;
uniform vec4 textColor;

void main(void){
	out_Color = texture(charTexture,pass_textureCoordinates) * textColor;
    if(fillBG)
        out_Color.a = 1;
}";

        public FontShader() : base(Vertex, Fragment)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCoordinates");
        }
    }
}