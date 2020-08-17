// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/Internal-DeferredShading" {
    Properties{
        _LightTexture0("", any) = "" {}
        _LightTextureB0("", 2D) = "" {}
        _ShadowMapTexture("", any) = "" {}
        _SrcBlend("", Float) = 1
        _DstBlend("", Float) = 1
    }
        SubShader{

            // Pass 1: Lighting pass
            //  LDR case - Lighting encoded into a subtractive ARGB8 buffer
            //  HDR case - Lighting additively blended into floating point buffer
            Pass {
                ZWrite Off
                BlendOp RevSub
                Blend[_SrcBlend][_DstBlend]

            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert_deferred
            #pragma fragment frag
            #pragma multi_compile_lightpass
            #pragma multi_compile ___ UNITY_HDR_ON

            #pragma exclude_renderers nomrt

            #include "UnityCG.cginc"
            #include "UnityDeferredLibrary.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardUtils.cginc"
            #include "UnityGBuffer.cginc"
            #include "UnityStandardBRDF.cginc"

            sampler2D _CameraGBufferTexture0;
            sampler2D _CameraGBufferTexture1;
            sampler2D _CameraGBufferTexture2;

            float3 rgb2hsv(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float3 hsv2rgb(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
            }

            half4 CalculateLight(unity_v2f_deferred i)
            {
                float3 wpos;
                float2 uv;
                float atten, fadeDist;
                UnityLight light;
                UNITY_INITIALIZE_OUTPUT(UnityLight, light);
                UnityDeferredCalculateLightParams(i, wpos, uv, light.dir, atten, fadeDist);

				//_LightColor.rgb *= _LightColor.a > 0.5 ? 1.0 : -1.0;
                light.color = _LightColor.rgb * atten;
                half3 hsv = rgb2hsv(light.color);
                hsv.r = hsv.r - 0.5;
                light.color = hsv2rgb(hsv);

                // unpack Gbuffer
                half4 gbuffer0 = tex2D(_CameraGBufferTexture0, uv);
                half4 gbuffer1 = tex2D(_CameraGBufferTexture1, uv);
                half4 gbuffer2 = tex2D(_CameraGBufferTexture2, uv);
                UnityStandardData data = UnityStandardDataFromGbuffer(gbuffer0, gbuffer1, gbuffer2);

                float3 eyeVec = normalize(wpos - _WorldSpaceCameraPos);
                half oneMinusReflectivity = 1 - SpecularStrength(data.specularColor.rgb);

                UnityIndirect ind;
                UNITY_INITIALIZE_OUTPUT(UnityIndirect, ind);
                ind.diffuse = 0;
                ind.specular = 0;

                half4 res = UNITY_BRDF_PBS(data.diffuseColor, data.specularColor, oneMinusReflectivity, data.smoothness, data.normalWorld, -eyeVec, light, ind);
				res.rgb *= _LightColor.a > 0.5 ? 1.0 : -1.0;

                return res;
            }

            #ifdef UNITY_HDR_ON
            half4
            #else
            fixed4
            #endif
            frag(unity_v2f_deferred i) : SV_Target
            {
                half4 c = CalculateLight(i);
                #ifdef UNITY_HDR_ON
                return c;
                #else
                return exp2(-c);
                #endif
            }

            ENDCG
            }


            // Pass 2: Final decode pass.
            // Used only with HDR off, to decode the logarithmic buffer into the main RT
            Pass {
                ZTest Always Cull Off ZWrite Off
                Stencil {
                    ref[_StencilNonBackground]
                    readmask[_StencilNonBackground]
                // Normally just comp would be sufficient, but there's a bug and only front face stencil state is set (case 583207)
                compback equal
                compfront equal
            }

        CGPROGRAM
        #pragma target 3.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma exclude_renderers nomrt

        #include "UnityCG.cginc"

        sampler2D _LightBuffer;
        struct v2f {
            float4 vertex : SV_POSITION;
            float2 texcoord : TEXCOORD0;
        };

        v2f vert(float4 vertex : POSITION, float2 texcoord : TEXCOORD0)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(vertex);
            o.texcoord = texcoord.xy;
        #ifdef UNITY_SINGLE_PASS_STEREO
            o.texcoord = TransformStereoScreenSpaceTex(o.texcoord, 1.0f);
        #endif
            return o;
        }

        fixed4 frag(v2f i) : SV_Target
        {
            return -log2(tex2D(_LightBuffer, i.texcoord));
        }
        ENDCG
        }

        }
            Fallback Off
}
