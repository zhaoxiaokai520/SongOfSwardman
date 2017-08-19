// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VolumetricFogAndMist/VolumetricFog" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NoiseTex ("Noise (RGB)", 2D) = "white" {}
		_FogDensity ("Density", Range (0, 1)) = 1
		_FogAlpha ("Alpha", Range (0, 1)) = 1
		_FogDistance ("Distance", Vector) = (0, 1, 1000, 0) 
		_FogHeight ("Height", Range (0, 100)) = 1
		_FogBaseHeight("Baseline Height", float) = 0
		_FogScale ("Scale", Range (1, 10)) = 1
		_Color ("Fog Color", Color) = (0.9,0.9,0.9)
		_FogSkyColor ("Sky Color", Color) = (0.9,0.9,0.9,0.8)
		_FogSkyData ("Sky Haze Data", Vector) = (50, 0, 0.3, 0.999)
		_FogWindDir ("Wind Direction", Vector) = (1,0,0)	
		_FogStepping ("Fog Stepping", Vector) = (0.0833333, 1, 0.0005)
		_FogVoidPosition("Fog Void Position", Vector) = (0,0,0)
		_FogVoidData("Fog Void Data", Vector) = (0,0,0,1) // xyz = size, w = falloff
		_FogOfWarCenter("Fog Of War Center", Vector) = (0,0,0)
		_FogOfWarSize("Fog Of War Size", Vector) = (1,1,1)
		_FogOfWar ("Fog of War Mask", 2D) = "white" {}
		_FogPointLightPosition0("Point Light 1 Position", Vector) = (0,0,0)
		_FogPointLightColor0("Point Light 1 Color", Color) = (1,1,0,1)
		_FogPointLightRadius0("Point Light 1 Radius", float) = 5
		_FogPointLightPosition1("Point Light 2 Position", Vector) = (0,0,0)
		_FogPointLightColor1("Point Light 2 Color", Color) = (1,1,0,1)
		_FogPointLightRadius1("Point Light 2 Radius", float) = 5
		_FogPointLightPosition2("Point Light 3 Position", Vector) = (0,0,0)
		_FogPointLightColor2("Point Light 3 Color", Color) = (1,1,0,1)
		_FogPointLightRadius2("Point Light 3 Radius", float) = 5
		_FogPointLightPosition3("Point Light 4 Position", Vector) = (0,0,0)
		_FogPointLightColor3("Point Light 4 Color", Color) = (1,1,0,1)
		_FogPointLightRadius3("Point Light 4 Radius", float) = 5
		_FogPointLightPosition4("Point Light 5 Position", Vector) = (0,0,0)
		_FogPointLightColor4("Point Light 5 Color", Color) = (1,1,0,1)
		_FogPointLightRadius4("Point Light 5 Radius", float) = 5
		_FogPointLightPosition5("Point Light 6 Position", Vector) = (0,0,0)
		_FogPointLightColor5("Point Light 6 Color", Color) = (1,1,0,1)
		_FogPointLightRadius5("Point Light 6 Radius", float) = 5
	}
        	
	CGINCLUDE
	#pragma fragmentoption ARB_precision_hint_fastest
	#pragma multi_compile __ FOG_DISTANCE_ON
	#pragma multi_compile __ FOG_OF_WAR_ON FOG_VOID_ON FOG_BOX_VOID_ON FOG_VOID_INVERTED FOG_BOX_VOID_INVERTED
	#pragma multi_compile __ FOG_HAZE_ON
	#pragma multi_compile __ FOG_POINT_LIGHT0 FOG_POINT_LIGHT1 FOG_POINT_LIGHT2 FOG_POINT_LIGHT3 FOG_POINT_LIGHT4 FOG_POINT_LIGHT5
	#pragma multi_compile __ FOG_SCATTERING_ON
	
	#pragma target 3.0
	#include "UnityCG.cginc"
	
	sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	sampler2D _NoiseTex;
	sampler2D_float _CameraDepthTexture;
	sampler2D_float _DepthTexture;
	sampler2D _FogDownsampled;
	float4 _FogDownsampled_TexelSize;
    float4x4 _ClipToWorld;
      
	half  _FogDensity;
	half  _FogAlpha;
	float4 _FogDistance;		// distance, distance * distance_falloff, max_distance, distance * (1 - distance_falloff)
	float  _FogHeight, _FogBaseHeight;
	half3 _Color;
	float3 _FogWindDir;
	float4  _FogStepping; // x = stepping, y = stepping near, z = edge improvement threshold, w = dithering on (=1)
	float  _FogScale;
	float3 _SunPosition;
	float3 _FarFrustum;

	#if FOG_VOID_ON	|| FOG_BOX_VOID_ON || FOG_VOID_INVERTED || FOG_BOX_VOID_INVERTED
	float3 _FogVoidPosition;	// xyz
	float4 _FogVoidData;
	#endif

	#if FOG_HAZE_ON
	half4  _FogSkyColor;
	#endif
	float4 _FogSkyData; // x = haze, y = noise, z = speed, w = depth (note, need to be available for all shader variants)
	
    #if FOG_OF_WAR_ON 
    sampler2D _FogOfWar;
    float3 _FogOfWarCenter;
    float3 _FogOfWarSize;
    float3 _FogOfWarCenterAdjusted;
    #endif
    
    #if FOG_POINT_LIGHT0 || FOG_POINT_LIGHT1 || FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
    float3 _FogPointLightPosition0;
    half3 _FogPointLightColor0;
    half  _FogPointLightRadius0;
    #endif

    #if FOG_POINT_LIGHT1 || FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
    float3 _FogPointLightPosition1;
    half3 _FogPointLightColor1;
    half  _FogPointLightRadius1;
    #endif

    #if FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
    float3 _FogPointLightPosition2;
    half3 _FogPointLightColor2;
    half  _FogPointLightRadius2;
    #endif

    #if FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
    float3 _FogPointLightPosition3;
    half3 _FogPointLightColor3;
    half  _FogPointLightRadius3;
    #endif

    #if FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
    float3 _FogPointLightPosition4;
    half3 _FogPointLightColor4;
    half  _FogPointLightRadius4;
    #endif
    
    #if FOG_POINT_LIGHT5
    float3 _FogPointLightPosition5;
    half3 _FogPointLightColor5;
    half  _FogPointLightRadius5;
    #endif
    
    #if FOG_SCATTERING_ON
    half4 _FogScatteringData;	// x = 1 / samples * spread, y = samples, z = exposure, w = weight
    half3 _FogScatteringData2;  // x = illumination, y = decay
    #endif

    const half4 zeros = half4(0,0,0,0);
    
    struct appdata {
    	float4 vertex : POSITION;
		half2 texcoord : TEXCOORD0;
    };
    
	struct v2f {
	    float4 pos : POSITION;
	    float2 uv: TEXCOORD0;
    	float2 depthUV : TEXCOORD1;
    	float3 cameraToFarPlane : TEXCOORD2;
	};

	v2f vert(appdata v) {
    	v2f o;
    	o.pos = UnityObjectToClipPos(v.vertex);
    	o.depthUV = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
   		o.uv = o.depthUV;
   	      
    	#if UNITY_UV_STARTS_AT_TOP
    	if (_MainTex_TexelSize.y < 0) {
	        // Depth texture is inverted WRT the main texture
    	    o.depthUV.y = 1 - o.depthUV.y;
    	}
    	#endif
               
    	// Clip space X and Y coords
    	float2 clipXY = o.pos.xy / o.pos.w;
               
    	// Position of the far plane in clip space
    	float4 farPlaneClip = float4(clipXY, 1, 1);
               
    	// Homogeneous world position on the far plane
    	farPlaneClip.y *= _ProjectionParams.x;	
    	float4 farPlaneWorld4 = mul(_ClipToWorld, farPlaneClip);
               
    	// World position on the far plane
    	float3 farPlaneWorld = farPlaneWorld4.xyz / farPlaneWorld4.w;
               
    	// Vector from the camera to the far plane
    	o.cameraToFarPlane = farPlaneWorld - _WorldSpaceCameraPos;
    	
    	return o;
	}


	float3 getWorldPos(v2f i, float depth01) {
    	// Reconstruct the world position of the pixel
     	_WorldSpaceCameraPos.y -= _FogBaseHeight;
    	float3 worldPos = (i.cameraToFarPlane * depth01) + _WorldSpaceCameraPos;
    	worldPos.y += 0.00001; // fixes artifacts when worldPos.y = _WorldSpaceCameraPos.y which is really rare but occurs at y = 0
    	return worldPos;
    }
    
	#if FOG_HAZE_ON
	half4 getSkyColor(half4 color, float3 worldPos, float2 uv) {
		// Compute sky color
		float y = 1.0f / max(worldPos.y + _FogBaseHeight, 1.0);
		float2 np = worldPos.xz * y * _FogScale + _Time[0] * _FogSkyData.z;
		float skyNoise = tex2D(_NoiseTex, np).a;
		if (_FogStepping.w) {
			float dither = dot(float2(2.4084507, 3.2535211), uv * _MainTex_TexelSize.zw);
			dither = frac(dither) - 0.5;
			skyNoise += dither * 0.04;
		}
		float t = _FogSkyColor.a * saturate( _FogSkyData.x * y * (1.0 - skyNoise*_FogSkyData.y) );
		color.rgb = lerp(color.rgb, _FogSkyColor.rgb, t);
		return color;
	}
	#endif

	half4 getFogColor(float2 uv, float3 worldPos, float depth01) {
		
		// early exit if fog is not crossed
		if (_WorldSpaceCameraPos.y>_FogHeight && worldPos.y>_FogHeight) {
			return zeros;		
		}
		if (_WorldSpaceCameraPos.y<-_FogHeight && worldPos.y<-_FogHeight) {
			return zeros;		
		}
 		
 		#if FOG_VOID_ON	|| FOG_BOX_VOID_ON || FOG_OF_WAR_ON || FOG_DISTANCE_ON
 		half voidAlpha = 1.0;
 		#endif
 				
		#if FOG_OF_WAR_ON
		if (depth01<_FogSkyData.w) {
			float2 fogTexCoord = worldPos.xz / _FogOfWarSize.xz - _FogOfWarCenterAdjusted.xz;
			voidAlpha = tex2D(_FogOfWar, fogTexCoord).a;
			if (voidAlpha <=0) return zeros;
		}
		#endif

		// Determine "fog length" and initial ray position between object and camera, cutting by fog distance params
		float3 adir = worldPos - _WorldSpaceCameraPos;
		
 		#if FOG_VOID_ON
		if (depth01<_FogSkyData.w) {
			float voidDistance = distance(_FogVoidPosition, worldPos) * _FogVoidData.x;
			voidAlpha *= saturate(lerp(1, voidDistance, _FogVoidData.w));
			if (voidAlpha <= 0) return zeros;
		}
		#elif FOG_BOX_VOID_ON
		if (depth01<_FogSkyData.w) {
			float3 absPos = abs(_FogVoidPosition - worldPos) * _FogVoidData.xyz;
			float voidDistance = max(max(absPos.x, absPos.y), absPos.z);
			voidAlpha *= saturate(lerp(1, voidDistance, _FogVoidData.w));
			if (voidAlpha <= 0) return zeros;
		}
		#elif FOG_VOID_INVERTED
			// early exit if ray does not sphere
		    float3 oc = _WorldSpaceCameraPos - _FogVoidPosition;
		    float c = dot(oc, oc) - _FogVoidData.y;
		    if (c>0) {
	    		float b = dot(normalize(adir), oc);
		    	float t = b*b - c;
    			if( t >= 0) t = -b - sqrt(t);
    			if (t<0) return zeros;	
    		}
    	#elif FOG_BOX_VOID_INVERTED
			// early exit if ray does not cross box
			float3 invR = 1.0 / adir;
		    float3 oc   = _FogVoidPosition - _WorldSpaceCameraPos;
			float3 tbot = invR * (oc - 1.0 /_FogVoidData.xyz);
			float3 ttop = invR * (oc + 1.0 /_FogVoidData.xyz);
			float3 tmin = min (ttop, tbot);
			float3 tmax = max (ttop, tbot);
			float2 t0   = max (tmin.xx, tmin.yz);
			float tnear = max (t0.x, t0.y);
			t0          = min (tmax.xx, tmax.yz);
			float tfar  = min (t0.x, t0.y);
			if (tnear > tfar) return zeros;
			_FogVoidData.xz /= _FogScale;
		#endif
		
		// ceiling cut
		float delta = length(adir.xz);
		float2 ndirxz = adir.xz / delta; // normalize(adir.xz);
		delta /= adir.y;
		
		#if FOG_DISTANCE_ON
		float maxh =  _FogDistance.w / delta;
		float h = clamp(_WorldSpaceCameraPos.y + maxh, -_FogHeight, _FogHeight);
		#else
		float h = clamp(_WorldSpaceCameraPos.y, -_FogHeight, _FogHeight);
		#endif
		
		float xh = delta * (_WorldSpaceCameraPos.y - h);
		float2 xz = _WorldSpaceCameraPos.xz - ndirxz * xh;
		float3 fogCeilingCut = float3(xz.x, h, xz.y);
		
		// does fog stars after pixel? If it does, exit now
		float dist  = min(length(adir), _FogDistance.z);
		float distanceToFog = distance(fogCeilingCut, _WorldSpaceCameraPos);
		if (distanceToFog>=dist) return zeros;

		// floor cut
		float hf = 0;
		// edge cases
		if (delta>0 && worldPos.y > -0.5) {
			hf = _FogHeight;
		} else if (delta<0 && worldPos.y < 0.5) {
			hf = - _FogHeight;
		}
		float xf = delta * ( hf - _WorldSpaceCameraPos.y ); 
		
		// apply start distance fall off
		#if FOG_DISTANCE_ON
		float axf = _FogDistance.x - abs(delta * (sign(_WorldSpaceCameraPos.y) * _FogHeight + _WorldSpaceCameraPos.y));
		if (axf>0) {
			voidAlpha *= saturate(1.0 - axf / _FogDistance.y);
			if (voidAlpha <= 0) return zeros;
 		}
 		#endif
 		
		float2 xzb = _WorldSpaceCameraPos.xz - ndirxz * xf;
		float3 fogFloorCut = float3(xzb.x, hf, xzb.y);

		// fog length is...
		float fogLength = distance(fogCeilingCut, fogFloorCut);
		fogLength = min(fogLength, dist - distanceToFog);
		if (fogLength<=0) return zeros;

		// Calc Ray-march params
		float r = 0.1 + max( log(fogLength), 0 ) * _FogStepping.x;		// stepping ratio with atten detail with distance
		r *= _FogDensity;	// prevents lag when density is too low
		r *= saturate (dist * _FogStepping.y);
		r = max(r, 0.01);
		float4 dir = float4( normalize(adir) * r, fogLength / r);       // ray direction & length

		#if FOG_POINT_LIGHT0 || FOG_POINT_LIGHT1 || FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5 
		float3 pldir = dir.xyz;
		_FogPointLightPosition0 = fogCeilingCut - _FogPointLightPosition0;
		#endif
		
		#if FOG_POINT_LIGHT1 || FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
		_FogPointLightPosition1 = fogCeilingCut - _FogPointLightPosition1;
		#endif

		#if FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
		_FogPointLightPosition2 = fogCeilingCut - _FogPointLightPosition2;
		#endif

		#if FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
		_FogPointLightPosition3 = fogCeilingCut - _FogPointLightPosition3;
		#endif

		#if FOG_POINT_LIGHT4 | FOG_POINT_LIGHT5
		_FogPointLightPosition4 = fogCeilingCut - _FogPointLightPosition4;
		#endif

		#if FOG_POINT_LIGHT5
		_FogPointLightPosition5 = fogCeilingCut - _FogPointLightPosition5;
		#endif

		_FogWindDir.xz *= _Time[1];
				
		// Extracted operations from ray-march loop for additional optimizations
		dir.xz  *= _FogScale;
		_FogHeight *= _FogDensity;	// extracted from loop, dragged here.
		dir.y   /= _FogHeight;
		float4 ft4 = float4(fogCeilingCut.xyz, 0); 
		ft4.xz  += _FogWindDir.xz;  // apply wind speed and direction; already defined above if the condition is true
		ft4.xz  *= _FogScale;
		ft4.y   /= _FogHeight;	

		// Dither start to reduce banding on edges
		if (_FogStepping.w) {
			ft4.xyz += dir.xyz * (tex2D(_NoiseTex, uv * 100).aaa * 3.0); // - 1.5);
		}

		#if FOG_VOID_INVERTED || FOG_BOX_VOID_INVERTED
			float2 voidCenter = _FogVoidPosition.xz + _FogWindDir.xz;
			voidCenter  *= _FogScale;
		#endif
	
		// Ray-march
		half4 sum = zeros, fgCol;
		
		for (;dir.w>1;dir.w--, ft4.xyz+=dir.xyz) {
			#if FOG_VOID_INVERTED
				float2 vd = (voidCenter - ft4.xz) * _FogVoidData.x;
				float voidDistance = dot(vd, vd);
				if (voidDistance>1) continue;
				half4 ng = tex2Dlod(_NoiseTex, ft4.xzww);
				ng.a -= abs(ft4.y) + voidDistance * _FogVoidData.w - 0.3;
			#elif FOG_BOX_VOID_INVERTED
				float2 vd = abs(voidCenter - ft4.xz) * _FogVoidData.xz;
				float voidDistance = max(vd.x, vd.y);
				if (voidDistance>1) continue;
				half4 ng = tex2Dlod(_NoiseTex, ft4.xzww);
				ng.a -= abs(ft4.y);
			#else
				half4 ng = tex2Dlod(_NoiseTex, ft4.xzww);
				ng.a -= abs(ft4.y);
			#endif
			if (ng.a > 0) {
				fgCol   = half4(_Color * (1.0-ng.a), ng.a * 0.4);

				#if FOG_POINT_LIGHT0 || FOG_POINT_LIGHT1 || FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
				half pd0 = dot(_FogPointLightPosition0, _FogPointLightPosition0);
				half pi0 = saturate(1.0 - pd0 * _FogPointLightRadius0);
				ng.rgb += _FogPointLightColor0 * pi0;
				#endif
				
				#if FOG_POINT_LIGHT1 || FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
				half pd1 = dot(_FogPointLightPosition1, _FogPointLightPosition1);
				half pi1 = saturate(1.0 - pd1 * _FogPointLightRadius1);
				ng.rgb += _FogPointLightColor1 * pi1;
				#endif
				
				#if FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
				half pd2 = dot(_FogPointLightPosition2, _FogPointLightPosition2);
				half pi2 = saturate(1.0 - pd2 * _FogPointLightRadius2);
				ng.rgb += _FogPointLightColor2 * pi2;
				#endif
				
				#if FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
				half pd3 = dot(_FogPointLightPosition3, _FogPointLightPosition3);
				half pi3 = saturate(1.0 - pd3 * _FogPointLightRadius3);
				ng.rgb += _FogPointLightColor3 * pi3;
				#endif
				
				#if FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5
				half pd4 = dot(_FogPointLightPosition4, _FogPointLightPosition4);
				half pi4 = saturate(1.0 - pd4 * _FogPointLightRadius4);
				ng.rgb += _FogPointLightColor4 * pi4;
				#endif
				
				#if FOG_POINT_LIGHT5
				half pd5 = dot(_FogPointLightPosition5, _FogPointLightPosition5);
				half pi5 = saturate(1.0 - pd5 * _FogPointLightRadius5);
				ng.rgb += _FogPointLightColor5 * pi5;
				#endif
				
				fgCol.rgb *= ng.rgb * fgCol.aaa;
				sum += fgCol * (1.0-sum.a);
				if (sum.a>0.99) break;
			}
			
			#if FOG_POINT_LIGHT0 || FOG_POINT_LIGHT1 || FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5 
			_FogPointLightPosition0 += pldir;
			#endif
			#if FOG_POINT_LIGHT1 || FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5 
			_FogPointLightPosition1 += pldir;
			#endif
			#if FOG_POINT_LIGHT2 || FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5 
			_FogPointLightPosition2 += pldir;
			#endif
			#if FOG_POINT_LIGHT3 || FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5 
			_FogPointLightPosition3 += pldir;
			#endif
			#if FOG_POINT_LIGHT4 || FOG_POINT_LIGHT5 
			_FogPointLightPosition4 += pldir;
			#endif
			#if FOG_POINT_LIGHT5
			_FogPointLightPosition5 += pldir;
			#endif
		}
		
		// adds fog fraction to prevent banding due stepping on low densities - not over skybox
		sum += step(sum.a, 0.99) * fgCol * (1.0-sum.a) * dir.w;
		
		sum *= _FogAlpha;

		#if FOG_VOID_ON	|| FOG_BOX_VOID_ON || FOG_OF_WAR_ON || FOG_DISTANCE_ON
		sum *= voidAlpha;
		#endif
		

		return sum;
	}
	
	#if FOG_SCATTERING_ON
	half4 getShaft(half4 color, float2 uv) {
   		half2 duv = _SunPosition.xy - uv;
  		duv *= _FogScatteringData.x;  
  		half illumination = _FogScatteringData2.x;
   		for (float i = _FogScatteringData.y; i > 0; i--) {
    		uv += duv;  
   			half4 sample = tex2Dlod(_MainTex, float4(uv.xy,0,0));  
    		color += sample * illumination * _FogScatteringData.w;
    		illumination *= _FogScatteringData2.y;
  		}  
   		return color * _FogScatteringData.z;
	}
	#endif

	// Fragment Shaders
	half4 fragBackFog (v2f i) : COLOR {
		half4 color = tex2D(_MainTex, i.uv);
		float depth01 = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.depthUV)));
		float3 worldPos = getWorldPos(i, depth01);
		#if FOG_HAZE_ON
		if (depth01>=_FogSkyData.w) {		
			#if FOG_HAZE_ON
			color = getSkyColor(color, worldPos, i.uv);
			#endif
		}
		#endif
		#if FOG_SCATTERING_ON
		half4 shaft = getShaft(color, i.uv); 
		color += shaft;
		#endif
		half4 sum = getFogColor(i.uv, worldPos, depth01);
		color = color*(1.0-sum.a) + sum;
		return color;
	}

	half4 fragOverlayFog (v2f i) : COLOR {
	    float depthTex = DecodeFloatRG (tex2D(_DepthTexture, i.depthUV).zw);
	    clip(depthTex - 0.00001);
    	float depth01 = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.depthUV)));
    	clip(depth01 - depthTex);
  		half4 color = tex2D(_MainTex, i.uv);
		float3 worldPos = getWorldPos(i, depthTex);
		half4 sum = getFogColor(i.uv, worldPos, depthTex);
		return color*(1.0-sum.a) + sum;
	}
		
	half4 fragGetFog (v2f i) : COLOR {
    	float depth01  = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.depthUV)));
		float3 worldPos = getWorldPos(i, depth01);
		return getFogColor(i.uv, worldPos, depth01);
	}

	half4 fragApplyFog (v2f i) : COLOR {
  		half4 color = tex2D(_MainTex, i.uv);
    	float depthFull = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.depthUV));
    	float depthLinear01 = Linear01Depth(depthFull);
		#if FOG_HAZE_ON
  		if (depthLinear01>=_FogSkyData.w) {		
			float3 worldPos = getWorldPos(i, depthLinear01);
			color = getSkyColor(color, worldPos, i.uv);
		}
		#endif
		#if FOG_SCATTERING_ON
		half4 shaft = getShaft(color, i.uv);
		color += shaft;
		#endif
		
		half4 sum;
		float2 minUV = i.depthUV;
		
  		if (_FogStepping.z>0) {
			float2 uv00 = i.depthUV - 0.5 * _FogDownsampled_TexelSize.xy;
    		float depth00 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, uv00));
			float2 uv10 = uv00 + float2(_FogDownsampled_TexelSize.x, 0);
    		float depth10 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, uv10));
			float2 uv01 = uv00 + float2(0, _FogDownsampled_TexelSize.y);
    		float depth01 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, uv01));
			float2 uv11 = uv00 + _FogDownsampled_TexelSize.xy;
    		float depth11 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, uv11));
	  		float diff00 = abs(depthFull - depth00);
  			float diff10 = abs(depthFull - depth10);
  			float diff01 = abs(depthFull - depth01);
	  		float diff11 = abs(depthFull - depth11);
	  		
			if (diff00 > _FogStepping.z || diff10 > _FogStepping.z || diff01 > _FogStepping.z || diff11 > _FogStepping.z) {
	  			// Check 10 vs 00
	  			float minDiff  = lerp(diff00, diff10, diff10 < diff00);
	  			minUV    = lerp(uv00, uv10, diff10 < diff00);
	  			// Check against 01
	  			minUV    = lerp(minUV, uv01, diff01 < minDiff);
	  			minDiff  = lerp(minDiff, diff01, diff01 < minDiff);
	  			// Check against 11
	  			minUV    = lerp(minUV, uv11, diff11 < minDiff);
			}
		}
		sum = tex2Dlod(_FogDownsampled, float4(minUV, 0, 0));
		
		return color*(1.0-sum.a) + sum;
	}
	ENDCG
	
	SubShader {
       	ZTest Always Cull Off ZWrite Off
       	Fog { Mode Off }
		Pass {
	        CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragBackFog
			ENDCG
        }
		Pass {
	        CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragOverlayFog
			ENDCG
        }        
		Pass {
	        CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragGetFog
			ENDCG
        }        
		Pass {
	        CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragApplyFog
			ENDCG
        }        
	}
	FallBack Off
}	