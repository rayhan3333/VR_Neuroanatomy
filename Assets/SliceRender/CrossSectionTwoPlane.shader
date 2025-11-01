Shader "Custom/CrossSectionWithTwoPlanes"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _CrossSectionColor ("Cross-Section Color", Color) = (1, 0, 0, 1)
        _Plane1Position ("Plane1 Position", Vector) = (0, .25, 0, 0)
        _Plane1Normal ("Plane1 Normal", Vector) = (0, 1, 0, 0)
        _Plane2Position ("Plane2 Position", Vector) = (0, -.25, 0, 0)
        _Plane2Normal ("Plane2 Normal", Vector) = (0, -1, 0, 0) // Inverse normal for Plane2
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
            float4 _Plane1Position;
            float4 _Plane1Normal;
            float4 _Plane2Position;
            float4 _Plane2Normal;
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

                // Calculate signed distance from Plane1
                float distanceToPlane1 = dot(v.vertex.xyz - _Plane1Position.xyz, _Plane1Normal.xyz);

                // Calculate signed distance from Plane2
                float distanceToPlane2 = dot(v.vertex.xyz - _Plane2Position.xyz, _Plane2Normal.xyz);

                // Check if vertex lies between the two planes
                if (distanceToPlane1 > -_CrossSectionThickness && distanceToPlane2 < _CrossSectionThickness)
                {
                    o.color = _CrossSectionColor; // Set to cross-section color if between planes
                }
                else
                {
                    o.color.a = 0; // Make the object invisible if outside the planes
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
