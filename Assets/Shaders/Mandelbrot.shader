Shader "Unlit/Mandelbrot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Range(0, 1)) = 0.0
        _Repeat("Repeat", float) = 1.0
        _Speed("Speed", float) = 1.0
        _Area("Area", vector) = (0, 0, 4, 4)
        _MaxIteration("MaxIteration", float) = 0.0
        _Angle("Angle", range(-3.1415, 3.1415)) = 0
        _Symmetry("Symmetry", range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            float4 _MainTex_ST;
            float4 _Area;
            float _MaxIteration;
            float _Angle;
            float _Color;
            float _Repeat;
            float _Speed;
            float _Symmetry;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float2 rot(float2 p, float2 pivot, float a)
            {
                float s = sin(a);
                float c = cos(a);
                
                p -= pivot;
                p = float2(p.x * c - p.y * s, p.x * s + p.y * c);
                p += pivot;

                return p;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv - 0.5;
                uv = abs(uv);
                uv = rot(uv, 0, 0.25 * 3.1415);
                uv = abs(uv);

                uv = lerp(i.uv - 0.5, uv, _Symmetry);
                
                float2 col = _Area.xy + uv * _Area.zw;
                col = rot(col, _Area.xy, _Angle);
                
                float r = 20; //Escape Radius;
                float r2 = r * r;
                
                float2 z, zPrev;
                float it;

                for (it = 0; it < _MaxIteration; ++it)
                {
                    zPrev = rot(z, 0, _Time.y);
                    z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + col;
                    
                    if (dot(z, zPrev) > r2)
                        break;
                }

                if (it > _MaxIteration)
                    return 0;

                float dist = length(z);
                float fracIter = (dist - r) / (r2 - r);
                fracIter = log2(log(dist) / log(r)) - 1;
                //it -= fracIter;

                float m = sqrt(it / _MaxIteration);
                float4 c = sin(float4(0.3, 0.45, 0.65, 1) * m * 20) * 0.5 + 0.5;
                c = tex2D(_MainTex, float2(m * _Repeat + _Time.y * _Speed, _Color));
                float angle = atan2(z.x, z.y);

                c *= smoothstep(3, 0, fracIter);
                c *= 1 + sin(angle * 2 + _Time.y * 4) * 0.2;
                return c;
            }
            ENDCG
        }
    }
}
