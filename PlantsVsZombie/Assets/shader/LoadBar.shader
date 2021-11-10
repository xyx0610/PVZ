// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/LoadBarMask" {
   Properties
    {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _Progress ("Progress", Range(0,1)) = 0
    }
 
    Category
    {
        Lighting Off
        ZWrite Off
        Cull back
        Fog { Mode Off }
        Tags {"Queue"="Transparent" "IgnoreProjector"="True"}
        Blend SrcAlpha OneMinusSrcAlpha
        SubShader
        {
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                sampler2D _MainTex;
                fixed4 _Color;
                float _Progress;
                struct appdata
                {
                    float4 vertex : POSITION;
                    float4 texcoord : TEXCOORD0;
                };
                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };
                v2f vert (appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord.xy;
                    return o;
                }
                half4 frag(v2f i) : COLOR
                {
                    fixed4 c = _Color * tex2D(_MainTex, i.uv);
                    c.a *= i.uv.x >= _Progress ? 0 : 1;
                    return c;
                }
                ENDCG
            }
        }
 
        SubShader
        {          
             AlphaTest LEqual [_Progress] 
              Pass 
              { 
                 SetTexture [_MaskTex] {combine texture} 
                 SetTexture [_MainTex] {combine texture, previous} 
              } 
        }
         
    }
    Fallback "LoadBar"
}