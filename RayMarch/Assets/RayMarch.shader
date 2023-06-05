Shader "Unlit/RayMarch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define MAX_STEPS 100
            #define MAX_DIST 100
            #define SURF_DIST .001


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
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float GetDist(float3 p)
            {
                float d = length(p) - .5;
                return d;
            }

            float Raymarch(float rayOrigin, float3 rayDirection)
            {
                float dO = 0;
                float distanceSurface;
                for (int i = 0; i < MAX_STEPS; i++)
                {
                    float3 p = rayOrigin + dO * rayDirection;
                    distanceSurface = GetDist(p);
                    dO += distanceSurface;
                    if (distanceSurface < SURF_DIST || dO > MAX_DIST) break;
                }
                return dO;
            }

            float3 GetNormal(float3 p)
            {
                float2 e = float2(1e-2, 0);
                float3 n = GetDist(p) - float3(
                    GetDist(p - e.xyy),
                    GetDist(p - e.yxy),
                    GetDist(p - e.yyx)
                );
                return normalize(n);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv - .5;
                float3 ro = float3(0, 0, -3);
                float3 rd = normalize(float3(uv.x, uv.y, 1));


                float d = Raymarch(ro, rd);
                fixed4 col = 0;

                if (d < MAX_DIST)
                {
                    float p = ro + rd * d;
                    float3 n = GetNormal(p);
                    col.rgb = n;
                }
                return col;
            }
            ENDCG
        }
    }
}