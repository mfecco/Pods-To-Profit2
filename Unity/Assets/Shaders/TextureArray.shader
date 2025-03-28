//Maija - I am not a shader expert, this code is credit to Holistic3D's TextureArray tutorial: https://www.youtube.com/watch?v=Q60cdwZDyjE

Shader "Custom/TextureArray"
{
    Properties
    {
        _MainTex("Texture Array", 2DArray) = "white" {}
    }

    SubShader
    {
        Tags {"RenderType" = "Opaque"}
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        UNITY_DECLARE_TEX2DARRAY(_MainTex);

        struct Input
        {
            float2 uv_MainTex;
            float arrayIndex;

        };

        void vert(inout appdata_full v, out Input o)
        {

            o.uv_MainTex= v.texcoord.xy;
            o.arrayIndex = v.texcoord.z;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(IN.uv_MainTex, IN.arrayIndex));
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
