Shader "Custom/NeonLightBorder"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _GlowColor("Glow Color", Color) = (1,0,0,1)
        _GlowIntensity("Glow Intensity", Float) = 1.0
        _EdgeWidth("Edge Width", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GlowColor;
            float _GlowIntensity;
            float _EdgeWidth;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                float alphaGlow = col.a;

                // Apply glow based on the edge detection
                float edge = fwidth(alphaGlow);
                float glow = smoothstep(_EdgeWidth, 0.0, edge * _GlowIntensity);

                // Mix glow color with texture color
                float4 glowColor = _GlowColor * glow;

                return float4(col.rgb + glowColor.rgb, col.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
