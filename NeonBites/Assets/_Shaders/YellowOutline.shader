Shader "Custom/YellowOutline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,0.8,1) // Lighter yellow
        _OutlineColor ("Outline Color", Color) = (0.5,0.5,0,1) // Darker color for the outline
        _Outline ("Outline width", Range (0.0, 0.1)) = 0.02
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG

        // Outline Pass
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            ZWrite Off
            ColorMask RGB
            Cull Front
            Offset 1, 1

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            fixed4 _OutlineColor;  // Ensure the variable is declared
            float _Outline;

            v2f vert (appdata v)
            {
                v2f o;
                // Expanding the vertex position along the vertex normal
                float3 offset = v.normal * _Outline;
                v.vertex.xyz += offset;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;  // Use the outline color
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
