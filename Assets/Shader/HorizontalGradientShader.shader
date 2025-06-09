Shader "UI/HorizontalGradient"
{
    Properties
    {
        _ColorLeft ("Left Color", Color) = (0,0,0,0.2)
        _ColorCenter ("Center Color", Color) = (0,0,0,0.8)
        _ColorRight ("Right Color", Color) = (0,0,0,0.2)
        _CenterWidth ("Center Width", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _ColorLeft;
            fixed4 _ColorCenter;
            fixed4 _ColorRight;
            float _CenterWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float x = i.uv.x;

                float halfWidth = _CenterWidth * 0.5;
                float leftBound = 0.5 - halfWidth;
                float rightBound = 0.5 + halfWidth;

                fixed4 col;

                if (x < leftBound)
                {
                    col = lerp(_ColorLeft, _ColorCenter, x / leftBound);
                }
                else if (x > rightBound)
                {
                    col = lerp(_ColorCenter, _ColorRight, (x - rightBound) / (1.0 - rightBound));
                }
                else
                {
                    col = _ColorCenter;
                }

                return col;
            }
            ENDCG
        }
    }
}
