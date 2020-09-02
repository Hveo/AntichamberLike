Shader "Custom/Eyeshader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius("Radius", float) = 0.25
        _Angle("Angle", float) = 0.0
        _Center("Center", Vector) = (0.5, 0.5, 0.0, 0.0)
        _EyelidColor("Surface Color", Color) = (1, 0, 0, 1)

        _Squeeze("Squeeze", Range(0.0, 1.0)) = 0.3

        _IrisColor("Iris Color", Color) = (0, 0, 1, 1)
        _IrisRadius("Iris Radius", float) = 0.03

        _PupilOffset("Pupil Offset", Vector) = (0.0, 0.0, 0.0, 0.0)
        _PupilColor("Pupil Color", Color) = (1, 0, 0, 1)
        _PupilRadius("Pupil Radius", float) = 0.03

        _Color("Eye Area Color", Color) = (1, 0, 0, 1)
        _Blink("Blink", Range(0.0, 1.0)) = 0.0
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
            fixed4 fragColor;

            uniform float _Radius;
            uniform float _Squeeze;
            uniform float _Angle;
            uniform float2 _Center;
            uniform fixed4 _Color;

            uniform float _IrisRadius;
            uniform fixed4 _IrisColor;

            uniform float2 _PupilOffset;
            uniform fixed4 _PupilColor;
            uniform float _PupilRadius;

            uniform fixed4 _EyelidColor;
            uniform float _Blink;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 drawCircle(v2f v, float radius, float2 center, fixed4 color, float squeeze, float angle, bool invert)
            {
                float2 fromCenter = center - v.uv;
                float uvLength = length(fromCenter);
                float angleFromCenter = atan2(fromCenter.y, fromCenter.x) - angle;
                v.uv.x = cos(angleFromCenter) * uvLength;
                v.uv.y = sin(angleFromCenter) * uvLength;
                v.uv += center;

                float squeezeDir = (v.uv.x < center.x) ? -squeeze : squeeze;
                squeezeDir *= radius;

                float2 squeezeAngle = float2(cos(angle), sin(angle)) * squeezeDir;

                if (distance(v.uv + float2(squeezeDir, 0.0), center) <= radius)
                    return invert ? fragColor : color;

                return invert ? color : fragColor;
            }

            float4 drawEyelid(v2f v, float squeeze, float blink)
            {
                return drawCircle(v, _Radius, _Center, _EyelidColor, _Squeeze + blink, _Angle, true);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Drawing shapes
                fragColor = fixed4(1, 1, 1, 0);
                fragColor = drawCircle(i, _Radius, _Center, _Color, 0.0, 0.0, false);

                //Mixing the shapes
                fixed4 scleraColor = fixed4(1.0, 1.0, 1.0, 1.0);
                fixed4 sclera = drawCircle(i, _Radius - 0.01, _Center, scleraColor, _Squeeze, _Angle, false);
                
                fragColor = sclera;

                _PupilOffset += _Center;
                fixed4 iris = drawCircle(i, _PupilRadius + _IrisRadius, _PupilOffset, _IrisColor, _Squeeze, _Angle, false);
                fragColor = iris;

                fixed4 pupil = drawCircle(i, _PupilRadius, _PupilOffset, _PupilColor, _Squeeze, _Angle, false);
                fragColor = pupil;

				//_Blink = lerp(0.0, 1.0, 0.5 + sin(_Time.w * 1.75) * 0.5);
                fixed4 eyelid = drawEyelid(i, _Squeeze, _Blink);
                fragColor = eyelid;


                return fragColor;
            }
            ENDCG
        }
    }
}
