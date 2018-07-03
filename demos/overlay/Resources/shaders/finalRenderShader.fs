#version 410

in vec3 Normal;
in vec2 Texcoord;

out vec4 outColor;
uniform float sceneAlpha;
uniform sampler2D sceneTexture;
uniform sampler2D handsTexture;
uniform float erodeThreshold;
uniform float kdelta;

float getAvgPixelValue(sampler2D txt){
	float avgValue = 0;

	avgValue += texture(txt, vec2(Texcoord.s-kdelta*2.0,Texcoord.t-kdelta*2.0)).r;
	avgValue += texture(txt, vec2(Texcoord.s-kdelta,Texcoord.t-kdelta*2.0)).r;
	avgValue += texture(txt, vec2(Texcoord.s,Texcoord.t-kdelta*2.0)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta,Texcoord.t-kdelta*2.0)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta*2.0,Texcoord.t-kdelta*2.0)).r;

	avgValue += texture(txt, vec2(Texcoord.s-kdelta*2.0,Texcoord.t-kdelta)).r;
	avgValue += texture(txt, vec2(Texcoord.s-kdelta,Texcoord.t-kdelta)).r;
	avgValue += texture(txt, vec2(Texcoord.s,Texcoord.t-kdelta)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta,Texcoord.t-kdelta)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta*2.0,Texcoord.t-kdelta)).r;

	avgValue += texture(txt, vec2(Texcoord.s-kdelta*2.0,Texcoord.t)).r;
	avgValue += texture(txt, vec2(Texcoord.s-kdelta,Texcoord.t)).r;
	avgValue += texture(txt, vec2(Texcoord.s,Texcoord.t)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta,Texcoord.t)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta*2.0,Texcoord.t)).r;

	avgValue += texture(txt, vec2(Texcoord.s-kdelta*2.0,Texcoord.t+kdelta)).r;
	avgValue += texture(txt, vec2(Texcoord.s-kdelta,Texcoord.t+kdelta)).r;
	avgValue += texture(txt, vec2(Texcoord.s,Texcoord.t+kdelta)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta,Texcoord.t+kdelta)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta*2.0,Texcoord.t+kdelta)).r;

	avgValue += texture(txt, vec2(Texcoord.s-kdelta*2.0,Texcoord.t+kdelta*2.0)).r;
	avgValue += texture(txt, vec2(Texcoord.s-kdelta,Texcoord.t+kdelta*2.0)).r;
	avgValue += texture(txt, vec2(Texcoord.s,Texcoord.t+kdelta*2.0)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta,Texcoord.t+kdelta*2.0)).r;
	avgValue += texture(txt, vec2(Texcoord.s+kdelta*2.0,Texcoord.t+kdelta*2.0)).r;

	return step(erodeThreshold, avgValue/25.0);
}

void main()
{
     vec4 scenePixel = texture(sceneTexture, Texcoord);
     vec4 handsPixel = texture(handsTexture, Texcoord);
     //handsPixel.a =handsPixel.a*getAvgPixelValue(handsTexture);
     outColor = vec4(mix(scenePixel.rgb,handsPixel.rgb, handsPixel.a),sceneAlpha*scenePixel.a);
}
