namespace Tucan3D_GameEngine.Gui
{
    public class GUIShader : Shader
    {
        private static string Vertex = @"
#version 150

in vec2 position;
in vec2 textureCoordinates;

uniform mat4 transformation;

out vec2 pass_textureCoordinates;

void main(void){

	gl_Position = transformation * vec4(position, 0.0, 1.0);
	pass_textureCoordinates = position * -1 + 1;
}";

        private static string Fragment = @"
#version 150

in vec2 pass_textureCoordinates;

out vec4 out_Color;

uniform sampler2D imageTexture;

void main(void){
	out_Color = texture(imageTexture,pass_textureCoordinates);
}";

        public GUIShader() : base(Vertex, Fragment)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
        }
    }
}