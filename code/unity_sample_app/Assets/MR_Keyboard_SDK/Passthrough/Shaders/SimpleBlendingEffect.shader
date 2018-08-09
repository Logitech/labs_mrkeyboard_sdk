Shader "PassthroughEffect/SimpleBlendingEffect"
{
    Properties
    {
        _TintColor("Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _Brightness("Brightness", Float) = 1
        _FadeEdges("FadeEdges", Int) = 1
        _FadeEdgeHoriRatio("FadeEdgeHoriRatio", Float) = 0.05
        _FadeEdgeVertRatio("FadeEdgeVertRatio", Float) = 0.1
    }

    Category
    {
        Tags{ "Queue" = "Transparent+100" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Cull Off Lighting Off ZWrite Off

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                fixed4 _TintColor;
                float4 _MainTex_ST;
                float _Brightness;
                bool _FadeEdges;
                float _FadeEdgeHoriRatio;
                float _FadeEdgeVertRatio;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = _Brightness * tex2D(_MainTex, i.uv);
                    // apply fog
                    UNITY_APPLY_FOG(i.fogCoord, col);

                    // apply Tron mode color
                    col = col * _TintColor;

                    // fade the edges to blend with the background natrually
                    if (_FadeEdges)
                    {
                        float alphaFade = 1.0f;
                        float fadeSizeVertical = _FadeEdgeVertRatio;
                        float fadeSizeHorizontal = _FadeEdgeHoriRatio;

                        if (i.uv.x < fadeSizeHorizontal)
                        {
                            alphaFade = (1.0f / fadeSizeHorizontal) * i.uv.x;
                        }
                        else if (i.uv.x > 1.0f - fadeSizeHorizontal)
                        {
                            alphaFade = (-1.0f / fadeSizeHorizontal) * (i.uv.x - 1.0f);
                        }

                        if (i.uv.y < fadeSizeVertical)
                        {
                            alphaFade *= (1.0f / fadeSizeVertical) * i.uv.y;
                        }
                        else if (i.uv.y > 1.0f - fadeSizeVertical)
                        {
                            alphaFade *= (-1.0f / fadeSizeVertical) * (i.uv.y - 1.0f);
                        }

                        col.a *= alphaFade;
                    }

                    col = clamp(col, 0.0, 1.0);
                    return col;
                }
                ENDCG
            }
        }
    }
}
