namespace Tucan3D_GameEngine.Rendering.Common.PostProcessingEffects
{
    public class VerticalBlurShader : Shader
    {
        private static string Vertex = @"
#version 150

out vec2 blurTextureCoords[11];

in vec2 position;
out vec2 pass_position;

uniform float targetHeight;

void main(void){

	gl_Position = vec4(position, 0.0, 1.0);
	vec2 centerTexCoords = position * 0.5 + 0.5;
    float pixelSize = 1.0 / targetHeight;
    
    for(int i = -5; i<=5; i++){
        blurTextureCoords[i+5] = centerTexCoords + vec2(0.0, pixelSize * i); 
    }

    pass_position = position;
}";

        private static string Fragment = @"
#version 150

out vec4 out_colour;

in vec2 pass_position;
in vec2 blurTextureCoords[11];

uniform sampler2D originalTexture;

void main(void){
	
	out_colour = vec4(0.0);
	out_colour += texture(originalTexture, blurTextureCoords[0]) * 0.0093;
    out_colour += texture(originalTexture, blurTextureCoords[1]) * 0.028002;
    out_colour += texture(originalTexture, blurTextureCoords[2]) * 0.065984;
    out_colour += texture(originalTexture, blurTextureCoords[3]) * 0.121703;
    out_colour += texture(originalTexture, blurTextureCoords[4]) * 0.175713;
    out_colour += texture(originalTexture, blurTextureCoords[5]) * 0.198596;
    out_colour += texture(originalTexture, blurTextureCoords[6]) * 0.175713;
    out_colour += texture(originalTexture, blurTextureCoords[7]) * 0.121703;
    out_colour += texture(originalTexture, blurTextureCoords[8]) * 0.065984;
    out_colour += texture(originalTexture, blurTextureCoords[9]) * 0.028002;
    out_colour += texture(originalTexture, blurTextureCoords[10]) * 0.0093;
}";

        public VerticalBlurShader() : base(Vertex, Fragment)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
        }
        
        public void SetTargetHeight(float height)
        {
            SetUniform("targetHeight", height);
        }
    }
}