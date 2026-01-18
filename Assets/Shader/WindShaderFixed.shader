Shader "Gorilla/SkyJungle22/Gorilla_SkyJungle_WindTunnelRibbons"
{
  Properties
  {
    [HideInInspector] __dirty ("", float) = 1
  }
  SubShader
  {
    Tags
    { 
      "IsEmissive" = "true"
      "QUEUE" = "Geometry+0"
      "RenderType" = "Opaque"
    }
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IsEmissive" = "true"
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Geometry+0"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _Time;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float3 texcoord :TEXCOORD0;
          float texcoord2 :TEXCOORD2;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord3 :TEXCOORD3;
          float4 texcoord5 :TEXCOORD5;
          float4 texcoord6 :TEXCOORD6;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 texcoord :TEXCOORD0;
          float texcoord2 :TEXCOORD2;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord3 :TEXCOORD3;
          float4 texcoord5 :TEXCOORD5;
          float4 texcoord6 :TEXCOORD6;
          float4 vertex :Position;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float u_xlat2;
      float u_xlat3;
      float3 u_xlat4;
      float3 u_xlat5;
      int u_xlatb5;
      float u_xlat15;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.x = (in_v.texcoord.x + _Time.y);
          u_xlat0.x = (u_xlat0.x * 0.5);
          #ifdef UNITY_ADRENO_ES3
          u_xlatb5 = (u_xlat0.x>=(-u_xlat0.x));
          #else
          u_xlatb5 = (u_xlat0.x>=(-u_xlat0.x));
          #endif
          u_xlat0.x = frac(abs(u_xlat0.x));
          u_xlat0.x = (u_xlatb5)?(u_xlat0.x):((-u_xlat0.x));
          u_xlat0.x = ((u_xlat0.x * 2) + (-0.300000012));
          u_xlat0.x = (u_xlat0.x * 10);
          u_xlat0.x = max(u_xlat0.x, 0.699999988);
          u_xlat0.x = min(u_xlat0.x, 1);
          u_xlat5.xyz = ((-in_v.vertex.xyz) + in_v.color.xyz);
          u_xlat0.xyz = ((u_xlat0.xxx * u_xlat5.xyz) + in_v.vertex.xyz);
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat2 = sin((-_Time.y));
          u_xlat3 = cos((-_Time.y));
          u_xlat4.z = u_xlat2;
          u_xlat4.y = u_xlat3;
          u_xlat4.x = (-u_xlat2);
          u_xlat5.x = dot(u_xlat0.xz, u_xlat4.xy);
          u_xlat0.x = dot(u_xlat0.xz, u_xlat4.yz);
          u_xlat1 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * u_xlat5.xxxx) + u_xlat1);
          u_xlat0 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.vertex = mul(unity_MatrixVP, u_xlat0);
          out_v.texcoord1.xyz = u_xlat0.xyz;
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord.xyz = normalize(u_xlat0.xyz);
          out_v.texcoord2 = 0;
          out_v.texcoord3.xyz = float3(0, 0, 0);
          out_v.texcoord5 = float4(0, 0, 0, 0);
          out_v.texcoord6 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          out_f.color = float4(0.293330193, 0.460108697, 0.623193681, 1);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IsEmissive" = "true"
        "LIGHTMODE" = "FORWARDADD"
        "QUEUE" = "Geometry+0"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      ZWrite Off
      Cull Off
      Blend One One
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _Time;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4x4 unity_WorldToLight;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float3 texcoord :TEXCOORD0;
          float texcoord2 :TEXCOORD2;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 texcoord :TEXCOORD0;
          float texcoord2 :TEXCOORD2;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 vertex :Position;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float u_xlat2;
      float u_xlat3;
      float3 u_xlat4;
      float3 u_xlat5;
      int u_xlatb5;
      float u_xlat16;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.x = (in_v.texcoord.x + _Time.y);
          u_xlat0.x = (u_xlat0.x * 0.5);
          #ifdef UNITY_ADRENO_ES3
          u_xlatb5 = (u_xlat0.x>=(-u_xlat0.x));
          #else
          u_xlatb5 = (u_xlat0.x>=(-u_xlat0.x));
          #endif
          u_xlat0.x = frac(abs(u_xlat0.x));
          u_xlat0.x = (u_xlatb5)?(u_xlat0.x):((-u_xlat0.x));
          u_xlat0.x = ((u_xlat0.x * 2) + (-0.300000012));
          u_xlat0.x = (u_xlat0.x * 10);
          u_xlat0.x = max(u_xlat0.x, 0.699999988);
          u_xlat0.x = min(u_xlat0.x, 1);
          u_xlat5.xyz = ((-in_v.vertex.xyz) + in_v.color.xyz);
          u_xlat0.xyz = ((u_xlat0.xxx * u_xlat5.xyz) + in_v.vertex.xyz);
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat2 = sin((-_Time.y));
          u_xlat3 = cos((-_Time.y));
          u_xlat4.z = u_xlat2;
          u_xlat4.y = u_xlat3;
          u_xlat4.x = (-u_xlat2);
          u_xlat5.x = dot(u_xlat0.xz, u_xlat4.xy);
          u_xlat0.x = dot(u_xlat0.xz, u_xlat4.yz);
          u_xlat1 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * u_xlat5.xxxx) + u_xlat1);
          u_xlat0 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.vertex = mul(unity_MatrixVP, u_xlat0);
          u_xlat1.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat1.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat1.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord.xyz = normalize(u_xlat1.xyz);
          out_v.texcoord1.xyz = u_xlat0.xyz;
          out_v.texcoord2 = 0;
          u_xlat1.xyz = (u_xlat0.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz);
          u_xlat1.xyz = ((conv_mxt4x4_0(unity_WorldToLight).xyz * u_xlat0.xxx) + u_xlat1.xyz);
          u_xlat0.xyz = ((conv_mxt4x4_2(unity_WorldToLight).xyz * u_xlat0.zzz) + u_xlat1.xyz);
          out_v.texcoord3.xyz = ((conv_mxt4x4_3(unity_WorldToLight).xyz * u_xlat0.www) + u_xlat0.xyz);
          out_v.texcoord4 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          out_f.color = float4(0, 0, 0, 1);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: ShadowCaster
    {
      Name "ShadowCaster"
      Tags
      { 
        "IsEmissive" = "true"
        "LIGHTMODE" = "SHADOWCASTER"
        "QUEUE" = "Geometry+0"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile SHADOWS_DEPTH
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _Time;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4 unity_LightShadowBias;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float texcoord1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float texcoord1 :TEXCOORD1;
          float4 vertex :Position;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float3 u_xlat2;
      float u_xlat3;
      float3 u_xlat4;
      float3 u_xlat5;
      int u_xlatb5;
      float u_xlat10;
      float u_xlat16;
      int u_xlatb16;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.x = (in_v.texcoord.x + _Time.y);
          u_xlat0.x = (u_xlat0.x * 0.5);
          #ifdef UNITY_ADRENO_ES3
          u_xlatb5 = (u_xlat0.x>=(-u_xlat0.x));
          #else
          u_xlatb5 = (u_xlat0.x>=(-u_xlat0.x));
          #endif
          u_xlat0.x = frac(abs(u_xlat0.x));
          u_xlat0.x = (u_xlatb5)?(u_xlat0.x):((-u_xlat0.x));
          u_xlat0.x = ((u_xlat0.x * 2) + (-0.300000012));
          u_xlat0.x = (u_xlat0.x * 10);
          u_xlat0.x = max(u_xlat0.x, 0.699999988);
          u_xlat0.x = min(u_xlat0.x, 1);
          u_xlat5.xyz = ((-in_v.vertex.xyz) + in_v.color.xyz);
          u_xlat0.xyz = ((u_xlat0.xxx * u_xlat5.xyz) + in_v.vertex.xyz);
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat2.x = sin((-_Time.y));
          u_xlat3 = cos((-_Time.y));
          u_xlat4.z = u_xlat2.x;
          u_xlat4.y = u_xlat3;
          u_xlat4.x = (-u_xlat2.x);
          u_xlat5.x = dot(u_xlat0.xz, u_xlat4.xy);
          u_xlat0.x = dot(u_xlat0.xz, u_xlat4.yz);
          u_xlat1 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * u_xlat5.xxxx) + u_xlat1);
          u_xlat0 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat1.xyz = (((-u_xlat0.xyz) * _WorldSpaceLightPos0.www) + _WorldSpaceLightPos0.xyz);
          u_xlat1.xyz = normalize(u_xlat1.xyz);
          u_xlat2.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat2.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat2.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat2.xyz = normalize(u_xlat2.xyz);
          u_xlat1.x = dot(u_xlat2.xyz, u_xlat1.xyz);
          u_xlat1.x = (((-u_xlat1.x) * u_xlat1.x) + 1);
          u_xlat1.x = sqrt(u_xlat1.x);
          u_xlat1.x = (u_xlat1.x * unity_LightShadowBias.z);
          u_xlat1.xyz = (((-u_xlat2.xyz) * u_xlat1.xxx) + u_xlat0.xyz);
          #ifdef UNITY_ADRENO_ES3
          u_xlatb16 = (unity_LightShadowBias.z!=0);
          #else
          u_xlatb16 = (unity_LightShadowBias.z!=0);
          #endif
          u_xlat0.xyz = (int(u_xlatb16))?(u_xlat1.xyz):(u_xlat0.xyz);
          u_xlat0 = mul(unity_MatrixVP, u_xlat0);
          u_xlat1.x = (unity_LightShadowBias.x / u_xlat0.w);
          #ifdef UNITY_ADRENO_ES3
          u_xlat1.x = min(max(u_xlat1.x, 0), 1);
          #else
          u_xlat1.x = clamp(u_xlat1.x, 0, 1);
          #endif
          u_xlat10 = (u_xlat0.z + u_xlat1.x);
          u_xlat1.x = max((-u_xlat0.w), u_xlat10);
          out_v.vertex.xyw = u_xlat0.xyw;
          u_xlat0.x = ((-u_xlat10) + u_xlat1.x);
          out_v.vertex.z = ((unity_LightShadowBias.y * u_xlat0.x) + u_xlat10);
          out_v.texcoord1 = 0;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          out_f.color = float4(0, 0, 0, 0);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
