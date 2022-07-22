namespace Tucan3D_GameEngine
{
    public class ShaderData : Shader
    {
        private static string Vertex = @"
#version 150

in vec3 position;
in vec2 textureCoordinates;
in vec3 normal;
in vec3 tangent;
in vec3 bitangent;

out vec3 toCameraVector;
out vec3 toLightVector;
out vec3 pass_fragPosition;
out vec2 pass_textureCoordinates;
out vec3 pass_normal;
out vec3 surfaceNormal;

uniform mat4 lightMatrix;
uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform vec3 lightPos;

void main(void){
	gl_Position = projectionMatrix * viewMatrix * transformationMatrix * vec4(position,1.0);
	pass_textureCoordinates = textureCoordinates;
    vec4 positionRelativeToCam = lightMatrix * vec4(position,1.0);
    pass_fragPosition = position;
    pass_normal = normal;

    surfaceNormal = (transformationMatrix * vec4(normal,0.0)).xyz;

    vec3 norm = normalize(surfaceNormal);
	vec3 tang = normalize((transformationMatrix * vec4(tangent, 0.0)).xyz);
	vec3 bitang = normalize(cross(norm, tang));
	
	mat3 toTangentSpace = mat3(
		tang.x, bitang.x, norm.x,
		tang.y, bitang.y, norm.y,
		tang.z, bitang.z, norm.z
	);

    toLightVector = toTangentSpace * (lightPos - positionRelativeToCam.xyz);
    toCameraVector = toTangentSpace * (-positionRelativeToCam.xyz);
}";

        private static string Fragment = @"
#version 150

in vec2 pass_textureCoordinates;
in vec3 pass_fragPosition;
in vec3 toCameraVector;
in vec3 toLightVector;
in vec3 surfaceNormal;

out vec4 out_Color;

uniform vec3 cameraPos;

uniform mat3 normalMatrix;

uniform bool hasNormalMap;
uniform sampler2D modelTexture;
uniform sampler2D normalTexture;

const float shineDamper = 1;
const float reflectivity = 0.1;

void main(void){
    vec3 lightColour = vec3(0.8, 0.8, 0.7);

    vec4 normalMapValue = 2.0 * texture(normalTexture, pass_textureCoordinates, -1.0) - 1.0;

    vec3 unitNormal = normalize(surfaceNormal);

    if(hasNormalMap)
	   unitNormal = normalize(normalMapValue.rgb);

	vec3 unitLightVector = normalize(toLightVector);
	
	float nDotl = dot(unitNormal,unitLightVector);
	float brightness = max(nDotl,0.2);
	vec3 diffuse = brightness * lightColour;
	
	vec3 unitVectorToCamera = normalize(toCameraVector);
	vec3 lightDirection = -unitLightVector;
	vec3 reflectedLightDirection = reflect(lightDirection,unitNormal);
	
	float specularFactor = dot(reflectedLightDirection, unitVectorToCamera);
	specularFactor = max(specularFactor, 0.0);
	float dampedFactor = pow(specularFactor, shineDamper);
	vec3 finalSpecular = dampedFactor * reflectivity * lightColour;
    
    out_Color = vec4(diffuse, 1) * texture(modelTexture, pass_textureCoordinates) + vec4(finalSpecular, 1);
}";

        public ShaderData() : base(Vertex, Fragment)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCoordinates");
            BindAttribute(2, "normal");
            BindAttribute(3, "tangent");
            BindAttribute(4, "bitangent");
        }
    }
}