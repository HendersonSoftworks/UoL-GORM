Shader "CRTFilterUnityCG"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			static const float pi = 3.141592653589793238462;

			float2 m_pixSize;

            uniform float p_time;
			uniform float p_screenBend;
			uniform float p_screenOverscan;
			uniform float p_blur;
			uniform float p_smidge;
			uniform float p_bleedr;
			uniform float p_bleedg;
			uniform float p_bleedb;
			uniform float p_resX;
			uniform float p_resY;
			uniform float p_scanlinesStrength;
			uniform float p_apertureStrength;
			uniform float p_shadowlines;			
			uniform float p_shadowlinesSpeed;
			uniform float p_shadowlinesAlpha;
			uniform float p_vignetteSize;
			uniform float p_vignetteSmooth;
			uniform float p_vignetteRound;
			uniform float p_noiseSize;
			uniform float p_noiseAlpha;
			uniform float p_noiseSpeed;
			uniform float p_brightness;
			uniform float p_contrast;
			uniform float p_gamma;
			uniform float p_red;
			uniform float p_green;
			uniform float p_blue;
			uniform float2 p_redOffset;
			uniform float2 p_greenOffset;
			uniform float2 p_blueOffset;

			float2 pixel_size()
			{
				return float2((_MainTex_TexelSize.z / p_resX) * _MainTex_TexelSize.x, (_MainTex_TexelSize.w / p_resY) * _MainTex_TexelSize.y);				
			}

			float2 pixel_part(float2 uv)
			{
				return float2(floor(fmod(uv.x, m_pixSize.x) * _MainTex_TexelSize.z), floor(fmod(uv.y, m_pixSize.y) * _MainTex_TexelSize.w));
			}
			float pixel_part_x(float uvx)
			{
				return floor(fmod(uvx, m_pixSize.x) * _MainTex_TexelSize.z);
			}
			float pixel_part_y(float uvy)
			{
				return floor(fmod(uvy, m_pixSize.y) * _MainTex_TexelSize.w);
			}

			float2 pixel_frac(float2 uv)
			{
				return float2(fmod(uv.x, m_pixSize.x) * p_resX, fmod(uv.y, m_pixSize.y) * p_resY);
			}
			float pixel_frac_x(float uvx)
			{
				return fmod(uvx, m_pixSize.x) * p_resX;
			}
			float pixel_frac_y(float uvy)
			{
				return fmod(uvy, m_pixSize.y) * p_resY;
			}

			float2 pixel_num(float2 uv)
			{
				return float2(floor(uv.x / m_pixSize.x), floor(uv.y / m_pixSize.y));
			}
			float pixel_num_x(float uvx)
			{
				return floor(uvx / m_pixSize.x);
			}
			float pixel_num_y(float uvy)
			{
				return floor(uvy / m_pixSize.y);
			}

            float random (float2 uv)
            {
                return frac(sin(dot(uv,float2(12.4898,78.233)))	* 43758.541987 * sin(p_time * p_noiseSpeed));
            }

			float noise(float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac(uv);

				float a = random(i);
				float b = random(i + float2(1., 0.));
				float c = random(i + float2(0, 1.));
				float d = random(i + float2(1., 1.));

				float2 u = smoothstep(0., 1., f);

				return lerp(a, b, u.x) + (c - a) * u.y * (1. - u.x) + (d - b) * u.x * u.y;
			}

			float vignette(float2 uv)
			{
				uv -= .5;
				uv *= p_vignetteSize;
				float amount = 1. - sqrt(pow(abs(uv.x), p_vignetteRound) + pow(abs(uv.y), p_vignetteRound));				

				return smoothstep(0, p_vignetteSmooth, amount);
			}

			float crt_line(float i, float lines, float speed)
			{
				return sin(i * lines * pi + speed * p_time);
			}

			float2 screen_bend(float2 uv)
			{
				uv -= 0.5;
				uv *= 2.;
				uv.x *= 1. + pow(uv.y / p_screenBend, 2.) - p_screenOverscan;
				uv.y *= 1. + pow(uv.x / p_screenBend, 2.) - p_screenOverscan;
				uv /= 2.;
				return uv + 0.5;
			}

			float4 blur (float4 col, float2 uv)
            {                
				col += tex2D(_MainTex, uv + float2(p_blur, p_blur));
                col += tex2D(_MainTex, uv + float2(p_blur, -p_blur));
                col += tex2D(_MainTex, uv + float2(-p_blur, p_blur));
                col += tex2D(_MainTex, uv + float2(-p_blur, -p_blur));
                col /= 5.;
                
                return col;
            }

			float weight(float4 color)
			{
				return max(max(color.r * p_bleedr, color.g * p_bleedg), color.b * p_bleedb);
			}

			float4 bleed(float4 col, float2 uv)
            {
				float side2 = ceil((_MainTex_TexelSize.z / p_resX) / 2.);
				float side1 = side2 - 1;

				float s = 2.;
				col *= s;

				float4 bld = tex2D(_MainTex, uv + float2(_MainTex_TexelSize.x * side1, 0));
				float w = weight(bld);
				col += bld * w;
				s += min(w, 1.);

				bld = tex2D(_MainTex, uv + float2(_MainTex_TexelSize.x * side2, 0));
				w = weight(bld);
				col += bld * w;
				s += min(w, 1.);

				bld = tex2D(_MainTex, uv - float2(_MainTex_TexelSize.x * side1, 0));
				w = weight(bld);
				col += bld * w;
				s += min(w, 1.);


				bld = tex2D(_MainTex, uv - float2(_MainTex_TexelSize.x * side2, 0));
				w = weight(bld);
				col += bld * w;
				s += min(w, 1.);

				return col / s;
            }

			float4 aperture(float4 col, float2 uv)
			{
				float odd = fmod(floor(uv.x * _MainTex_TexelSize.z), 2.);

				col *= 1 - odd * p_apertureStrength;
								
				return col;
			}

			float4 scanline(float4 col, float2 uv)
			{
				return col * lerp(1, max(0.4, sin(pixel_frac_y(uv.y - _MainTex_TexelSize.y / 2.) * pi)), p_scanlinesStrength);
			}

			float4 pixelSmidge(float4 col, float2 uv)
			{
				float4 smgL = tex2D(_MainTex, uv + float2(-m_pixSize.x / 2, -m_pixSize.y));
				float4 smgR = tex2D(_MainTex, uv + float2(m_pixSize.x / 2, -m_pixSize.y));
				
				float sL = step(0.4, max(smgL.r, max(smgL.g, smgL.b)));
				float sR = step(0.4, max(smgR.r, max(smgR.g, smgR.b)));
				smgL *= sL;
				smgR *= sR;
				
				float s = abs(sL - sR) * p_smidge;
				s *= 1 - step(0.10, col.r + col.g + col.b);
				s *= 1 - pixel_frac_y(uv.y);

				return col + (smgL + smgR) * s;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				m_pixSize = pixel_size();

				float2 buv = screen_bend(i.uv);
				
				float4 col = tex2D(_MainTex, buv);
				
				col = blur(col, buv);

				col.r += tex2D(_MainTex, buv + p_redOffset).r;
				col.g += tex2D(_MainTex, buv + p_greenOffset).g;
				col.b += tex2D(_MainTex, buv + p_blueOffset).b;
				col.rgb /= 2.0f;

				col = bleed(col, buv);
				col = scanline(col, buv);
				col = aperture(col, buv);
				col = pixelSmidge(col, buv);

				col = pow(col, (1 / p_gamma));
				col = p_contrast * (col - 0.5) + 0.5 + p_brightness;

				col.r *= p_red;
				col.g *= p_green;
				col.b *= p_blue;
				
				col = lerp(col, float(noise(buv * p_noiseSize)), p_noiseAlpha);
				col = lerp(col, crt_line(buv.y, p_shadowlines, p_shadowlinesSpeed), p_shadowlinesAlpha * max(0, min(1, p_shadowlines)));
				col = lerp(col, crt_line(buv.x, p_shadowlines, p_shadowlinesSpeed), p_shadowlinesAlpha * max(0, min(1, -p_shadowlines)));

				return col * vignette(i.uv);
			}

            ENDCG
        }
    }
}
