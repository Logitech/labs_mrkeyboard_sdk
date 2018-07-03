#version 410

in vec3 Normal;
in vec2 Texcoord;

out vec4 outColor;
uniform sampler2D mainTexture;

void main()
{
    outColor = texture(mainTexture, Texcoord);
}
