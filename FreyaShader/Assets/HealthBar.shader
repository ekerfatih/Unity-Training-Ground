Shader "Unlit/HealthBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FullHpColor ("Full Hp Color",Color) = (1,1,1,1)
        _DeadColor ("Dead Hp Color",Color) = (1,1,1,1)
        _Health("Health",Range(0,1)) = 1
        _Frequency("Frequency",Range(0,10)) = 1
        _Amplitude("Amplitude",Range(0,10)) = 1
        _BorderSize("Border Size",Range(0,.5))=0.1
    }
    SubShader
    {

        Blend SrcAlpha OneMinusSrcAlpha

        Tags
        {
            ///"RenderType"="Opaque"
            "RenderType"="Transparent"
            "Queue"="Transparent"
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

            sampler2D _MainTex;
            float4 _FullHpColor;
            float4 _DeadColor;
            float _Health;
            float _Frequency;
            float _Amplitude;
            float _BorderSize;
            
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
                // rounded edge part
                float2 coords = i.uv;
                coords.x *= 8;

                float2 pointOnLineSeg = float2(clamp(coords.x,0.5,7.5),.5);
                float sdf = distance(coords,pointOnLineSeg) *2 -1;
                clip(-sdf);

                float borderSdf = sdf + _BorderSize;
                float pd = fwidth(borderSdf);
                length(float2(ddx(borderSdf),ddy(borderSdf)));
                float borderMask =1-saturate(borderSdf/pd);
                //return float4(borderMask.xxx,1);
                
                // rounded egde end

                float healthBarMask = _Health > i.uv.x;
                float flash = abs(cos(_Time.y * _Frequency) * _Amplitude) + 1;
                float3 healthBarColor = tex2D(_MainTex, float2(_Health, i.uv.y));
                if (_Health < .2)
                {
                    healthBarColor *= flash;
                }

                return float4(healthBarColor * healthBarMask * borderMask, 1);




                // float4 ret = 
                // return ret;



                //  float tHealthColor = saturate(InverseLerp(0.2,0.8,_Health)); // bar color lerp time
                //  float4 rawOutColor = lerp(_DeadColor, _FullHpColor, tHealthColor); // Bar color lerp
                //  float4 black = float4(0, 0, 0, 0); // black color
                //
                //  float healbarMask = _Health > i.uv.x; // bools can writeable as float
                //  clip(healbarMask-.5);
                //  // _Health > i.uv.x ? outColor : float4(0,0,0,0) == step(i.uv.x, _Health);
                //
                // // float4 outColor = lerp(black,rawOutColor,healbarMask);
                //  return float4(rawOutColor.rgb,healbarMask );
                //  //return outColor;

                // float healthBarMask = _Health > i.uv.x;
                // float tHealthColor = saturate(InverseLerp(0.2,0.8,_Health));
                // float3 healthBarColor = lerp(float3(1,0,0),float3(0,1,0),tHealthColor);
                // return float4(healthBarColor,healthBarMask * 0.5);



            }
            ENDCG
        }
    }
}