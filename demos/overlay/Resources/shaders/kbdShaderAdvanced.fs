#version 410
in vec3 Normal;
in vec3 FragPos;
in vec2 Texcoord;
out vec4 outColor;

uniform float ambientStrength;
uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 lightColor;
uniform vec4 printColor;
uniform vec4 keyColor;

uniform sampler2D t_albedo;
uniform sampler2D t_prints;

void main()
{
	// Albedo
	vec4 objectColor = texture(t_albedo, Texcoord);
	objectColor = vec4(keyColor.rgb * keyColor.a + objectColor.rgb * (1.0 - keyColor.a), objectColor.a);

	vec3 lightDir = normalize(lightPos - FragPos);
	
	// Ambient	
	vec3 ambient = ambientStrength * lightColor;

	// Diffuse
	float diff = max(dot(Normal, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

	// Prints
	vec4 print = texture(t_prints, Texcoord);
	vec3 actualPrintColor = printColor.rgb * printColor.a + print.rgb * (1.0 - printColor.a);

	// Final render
	vec4 kbdColor = vec4((ambient + diffuse), 1.0) * objectColor;
	outColor = vec4(mix(kbdColor.rgb, actualPrintColor.rgb, print.a), kbdColor.a);
}
