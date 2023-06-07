Shader "Unlit/Shader2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST; // optional magic variable name texture tiling

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld,v.vertex);  // also can be used model matrix UNITY_MATRIX_M  // if 4th value is 0 its pass as direction but if its 1 it pass as position
                o.vertex = UnityObjectToClipPos(v.vertex); 
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.x += _Time.y *0.1;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 topDownProjection = i.worldPos.xz;

                //return float4(topDownProjection,0,1);
                
                fixed4 col = tex2D(_MainTex, topDownProjection);
                return col;
            }
            ENDCG
        }
    }
}
