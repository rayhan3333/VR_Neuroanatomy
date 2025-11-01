Shader "Custom/3DTextureSlice"
{
     Properties
    {
        _VolumeTex("3D Texture", 3D) = "" {}        
        _SliceIndex("Slice Index", Float) = 0.0    
        _Axis("Axis (0Coronal 1Axial 2Sagittal)", Int) = 0
        
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler3D _VolumeTex;  
            float _SliceIndex;   
            float _Axis;  
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; 
            };

            struct v2f
            {
                float2 uv : TEXCOORD0; 
                float4 pos : SV_POSITION; 
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); 
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
               float3 texCoord;
                if (_Axis==0) {
                    texCoord = float3(i.uv, _SliceIndex);
                }
                if (_Axis==1) {
                    texCoord = float3(i.uv.x, _SliceIndex, i.uv.y);
                }
                if (_Axis==2) {
                    texCoord = float3(_SliceIndex, i.uv);
                }
                
                
                return tex3D(_VolumeTex, texCoord);       
            }
            ENDCG
        }
    }
}
