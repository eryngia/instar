Shader "Custom/ColorMapShader"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _ColorWhite ("Color for White", Color) = (1, 1, 1, 1)
        _ColorBlack ("Color for Black", Color) = (0, 0, 0, 1)
        _ColorRed ("Color for Red", Color) = (1, 0, 0, 1)
        _GrayRatio ("Gray Ratio", Float) = 0.2
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _ColorWhite;
            fixed4 _ColorBlack;
            fixed4 _ColorRed;
            float _GrayRatio;

            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 color = col.rgb;

                float3 white = float3(1, 1, 1);
                float3 black = float3(0, 0, 0);
                float3 red   = float3(1, 0, 0);
                float3 green  = float3(0, 1, 0);    

                // Distances to color references
                float distWhite = distance(color, white);
                float distBlack = distance(color, black);
                float distRed   = distance(color, red);
                float distGreen = distance(color, green);
                
                
                // Find closest color
                float minDist = min(min(distWhite, distBlack), min(distRed, distGreen));
                
                if (minDist == distWhite)
                color = _ColorWhite.rgb;
                else if (minDist == distBlack)
                color = _ColorBlack.rgb;
                else if (minDist == distRed)
                color = _ColorRed.rgb;
                else 
                color = _ColorWhite.rgb * _GrayRatio;

                return fixed4(color, col.a);
            }            
            ENDCG
        }
    }
}
