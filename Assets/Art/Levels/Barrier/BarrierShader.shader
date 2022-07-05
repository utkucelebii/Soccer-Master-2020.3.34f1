Shader "Custom/BarrierShader"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _Smoothness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            Cull Off

            CGPROGRAM
            #pragma surface surf Standard vertex:vert
            #pragma target 3.0

            sampler2D _MainTex;
            half _Smoothness;
            half _Metallic;
            fixed4 _Color;

            struct Input {
                float2 uv_MainTex;
            };

            void vert(inout appdata_full v) {
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNorm = UnityObjectToWorldNormal(v.normal);
                float3 viewDir = worldPos - _WorldSpaceCameraPos;
                v.normal *= dot(viewDir, worldNorm) > 0 ? -1 : 1;
            }

            void surf(Input IN, inout SurfaceOutputStandard o) {
                half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Smoothness = _Smoothness;
                o.Metallic = _Metallic;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
}