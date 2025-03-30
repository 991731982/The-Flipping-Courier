Shader "Custom/EdgeGlowLight"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 5
        _EdgeWidth ("Edge Width", Range(0.001, 0.05)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _EdgeColor;
            float _GlowIntensity;
            float _EdgeWidth;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Calculate edge detection
                float edge = abs(tex2D(_MainTex, i.uv + float2(_EdgeWidth, 0)).r - tex2D(_MainTex, i.uv - float2(_EdgeWidth, 0)).r) +
                             abs(tex2D(_MainTex, i.uv + float2(0, _EdgeWidth)).r - tex2D(_MainTex, i.uv - float2(0, _EdgeWidth)).r);

                // Apply glow intensity to the edge
                float glow = saturate(edge * _GlowIntensity);

                // Combine with edge color
                return lerp(texColor, _EdgeColor, glow);
            }
            ENDCG
        }
    }
}
