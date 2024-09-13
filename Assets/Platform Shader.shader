Shader "Custom/Platform"
{
    Properties
    {
        _LineWidth ("Line Width", Float) = 0.1 // Ширина линий сетки
		_FirstColor ("First Color", COLOR) = (1,1,1,1)
		_SecondColor ("Second Color", COLOR) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _LineWidth;
			float4 _FirstColor;
			float4 _SecondColor;

            // Function for calculating filter width
            float filter_width(float2 v) {
                float2 fw = max(abs(ddx(v)), abs(ddy(v)));
                return max(fw.x, fw.y);
            }

            // Functionized version of the BUMP_INT macro
            float2 bump_int(float2 i) {
                return floor(i / 2) + 2.f * max((i / 2) - floor((i / 2)) - .5f, 0.f);
            }

            // Function to calculate checker pattern
            float4 checker(float2 uv) {
                float width = filter_width(uv); // or fwidth(uv)
                float2 p0 = uv - .5 * width, p1 = uv + .5 * width;
                float2 i = (bump_int(p1) - bump_int(p0)) / width;
                return (i.x * i.y + (1 - i.x) * (1 - i.y)) * _FirstColor;
            }

            // The actual fragment shader
            fixed4 frag(v2f_img v2f) : SV_Target
            {
                // Get world position in meters
                float3 worldPos = mul(unity_ObjectToWorld, float4(v2f.uv, 0, 1)).xyz;

                // Divide world position by 1 to get 1x1 meter grid squares
                float2 gridUV = worldPos.xy;

                // Generate the checker pattern based on world position (1x1 meter grid)
                return checker(gridUV);
            }
            ENDCG
        }
    }
}
