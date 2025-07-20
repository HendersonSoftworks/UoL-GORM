Shader "CRTFilterLite"
{
    HLSLINCLUDE
    
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        // The Blit.hlsl file provides the vertex shader (Vert),
        // the input structure (Attributes), and the output structure (Varyings)
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"			

		// parameters
        uniform half p_time;
		uniform half p_screenBend;
		uniform half p_screenOverscan;
		uniform half p_blur;
		uniform half p_smidge;
		uniform half p_bleedr;
		uniform half p_bleedg;
		uniform half p_bleedb;
		uniform half p_resX;
		uniform half p_resY;
		uniform half p_scanlinesStrength;
		uniform half p_apertureStrength;
		uniform half p_shadowlines;			
		uniform half p_shadowlinesSpeed;
		uniform half p_shadowlinesAlpha;
		uniform half p_vignetteSize;
		uniform half p_vignetteSmooth;
		uniform half p_vignetteRound;
		uniform half p_noiseSize;
		uniform half p_noiseAlpha;
		uniform half p_noiseSpeed;
		uniform half p_brightness;
		uniform half p_contrast;
		uniform half p_gamma;
		uniform half p_red;
		uniform half p_green;
		uniform half p_blue;
		uniform half2 p_redOffset;
		uniform half2 p_greenOffset;
		uniform half2 p_blueOffset;

		// variables
		static const half pi = 3.141592653589793238462;
		half2 m_pixSize;

		half3 tex(half2 uv)
		{
			return SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).rgb;
		}
		
		half2 pixel_size()
		{
			return half2((_BlitTexture_TexelSize.z / p_resX) * _BlitTexture_TexelSize.x, (_BlitTexture_TexelSize.w / p_resY) * _BlitTexture_TexelSize.y);				
		}

		half2 pixel_part(half2 uv)
		{
			return half2(floor(fmod(uv.x, m_pixSize.x) * _BlitTexture_TexelSize.z), floor(fmod(uv.y, m_pixSize.y) * _BlitTexture_TexelSize.w));
		}
		half pixel_part_x(half uvx)
		{
			return floor(fmod(uvx, m_pixSize.x) * _BlitTexture_TexelSize.z);
		}
		half pixel_part_y(half uvy)
		{
			return floor(fmod(uvy, m_pixSize.y) * _BlitTexture_TexelSize.w);
		}

		half2 pixel_frac(half2 uv)
		{
			return half2(fmod(uv.x, m_pixSize.x) * p_resX, fmod(uv.y, m_pixSize.y) * p_resY);
		}
		half pixel_frac_x(half uvx)
		{
			return fmod(uvx, m_pixSize.x) * p_resX;
		}
		half pixel_frac_y(half uvy)
		{
			return fmod(uvy, m_pixSize.y) * p_resY;
		}

		half2 pixel_num(half2 uv)
		{
			return half2(floor(uv.x / m_pixSize.x), floor(uv.y / m_pixSize.y));
		}
		half pixel_num_x(half uvx)
		{
			return floor(uvx / m_pixSize.x);
		}
		half pixel_num_y(half uvy)
		{
			return floor(uvy / m_pixSize.y);
		}

        half random (half2 uv)
        {
            return frac(sin(dot(uv,half2(12.4898,78.233)))	* 43758.541987 * sin(p_time * p_noiseSpeed));
        }

		half noise(half2 uv)
		{
			half2 i = floor(uv);
			half2 f = frac(uv);

			half a = random(i);
			half b = random(i + half2(1., 0.));
			half c = random(i + half2(0, 1.));
			half d = random(i + half2(1., 1.));

			half2 u = smoothstep(0., 1., f);

			return lerp(a, b, u.x) + (c - a) * u.y * (1. - u.x) + (d - b) * u.x * u.y;
		}

		half vignette(half2 uv)
		{
			uv -= .5;
			uv *= p_vignetteSize;
			half amount = 1. - sqrt(pow(abs(uv.x), p_vignetteRound) + pow(abs(uv.y), p_vignetteRound));				

			return smoothstep(0, p_vignetteSmooth, amount);
		}

		half crt_line(half i, half lines, half speed)
		{
			return sin(i * lines * pi + speed * p_time);
		}

		half2 screen_bend(half2 uv)
		{
			uv -= 0.5;
			uv *= 2.;
			uv.x *= 1. + pow(uv.y / p_screenBend, 2.) - p_screenOverscan;
			uv.y *= 1. + pow(uv.x / p_screenBend, 2.) - p_screenOverscan;
			uv /= 2.;
			return uv + 0.5;
		}

		half3 blur (half3 col, half2 uv)
        {                
			col += tex(uv + half2(p_blur, p_blur));
            col += tex(uv + half2(p_blur, -p_blur));
            col += tex(uv + half2(-p_blur, p_blur));
            col += tex(uv + half2(-p_blur, -p_blur));
            col /= 5.;
                
            return col;
        }

		half weight(half3 color)
		{
			return max(max(color.r * p_bleedr, color.g * p_bleedg), color.b * p_bleedb);
		}

		half3 bleed(half3 col, half2 uv)
        {
			half side2 = ceil((_BlitTexture_TexelSize.z / p_resX) / 2.);
			half side1 = side2 - 1;

			half s = 2.;
			col *= s;

			half3 bld = tex(uv + half2(_BlitTexture_TexelSize.x * side1, 0));
			half w = weight(bld);
			col += bld * w;
			s += min(w, 1.);

			bld = tex(uv + half2(_BlitTexture_TexelSize.x * side2, 0));
			w = weight(bld);
			col += bld * w;
			s += min(w, 1.);

			bld = tex(uv - half2(_BlitTexture_TexelSize.x * side1, 0));
			w = weight(bld);
			col += bld * w;
			s += min(w, 1.);


			bld = tex(uv - half2(_BlitTexture_TexelSize.x * side2, 0));
			w = weight(bld);
			col += bld * w;
			s += min(w, 1.);

			return col / s;
        }

		half3 aperture(half3 col, half2 uv)
		{
			half odd = fmod(floor(uv.x * _BlitTexture_TexelSize.z), 2.);

			col *= 1 - odd * p_apertureStrength;
								
			return col;
		}

		half3 scanline(half3 col, half2 uv)
		{
			return col * lerp(1, max(0.4, sin(pixel_frac_y(uv.y - _BlitTexture_TexelSize.y / 2.) * pi)), p_scanlinesStrength);
		}

		half3 pixelSmidge(half3 col, half2 uv)
		{
			half3 smgL = tex(uv + half2(-m_pixSize.x / 2, -m_pixSize.y));
			half3 smgR = tex(uv + half2(m_pixSize.x / 2, -m_pixSize.y));
				
			half sL = step(0.4, max(smgL.r, max(smgL.g, smgL.b)));
			half sR = step(0.4, max(smgR.r, max(smgR.g, smgR.b)));
			smgL *= sL;
			smgR *= sR;
				
			half s = abs(sL - sR) * p_smidge;
			s *= 1 - step(0.10, col.r + col.g + col.b);
			s *= 1 - pixel_frac_y(uv.y);

			return col + (smgL + smgR) * s;
		}
		
		float4 ApplyCRT (Varyings input) : SV_Target
		{
			m_pixSize = pixel_size();

			//if SCREEN GEOMETRY is not required, use commented line instead of the current one
			half2 buv = screen_bend(input.texcoord);
			//half2 buv = input.texcoord;
				
			half3 col = tex(buv);

			//col = blur(col, buv);

			//CHROMATIC ABERRATION
			//col.r += tex(buv + p_redOffset).r;
			//col.g += tex(buv + p_greenOffset).g;
			//col.b += tex(buv + p_blueOffset).b;
			//col.rgb /= 2.0f;

			col = bleed(col, buv);
			col = scanline(col, buv);
			//col = aperture(col, buv);
			//col = pixelSmidge(col, buv);

			//col = pow(col, (1 / p_gamma));
			//col = p_contrast * (col - 0.5) + 0.5 + p_brightness;

			//RGB ADJUSTEMENTS
			//col.r *= p_red;
			//col.g *= p_green;
			//col.b *= p_blue;

			//NOISE
			col = lerp(col, half(noise(buv * p_noiseSize)), p_noiseAlpha);

			//SHADOWLINES
			col = lerp(col, crt_line(buv.y, p_shadowlines, p_shadowlinesSpeed), p_shadowlinesAlpha * max(0, min(1, p_shadowlines)));
			col = lerp(col, crt_line(buv.x, p_shadowlines, p_shadowlinesSpeed), p_shadowlinesAlpha * max(0, min(1, -p_shadowlines)));

			return float4(col * vignette(input.texcoord), 1);
		}
    
    ENDHLSL
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "CRTFilterPass"

            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment ApplyCRT
            
            ENDHLSL
        }
    }
}