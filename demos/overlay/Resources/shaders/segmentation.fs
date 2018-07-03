#version 410

in vec3 Normal;
in vec2 Texcoord;
out vec4 outColor;
uniform sampler2D videoTexture;
uniform vec4 handsColor;
uniform vec2 rgSegmentationRange;
uniform vec2 luminanceSegmentationRange;
uniform int segmentationMode;

int rgSegmentation = 0;
int luminanceSegmentation = 1;

float grayValue(vec4 c){
	//return 0.21*c.r + 0.72*c.g + 0.07*c.b;
	return dot(c.rgb, vec3(0.299, 0.587, 0.114));
}


void main()
{		
	vec4 livePixel = texture(videoTexture,Texcoord);
	float gray = grayValue(livePixel);     		
	
	float fragmentIsSkin = 0.0;
	
	if (segmentationMode == rgSegmentation){
		float rgbDiff = livePixel.r - livePixel.g;
		fragmentIsSkin = max(sign(rgbDiff - rgSegmentationRange.x), 0.0) * max(sign(rgSegmentationRange.y - rgbDiff), 0.0);
	}
	if (segmentationMode == luminanceSegmentation){
		fragmentIsSkin = max(sign(livePixel.r - luminanceSegmentationRange.x), 0.0) * max(sign(luminanceSegmentationRange.y - livePixel.r), 0.0);
	}
	
	vec3 finalColor = handsColor.rgb*gray*1.5;
	outColor = vec4 (finalColor,fragmentIsSkin*handsColor.a);
}