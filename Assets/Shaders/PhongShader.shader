// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// This code was modified from an example Phong Shader provided at:
// http://janhalozan.com/2017/08/12/phong-shader/
// We decided to implement this version of the Phong Shader as it was cleaner to
// implement for a given arbitrary, nonscripted GameObject.

Shader "PhongShader" {
    Properties {
        // The colour and texture of the GameObject
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Tex ("Pattern", 2D) = "white" {}
        
        // The "Shininess," or specular reflection intensity of the shader
        _Shininess ("Shininess", Float) = 10
        
        // The colour of said specular highlights.
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 200 // Variable that sets the level of detail

        Pass {
            Tags { "LightMode" = "ForwardBase" } //For the first light

            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                
                // This gives us access to the data from the directional light,
                // the camera, etc.
                #include "UnityCG.cginc"
                
                // Comes from unityCG, gives us color of the light
                uniform float4 _LightColor0;
                
                // Texture and tiling, respectively
                sampler2D _Tex;
                float4 _Tex_ST;
                
                // Access the variables from the properties here
                uniform float4 _Color;
                uniform float4 _SpecColor;
                uniform float _Shininess;
                
                // Define a struct for the data that will be passed to the
                // vertex shader
                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };
                
                // Define a struct for the vertex that will be passed into the
                // vertex shader
                struct vertIn
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                    float4 worldPos : TEXCOORD1;
                };
                
                // Vertex shader
                vertIn vert(appdata v)
                {
                    // Inital vertex struct as definted above that will be
                    // returned
                    vertIn o;
                    
                    // Here, we calculate the world position for the vertex
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                    
                    // now calculate the normal using v.normal
                    o.normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                    
                    // finally calculate the position
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _Tex);
                    
                    // return the vertIn
                    return o;
                }
                
                // Fragment shader where the bulk of the calculations actually
                // occur.
                fixed4 frag(vertIn i) : COLOR
                {
                    // Get the normalized direction using the passed vertex
                    // normal
                    float3 normalDirection = normalize(i.normal);
                    
                    // calculate the direction of the viewer using the camera
                    // data position.
                    float3 viewDirection = normalize(_WorldSpaceCameraPos - i.worldPos.xyz);
                    
                    
                    // Gives us the distance from the vertex to the directional
                    // light
                    float3 vert2LightSource = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
                    
                    // Gives us the inverse of the distance to be used in the
                    // attenuation calculation
                    float oneOverDistance = 1.0 / length(vert2LightSource);
                    float attenuation = lerp(1.0, oneOverDistance, _WorldSpaceLightPos0.w);
                    
                    float3 lightDirection = _WorldSpaceLightPos0.xyz - i.worldPos.xyz * _WorldSpaceLightPos0.w;
                    
                    // Calculation of the ambient component of the model
                    float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
                    
                    // Calculation of the diffuse component of the model
                    float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));
                    
                    // Calculation of the specular component of the model
                    float3 specularReflection;
                    // If the light is at the back of the GameObject
                    if (dot(i.normal, lightDirection) < 0.0)
                    {
                        // don't add any specular reflection
                        specularReflection = float3(0.0, 0.0, 0.0);
                    }
                    else
                    {
                        // Otherwise, calculate it as follows
                        specularReflection = attenuation * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
                    }
                    
                    // Finally, give the color, where the texture does not apply
                    // to the specular reflection.
                    float3 color = (ambientLighting + diffuseReflection) * tex2D(_Tex, i.uv) + specularReflection; //Texture is not applient on specularReflection
                    return float4(color, 1.0);
                }
            ENDCG
        }
    }
}