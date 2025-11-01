Shader "Custom/PlaneIntersectionShader"
{
     Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _HighlightColor ("Highlight Color", Color) = (1, 0, 0, 1)
        _IntersectionMask ("Intersection Mask", 2D) = "white" { }
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            uniform float4 _BaseColor;
            uniform float4 _HighlightColor;
            uniform sampler2D _IntersectionMask; // Texture with intersection regions

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy * 0.5 + 0.5; // Convert to UV (assuming the plane is aligned to X,Y or Y,Z)
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Sample the intersection mask at the current UV
                float mask = tex2D(_IntersectionMask, i.uv).r;

                // If the intersection mask is non-zero, apply the highlight color
                if (mask > 0.0)
                {
                    return _HighlightColor;
                }

                // Otherwise, return the base color
                return _BaseColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
