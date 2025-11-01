Shader "Custom/CrossSection"
{
       Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _CrossSectionColor ("Cross-Section Color", Color) = (1, 0, 0, 1)
        _PlanePosition ("Plane Position", Vector) = (0, 0, 0, 0)
        _PlaneNormal ("Plane Normal", Vector) = (0, 1, 0, 0)
        _Plane2Position ("Plane Position", Vector) = (0, 0, 0, 0)
        _Plane2Normal ("Plane Normal", Vector) = (0, 1, 0, 0)
        _CrossSectionThickness ("Cross-Section Thickness", Float) = 0.05
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

            // Shader properties
            float4 _Color;
            float4 _CrossSectionColor;
            float4 _PlanePosition;
            float4 _PlaneNormal;
            float _CrossSectionThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _Color;

                // Calculate the signed distance of the vertex from the plane
                float distanceToPlane = dot(v.vertex.xyz - _PlanePosition.xyz, _PlaneNormal.xyz);

                // Check if the vertex is within the thickness threshold
                if (abs(distanceToPlane) < _CrossSectionThickness)
                {
                    o.color = _CrossSectionColor; // Set to cross-section color if within threshold
                }

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return i.color; // Return the color of the vertex
            }

            ENDCG
        }
    }

    Fallback "Diffuse"
}
