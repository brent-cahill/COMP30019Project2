// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/CelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Cel Threshold", Range(1., 20.)) = 3.
        _Ambient ("Ambient intensity", Range(0., 0.5)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _LightColor0;
            half _Ambient;
  
            // Define a struct for the vertex that will be passed into the
            // vertex shader
            struct vertIn
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
 
            // Threshold value for the cel breakdown that will be used later
            float _Threshold;
            
            // Calculates L.N to be used in the calculation of diffuse RGB reflections
            float CelShading(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));
                return floor(NdotL * _Threshold) / (_Threshold - 0.5);
            }
            
            // Vertex shader which takes the vertex data in the form of appdata_full
            vertIn vert (appdata_full v)
            {
                // vertIn to be returned, which will be transformed from world
                // space into object space. The uvs give us the ability to wrap
                // the texture (with the cel shading) onto the object
                vertIn o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = mul(v.normal.xyz, (float3x3) unity_WorldToObject);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
 
            fixed4 frag (vertIn i) : SV_Target
            {
                // Finally, our fragment shader puts the final touches onto the
                // object's shading, giving the color of specific threshold
                // areas, depending on the number of cuts
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= saturate(CelShading(i.normal, _WorldSpaceLightPos0.xyz) + _Ambient) * _LightColor0.rgb;
                return col;
            }
            ENDCG
        }
    }
}