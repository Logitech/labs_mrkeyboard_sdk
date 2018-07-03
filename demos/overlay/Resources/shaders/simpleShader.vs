#version 410

layout(location = 0) in vec3 position;
in vec3 normal;
in vec2 texcoord;

out vec3 Normal;
out vec2 Texcoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	Normal = normal;
    Texcoord = texcoord;
    gl_Position = projection * view * model * vec4(position, 1.0);
}