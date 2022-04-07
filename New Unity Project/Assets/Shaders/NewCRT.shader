Shader "NewCRT"
{
    Properties
    {
        Vector1_7CF07853("Curve", Float) = 1
        [NoScaleOffset]_MainTex("Sprite", 2D) = "white" {}
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "Transparent"
        }
        Stencil
        {
             Ref 1
             Comp Equal
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }

        // Render State
        Cull Off
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma exclude_renderers d3d11_9x
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEUNLIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv0 : TEXCOORD0;
        float4 color : COLOR;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS;
        float4 texCoord0;
        float4 color;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 WorldSpacePosition;
        float4 ScreenPosition;
        float4 uv0;
        float4 VertexColor;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        float4 interp2 : TEXCOORD2;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        output.interp1.xyzw = input.texCoord0;
        output.interp2.xyzw = input.color;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        output.texCoord0 = input.interp1.xyzw;
        output.color = input.interp2.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float Vector1_7CF07853;
float4 _MainTex_TexelSize;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Cosine_float(float In, out float Out)
{
    Out = cos(In);
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
}

void Unity_InvertColors_float(float In, float InvertColors, out float Out)
{
    Out = abs(InvertColors - In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    float4 _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
    float _Split_b11c60663a22868d84a350e78580dca2_R_1 = _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0[0];
    float _Split_b11c60663a22868d84a350e78580dca2_G_2 = _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0[1];
    float _Split_b11c60663a22868d84a350e78580dca2_B_3 = _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0[2];
    float _Split_b11c60663a22868d84a350e78580dca2_A_4 = _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0[3];
    float _Multiply_2893f53b2903fd8e87465c676555b30c_Out_2;
    Unity_Multiply_float(_Split_b11c60663a22868d84a350e78580dca2_G_2, _ScreenParams.y, _Multiply_2893f53b2903fd8e87465c676555b30c_Out_2);
    float _Cosine_c95396e519d0358ba925dd50a035f752_Out_1;
    Unity_Cosine_float(_Multiply_2893f53b2903fd8e87465c676555b30c_Out_2, _Cosine_c95396e519d0358ba925dd50a035f752_Out_1);
    float _Add_d07cd68b65c9fb899e77e39bf87b9f9e_Out_2;
    Unity_Add_float(_Cosine_c95396e519d0358ba925dd50a035f752_Out_1, 1, _Add_d07cd68b65c9fb899e77e39bf87b9f9e_Out_2);
    float _Add_aef09e52f73f9988b4963a7d23b67c8b_Out_2;
    Unity_Add_float(0.1, 1, _Add_aef09e52f73f9988b4963a7d23b67c8b_Out_2);
    float _Multiply_4249b5fcc90ced8e9c088c7895b89151_Out_2;
    Unity_Multiply_float(_Add_d07cd68b65c9fb899e77e39bf87b9f9e_Out_2, _Add_aef09e52f73f9988b4963a7d23b67c8b_Out_2, _Multiply_4249b5fcc90ced8e9c088c7895b89151_Out_2);
    UnityTexture2D _Property_3497ccb92c466a8494f763a5eeef400f_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_3497ccb92c466a8494f763a5eeef400f_Out_0.tex, _Property_3497ccb92c466a8494f763a5eeef400f_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_R_4 = _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0.r;
    float _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_G_5 = _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0.g;
    float _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_B_6 = _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0.b;
    float _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_A_7 = _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0.a;
    float4 _Combine_685109d740f1a682836aec119a164b99_RGBA_4;
    float3 _Combine_685109d740f1a682836aec119a164b99_RGB_5;
    float2 _Combine_685109d740f1a682836aec119a164b99_RG_6;
    Unity_Combine_float(_SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_R_4, 0, _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_B_6, 0, _Combine_685109d740f1a682836aec119a164b99_RGBA_4, _Combine_685109d740f1a682836aec119a164b99_RGB_5, _Combine_685109d740f1a682836aec119a164b99_RG_6);
    float4 _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2;
    Unity_Multiply_float((_Multiply_4249b5fcc90ced8e9c088c7895b89151_Out_2.xxxx), _Combine_685109d740f1a682836aec119a164b99_RGBA_4, _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2);
    float _Split_49bff29907c61482a948fdece0c79eba_R_1 = _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2[0];
    float _Split_49bff29907c61482a948fdece0c79eba_G_2 = _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2[1];
    float _Split_49bff29907c61482a948fdece0c79eba_B_3 = _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2[2];
    float _Split_49bff29907c61482a948fdece0c79eba_A_4 = _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2[3];
    float _Sine_85dfdc8a05675d888997e7439485a746_Out_1;
    Unity_Sine_float(_Multiply_2893f53b2903fd8e87465c676555b30c_Out_2, _Sine_85dfdc8a05675d888997e7439485a746_Out_1);
    float _Add_844f05ad17c7c5819e9d3f1d3e3195c0_Out_2;
    Unity_Add_float(_Sine_85dfdc8a05675d888997e7439485a746_Out_1, 1, _Add_844f05ad17c7c5819e9d3f1d3e3195c0_Out_2);
    float _Add_d452580216ccd98d8d8b3e22d5ce555f_Out_2;
    Unity_Add_float(0.15, 1, _Add_d452580216ccd98d8d8b3e22d5ce555f_Out_2);
    float _Multiply_375bb0d45f564d8fbd9b7e6940e814f6_Out_2;
    Unity_Multiply_float(_Add_844f05ad17c7c5819e9d3f1d3e3195c0_Out_2, _Add_d452580216ccd98d8d8b3e22d5ce555f_Out_2, _Multiply_375bb0d45f564d8fbd9b7e6940e814f6_Out_2);
    float _Multiply_6580116c0efe398d80a161dbb6171039_Out_2;
    Unity_Multiply_float(_Multiply_375bb0d45f564d8fbd9b7e6940e814f6_Out_2, _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_G_5, _Multiply_6580116c0efe398d80a161dbb6171039_Out_2);
    float4 _Combine_74d9a77fb7a64983a319833918e208f8_RGBA_4;
    float3 _Combine_74d9a77fb7a64983a319833918e208f8_RGB_5;
    float2 _Combine_74d9a77fb7a64983a319833918e208f8_RG_6;
    Unity_Combine_float(_Split_49bff29907c61482a948fdece0c79eba_R_1, _Multiply_6580116c0efe398d80a161dbb6171039_Out_2, _Split_49bff29907c61482a948fdece0c79eba_B_3, 0, _Combine_74d9a77fb7a64983a319833918e208f8_RGBA_4, _Combine_74d9a77fb7a64983a319833918e208f8_RGB_5, _Combine_74d9a77fb7a64983a319833918e208f8_RG_6);
    float4 _Multiply_dcf781d0c5f3fa84a4f2f60065bbe7b2_Out_2;
    Unity_Multiply_float(_Combine_74d9a77fb7a64983a319833918e208f8_RGBA_4, IN.VertexColor, _Multiply_dcf781d0c5f3fa84a4f2f60065bbe7b2_Out_2);
    float _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_Out_1;
    float _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_InvertColors = float(0
);    Unity_InvertColors_float(_SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_A_7, _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_InvertColors, _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_Out_1);
    surface.BaseColor = (_Multiply_dcf781d0c5f3fa84a4f2f60065bbe7b2_Out_2.xyz);
    surface.Alpha = _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_Out_1;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.WorldSpacePosition = input.positionWS;
    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
    output.uv0 = input.texCoord0;
    output.VertexColor = input.color;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "Sprite Unlit"
    Tags
    {
        "LightMode" = "UniversalForward"
    }

        // Render State
        Cull Off
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma exclude_renderers d3d11_9x
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEFORWARD
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv0 : TEXCOORD0;
        float4 color : COLOR;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS;
        float4 texCoord0;
        float4 color;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 WorldSpacePosition;
        float4 ScreenPosition;
        float4 uv0;
        float4 VertexColor;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        float4 interp2 : TEXCOORD2;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        output.interp1.xyzw = input.texCoord0;
        output.interp2.xyzw = input.color;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        output.texCoord0 = input.interp1.xyzw;
        output.color = input.interp2.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float Vector1_7CF07853;
float4 _MainTex_TexelSize;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// Graph Functions

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Cosine_float(float In, out float Out)
{
    Out = cos(In);
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
}

void Unity_InvertColors_float(float In, float InvertColors, out float Out)
{
    Out = abs(InvertColors - In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    float4 _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
    float _Split_b11c60663a22868d84a350e78580dca2_R_1 = _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0[0];
    float _Split_b11c60663a22868d84a350e78580dca2_G_2 = _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0[1];
    float _Split_b11c60663a22868d84a350e78580dca2_B_3 = _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0[2];
    float _Split_b11c60663a22868d84a350e78580dca2_A_4 = _ScreenPosition_4247242be4dda087bae8fe20713a827f_Out_0[3];
    float _Multiply_2893f53b2903fd8e87465c676555b30c_Out_2;
    Unity_Multiply_float(_Split_b11c60663a22868d84a350e78580dca2_G_2, _ScreenParams.y, _Multiply_2893f53b2903fd8e87465c676555b30c_Out_2);
    float _Cosine_c95396e519d0358ba925dd50a035f752_Out_1;
    Unity_Cosine_float(_Multiply_2893f53b2903fd8e87465c676555b30c_Out_2, _Cosine_c95396e519d0358ba925dd50a035f752_Out_1);
    float _Add_d07cd68b65c9fb899e77e39bf87b9f9e_Out_2;
    Unity_Add_float(_Cosine_c95396e519d0358ba925dd50a035f752_Out_1, 1, _Add_d07cd68b65c9fb899e77e39bf87b9f9e_Out_2);
    float _Add_aef09e52f73f9988b4963a7d23b67c8b_Out_2;
    Unity_Add_float(0.1, 1, _Add_aef09e52f73f9988b4963a7d23b67c8b_Out_2);
    float _Multiply_4249b5fcc90ced8e9c088c7895b89151_Out_2;
    Unity_Multiply_float(_Add_d07cd68b65c9fb899e77e39bf87b9f9e_Out_2, _Add_aef09e52f73f9988b4963a7d23b67c8b_Out_2, _Multiply_4249b5fcc90ced8e9c088c7895b89151_Out_2);
    UnityTexture2D _Property_3497ccb92c466a8494f763a5eeef400f_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_3497ccb92c466a8494f763a5eeef400f_Out_0.tex, _Property_3497ccb92c466a8494f763a5eeef400f_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_R_4 = _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0.r;
    float _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_G_5 = _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0.g;
    float _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_B_6 = _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0.b;
    float _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_A_7 = _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_RGBA_0.a;
    float4 _Combine_685109d740f1a682836aec119a164b99_RGBA_4;
    float3 _Combine_685109d740f1a682836aec119a164b99_RGB_5;
    float2 _Combine_685109d740f1a682836aec119a164b99_RG_6;
    Unity_Combine_float(_SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_R_4, 0, _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_B_6, 0, _Combine_685109d740f1a682836aec119a164b99_RGBA_4, _Combine_685109d740f1a682836aec119a164b99_RGB_5, _Combine_685109d740f1a682836aec119a164b99_RG_6);
    float4 _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2;
    Unity_Multiply_float((_Multiply_4249b5fcc90ced8e9c088c7895b89151_Out_2.xxxx), _Combine_685109d740f1a682836aec119a164b99_RGBA_4, _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2);
    float _Split_49bff29907c61482a948fdece0c79eba_R_1 = _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2[0];
    float _Split_49bff29907c61482a948fdece0c79eba_G_2 = _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2[1];
    float _Split_49bff29907c61482a948fdece0c79eba_B_3 = _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2[2];
    float _Split_49bff29907c61482a948fdece0c79eba_A_4 = _Multiply_fbd4f7df50bb2c868d254b3c72902746_Out_2[3];
    float _Sine_85dfdc8a05675d888997e7439485a746_Out_1;
    Unity_Sine_float(_Multiply_2893f53b2903fd8e87465c676555b30c_Out_2, _Sine_85dfdc8a05675d888997e7439485a746_Out_1);
    float _Add_844f05ad17c7c5819e9d3f1d3e3195c0_Out_2;
    Unity_Add_float(_Sine_85dfdc8a05675d888997e7439485a746_Out_1, 1, _Add_844f05ad17c7c5819e9d3f1d3e3195c0_Out_2);
    float _Add_d452580216ccd98d8d8b3e22d5ce555f_Out_2;
    Unity_Add_float(0.15, 1, _Add_d452580216ccd98d8d8b3e22d5ce555f_Out_2);
    float _Multiply_375bb0d45f564d8fbd9b7e6940e814f6_Out_2;
    Unity_Multiply_float(_Add_844f05ad17c7c5819e9d3f1d3e3195c0_Out_2, _Add_d452580216ccd98d8d8b3e22d5ce555f_Out_2, _Multiply_375bb0d45f564d8fbd9b7e6940e814f6_Out_2);
    float _Multiply_6580116c0efe398d80a161dbb6171039_Out_2;
    Unity_Multiply_float(_Multiply_375bb0d45f564d8fbd9b7e6940e814f6_Out_2, _SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_G_5, _Multiply_6580116c0efe398d80a161dbb6171039_Out_2);
    float4 _Combine_74d9a77fb7a64983a319833918e208f8_RGBA_4;
    float3 _Combine_74d9a77fb7a64983a319833918e208f8_RGB_5;
    float2 _Combine_74d9a77fb7a64983a319833918e208f8_RG_6;
    Unity_Combine_float(_Split_49bff29907c61482a948fdece0c79eba_R_1, _Multiply_6580116c0efe398d80a161dbb6171039_Out_2, _Split_49bff29907c61482a948fdece0c79eba_B_3, 0, _Combine_74d9a77fb7a64983a319833918e208f8_RGBA_4, _Combine_74d9a77fb7a64983a319833918e208f8_RGB_5, _Combine_74d9a77fb7a64983a319833918e208f8_RG_6);
    float4 _Multiply_dcf781d0c5f3fa84a4f2f60065bbe7b2_Out_2;
    Unity_Multiply_float(_Combine_74d9a77fb7a64983a319833918e208f8_RGBA_4, IN.VertexColor, _Multiply_dcf781d0c5f3fa84a4f2f60065bbe7b2_Out_2);
    float _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_Out_1;
    float _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_InvertColors = float(0
);    Unity_InvertColors_float(_SampleTexture2D_d4118b83c9dd1d8094077a0b61aa68dc_A_7, _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_InvertColors, _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_Out_1);
    surface.BaseColor = (_Multiply_dcf781d0c5f3fa84a4f2f60065bbe7b2_Out_2.xyz);
    surface.Alpha = _InvertColors_55c36b3c4201a08292a9970ed8aa8f7c_Out_1;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.WorldSpacePosition = input.positionWS;
    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
    output.uv0 = input.texCoord0;
    output.VertexColor = input.color;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

    ENDHLSL
}
    }
        FallBack "Hidden/Shader Graph/FallbackError"
}