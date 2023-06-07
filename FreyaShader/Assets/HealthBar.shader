Shader "Unlit/HealthBar"
{
    Properties
    {
        _FullHpColor ("Full Hp Color",Color) = (1,1,1,1)
        _DeadColor ("Dead Hp Color",Color) = (1,1,1,1)
        _Health("Health",Range(0,1)) = 1

    }
    SubShader
    {
        
        Blend SrcAlpha OneMinusSrcAlpha

        Tags
        {
            "RenderType"="Transparent"
        }
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

            float4 _FullHpColor;
            float4 _DeadColor;
            float _Health;

            float InverseLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.uv = v.uv;
                //o.vertex = UnityObjectToClipPos(float4((v.vertex.x*_Health)+( _Health/2)-.5,v.vertex.y,v.vertex.z,1));
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                float tHealthColor = saturate(InverseLerp(0.2,0.8,_Health));
                
                float health = _Health;
                if(_Health < 0.2) health = 0;
                if(_Health>0.8) health = 1;
                float4 outColor = lerp(_DeadColor, _FullHpColor, tHealthColor);
                // _Health > i.uv.x ? outColor : float4(0,0,0,0) == step(i.uv.x, _Health);
                float4 black = float4(0, 0, 0, 0);
                return _Health > i.uv.x ? outColor : black;
            }
            ENDCG
        }
    }
}