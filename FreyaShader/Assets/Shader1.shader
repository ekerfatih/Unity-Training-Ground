Shader "Unlit/Shader1"
{
    Properties
    {
        _ColorA ("ColorA",Color) = (1,1,1,1)
        _ColorB ("ColorB",Color) = (1,1,1,1)
        _ColorStart ("Color Start",Range(0,1)) = 1
        _ColorEnd ("Color End",Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4  _ColorA;
            float4  _ColorB;
            float _ColorStart;
            float _ColorEnd;

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float2 uv : TEXCOORD0; // Texcord refers uv cords
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1; // just index
            };

            Interpolators vert (MeshData v)
            {
                 Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv;//(v.uv + _Offset) * _Scale;
                return o;
            }

            float InverseLerp(float a,float b,float v)
            {
                return (v-a)/(b-a);
            }
            
            fixed4 frag (Interpolators i) : SV_Target
            {
                float t = saturate(InverseLerp(_ColorStart,_ColorEnd,i.uv.x)); // saturate basicly clamp01
                //t = frac(t); limit but causes repeats or outside range of 0 1 
                
                float4 outColor = lerp(_ColorA,_ColorB,t);
                return outColor;
                // frac = v - floor(v)
                
                // float4 outColor = lerp(_ColorA,_ColorB,i.uv.x);
                // return outColor;
            }
            ENDCG
        }
    }
}
