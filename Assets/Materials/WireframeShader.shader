Shader "Custom/Wireframe"
{
    Properties
    {
        _Color ("Color", Color) = (0.5,0.5,0.5,0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Cull Off
            Lighting Off
            ZWrite On
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            #include "UnityCG.cginc"

            fixed4 _Color;

            struct appdata { float4 vertex : POSITION; };
            struct v2g { float4 vertex : POSITION; };
            struct g2f { float4 vertex : SV_POSITION; };

            v2g vert(appdata v)
            {
                v2g o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            [maxvertexcount(24)]
            void geom(triangle v2g IN[3], inout LineStream<g2f> lineStream)
            {
                for (int i = 0; i < 3; i++)
                {
                    g2f o1, o2;
                    o1.vertex = IN[i].vertex;
                    o2.vertex = IN[uint(i+1)%3].vertex;
                    lineStream.Append(o1);
                    lineStream.Append(o2);
                }
            }

            fixed4 frag(g2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
    FallBack Off
}
