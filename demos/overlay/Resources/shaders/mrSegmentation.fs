#version 410

in vec3 Normal;
in vec2 Texcoord;

out vec4 outColor;
uniform sampler2D mainTexture;
uniform vec2 mrSegmentationRange;
uniform float useMrDefaultSegmentation;
uniform float mrSegmentationAlphaBoost;
void main()
{
    outColor = texture(mainTexture, Texcoord);
    
    if (useMrDefaultSegmentation > 0){
    	outColor.a = outColor.a * mrSegmentationAlphaBoost;
    }else{
    	outColor.a = max(sign(outColor.r - mrSegmentationRange.x), 0.0) * max(sign(mrSegmentationRange.y+0.01 - outColor.r), 0.0);	
    }
    
}
