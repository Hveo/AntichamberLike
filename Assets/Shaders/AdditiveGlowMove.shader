// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9208,x:33201,y:32517,varname:node_9208,prsc:2|emission-9522-OUT,alpha-6824-OUT;n:type:ShaderForge.SFN_Tex2d,id:8009,x:31854,y:32640,ptovrint:False,ptlb:Main Texture,ptin:_MainTexture,varname:node_8009,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-4889-OUT;n:type:ShaderForge.SFN_VertexColor,id:6402,x:31864,y:32856,varname:node_6402,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4550,x:32306,y:32698,varname:node_4550,prsc:2|A-8010-OUT,B-6402-RGB;n:type:ShaderForge.SFN_TexCoord,id:7577,x:30803,y:32569,varname:node_7577,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:9976,x:30982,y:32928,varname:node_9976,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:1352,x:30803,y:32778,ptovrint:False,ptlb:U Speed,ptin:_USpeed,varname:node_1352,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:1482,x:30803,y:32872,ptovrint:False,ptlb:V Speed,ptin:_VSpeed,varname:_node_8786_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Append,id:3367,x:30982,y:32781,varname:node_3367,prsc:2|A-1352-OUT,B-1482-OUT;n:type:ShaderForge.SFN_Multiply,id:3478,x:31178,y:32791,varname:node_3478,prsc:2|A-3367-OUT,B-9976-T;n:type:ShaderForge.SFN_Add,id:4889,x:31432,y:32656,varname:node_4889,prsc:2|A-7577-UVOUT,B-3478-OUT;n:type:ShaderForge.SFN_Multiply,id:5903,x:32815,y:32550,varname:node_5903,prsc:2|A-408-OUT,B-6855-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5047,x:31603,y:32421,ptovrint:False,ptlb:Glow Amount,ptin:_GlowAmount,varname:node_5047,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Color,id:7686,x:31603,y:32250,ptovrint:False,ptlb:Color Overlay,ptin:_ColorOverlay,varname:node_7686,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:408,x:31792,y:32332,varname:node_408,prsc:2|A-7686-RGB,B-5047-OUT;n:type:ShaderForge.SFN_Multiply,id:6824,x:32306,y:32892,varname:node_6824,prsc:2|A-8009-R,B-6402-A;n:type:ShaderForge.SFN_ValueProperty,id:9500,x:31910,y:32524,ptovrint:False,ptlb:Texture Contrast,ptin:_TextureContrast,varname:node_9500,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:8010,x:32129,y:32555,varname:node_8010,prsc:2|VAL-8009-RGB,EXP-9500-OUT;n:type:ShaderForge.SFN_Tex2d,id:1411,x:31812,y:33049,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:_MainTexture_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-4524-OUT;n:type:ShaderForge.SFN_Multiply,id:5566,x:32295,y:33096,varname:node_5566,prsc:2|A-8858-OUT,B-9706-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9706,x:32017,y:33301,ptovrint:False,ptlb:Noise Power,ptin:_NoisePower,varname:node_9706,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:6855,x:32631,y:32728,varname:node_6855,prsc:2|A-4550-OUT,B-5566-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6986,x:31812,y:33264,ptovrint:False,ptlb:Noise Contrast,ptin:_NoiseContrast,varname:node_6986,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:8858,x:32056,y:33116,varname:node_8858,prsc:2|VAL-1411-RGB,EXP-6986-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6497,x:30924,y:33211,ptovrint:False,ptlb:U Speed Noise,ptin:_USpeedNoise,varname:_USpeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:1890,x:30924,y:33305,ptovrint:False,ptlb:V Speed_Noise,ptin:_VSpeed_Noise,varname:_VSpeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Append,id:1808,x:31103,y:33214,varname:node_1808,prsc:2|A-6497-OUT,B-1890-OUT;n:type:ShaderForge.SFN_Multiply,id:4973,x:31390,y:33140,varname:node_4973,prsc:2|A-1808-OUT,B-9976-T;n:type:ShaderForge.SFN_Add,id:4524,x:31603,y:33083,varname:node_4524,prsc:2|A-7577-UVOUT,B-4973-OUT;n:type:ShaderForge.SFN_Multiply,id:9522,x:33022,y:32634,varname:node_9522,prsc:2|A-5903-OUT,B-6402-A;proporder:7686-5047-8009-1352-1482-9500-1411-9706-6986-6497-1890;pass:END;sub:END;*/

Shader "PM_Assets/AdditiveGlowMove" {
    Properties {
        _ColorOverlay ("Color Overlay", Color) = (1,1,1,1)
        _GlowAmount ("Glow Amount", Float ) = 1
        _MainTexture ("Main Texture", 2D) = "white" {}
        _USpeed ("U Speed", Float ) = 0
        _VSpeed ("V Speed", Float ) = 0
        _TextureContrast ("Texture Contrast", Float ) = 1
        _Noise ("Noise", 2D) = "white" {}
        _NoisePower ("Noise Power", Float ) = 1
        _NoiseContrast ("Noise Contrast", Float ) = 1
        _USpeedNoise ("U Speed Noise", Float ) = 0
        _VSpeed_Noise ("V Speed_Noise", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
			Blend One DstColor
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 d3d12 vulkan glcore gles switch ps4 xboxone
            #pragma target 3.0
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float _USpeed;
            uniform float _VSpeed;
            uniform float _GlowAmount;
            uniform float4 _ColorOverlay;
            uniform float _TextureContrast;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _NoisePower;
            uniform float _NoiseContrast;
            uniform float _USpeedNoise;
            uniform float _VSpeed_Noise;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_9976 = _Time;
                float2 node_4889 = (i.uv0+(float2(_USpeed,_VSpeed)*node_9976.g));
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(node_4889, _MainTexture));
                float2 node_4524 = (i.uv0+(float2(_USpeedNoise,_VSpeed_Noise)*node_9976.g));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_4524, _Noise));
                float3 emissive = (((_ColorOverlay.rgb*_GlowAmount)*((pow(_MainTexture_var.rgb,_TextureContrast)*i.vertexColor.rgb)*(pow(_Noise_var.rgb,_NoiseContrast)*_NoisePower)))*i.vertexColor.a);
                float3 finalColor = emissive;
                return fixed4(finalColor,(_MainTexture_var.r*i.vertexColor.a));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 d3d12 vulkan glcore gles 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
