﻿using UnityEngine;
using UnityEditor;
using System.Collections;

namespace VolumetricFogAndMist {
	[CustomEditor(typeof(VolumetricFog))]
	public class VolumetricFogInspector : Editor {

		VolumetricFog _fog;
		Texture2D _blackTexture;
		GUIStyle blackStyle, sectionHeaderStyle;
		Color titleColor;
		bool expandFogGeometrySection, expandFogStyleSection, expandSkySection, expandSunShaftsSection, expandFogOfWarSection, expandFogVoidSection, expandOptionalPointLightSection, expandOptimizationSettingsSection;
		const string SCATTERING_ON = "Light Scattering (ON)";
		const string SCATTERING_OFF = "Light Scattering";
		const string FOW_ON = "Fog Of War (ON)";
		const string FOW_OFF = "Fog Of War";
		const string FOG_VOID_ON = "Fog Void (ON)";
		const string FOG_VOID_OFF = "Fog Void";
		const string FOG_SKY_HAZE_ON = "Sky Haze (ON)";
		const string FOG_SKY_HAZE_OFF = "Sky Haze";


		void OnEnable () {
			Color backColor = EditorGUIUtility.isProSkin ? new Color (0.18f, 0.18f, 0.18f) : new Color (0.825f, 0.825f, 0.825f);
			titleColor = EditorGUIUtility.isProSkin ? new Color (0.52f, 0.66f, 0.9f) : new Color(0.12f, 0.16f, 0.4f);
			_blackTexture = MakeTex (4, 4, backColor);
			_blackTexture.hideFlags = HideFlags.DontSave;
			blackStyle = new GUIStyle ();
			blackStyle.normal.background = _blackTexture;
			_fog = (VolumetricFog)target;

			expandFogGeometrySection = EditorPrefs.GetBool("expandFogGeometrySection", false);
			expandFogStyleSection = EditorPrefs.GetBool("expandFogStyleSection", false);
			expandSkySection = EditorPrefs.GetBool("expandSkySection", false);
			expandOptionalPointLightSection = EditorPrefs.GetBool("expandOptionalPointLightSection", false);
			expandFogOfWarSection = EditorPrefs.GetBool("expandFogOfWarSection", false);
			expandFogVoidSection = EditorPrefs.GetBool("expandFogVoidSection", false);
			expandOptimizationSettingsSection = EditorPrefs.GetBool("expandOptimizationSettingsSection", false);
			expandSunShaftsSection = EditorPrefs.GetBool("expandSunShaftsSection", false);
		}

		void OnDestroy() {
			// Save folding sections state
			EditorPrefs.SetBool("expandFogGeometrySection", expandFogGeometrySection);
			EditorPrefs.SetBool("expandFogStyleSection", expandFogStyleSection);
			EditorPrefs.SetBool("expandSkySection", expandSkySection);
			EditorPrefs.SetBool("expandOptionalPointLightSection", expandOptionalPointLightSection);
			EditorPrefs.SetBool("expandFogOfWarSection", expandFogOfWarSection);
			EditorPrefs.SetBool("expandFogVoidSection", expandFogVoidSection);
			EditorPrefs.SetBool("expandOptimizationSettingsSection", expandOptimizationSettingsSection);
			EditorPrefs.SetBool("expandSunShaftsSection", expandSunShaftsSection);
		}

		public override void OnInspectorGUI () {
			if (_fog == null)
				return;
			_fog.isDirty = false;

			EditorGUILayout.Separator ();
			EditorGUILayout.BeginVertical (blackStyle);
			
			EditorGUILayout.BeginHorizontal ();
			DrawTitleLabel("General Settings");
			if (GUILayout.Button("Help", GUILayout.Width(50))) EditorUtility.DisplayDialog("Help", "Move the mouse over each label to show a description of the parameter.\nThese parameters allow you to customize the fog effect to achieve the effect and performance desired.\n\nPlease rate Volumetric Fog & Mist on the Asset Store! Thanks.", "Ok");
			if (GUILayout.Button("About", GUILayout.Width(60))) {
				VolumetricFogAbout.ShowAboutWindow();
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Preset", "List of preconfigured and ready to use fog styles. You can customize anyone of the list and choose 'Custom' to persist the changes when you save the scene."), GUILayout.Width(130));
			_fog.preset = (FOG_PRESET)EditorGUILayout.EnumPopup(_fog.preset);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Enable Fog Volumes", "Allow the use of fog volumes, which will change fog and sky haze alpha automatically when camera enters/exits volumes.\nTo create a Fog Volume in your scene, select the option from the main menu 'GameObject/Create Other/Volumetric Fog Volume'."), GUILayout.Width(130));
			_fog.useFogVolumes = EditorGUILayout.Toggle(_fog.useFogVolumes);
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Render Before Transp.", "Applies image effect before transparent objects are rendered. For example, you may disable this option to render the fog on top of transparent objects, like water. But if you use particles, you should enable it to avoid particles being overdrawn by the fog if this is too thick. This option is not compatible with 'Improve transparency'."), GUILayout.Width(130));
			bool prev = _fog.renderOpaque;
			_fog.renderOpaque = EditorGUILayout.Toggle(_fog.renderOpaque, GUILayout.Width(40));
			if (_fog.renderOpaque!=prev) GUIUtility.ExitGUI();
			GUILayout.Label(new GUIContent("Improve Transparency", "Add an additional render pass only over transparent objects to allow fog to be seen before them. If set to false (which is the default behaviour), fog will always be rendered behind transparent objects."));
			prev = _fog.improveTransparency;
			_fog.improveTransparency = EditorGUILayout.Toggle(_fog.improveTransparency, GUILayout.Width(40));
			if (_fog.improveTransparency!=prev) GUIUtility.ExitGUI();
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label (new GUIContent("Sun", "Assign a Game Object (usually a Directional Light that acts as the Sun) to automatically synchronize the light direction parameter and make the fog highlight be aligned with the Sun."), GUILayout.Width (130));
			_fog.sun = (GameObject)EditorGUILayout.ObjectField (_fog.sun, typeof(GameObject), true );
			if (_fog.sun!=null && GUILayout.Button(new GUIContent("Unassign", "Removes link with current directional light (Sun) to allow custom light color, direction and direction settings."))) {
				_fog.sun = null;
				GUIUtility.ExitGUI();
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Separator ();

			EditorGUILayout.EndVertical ();
			EditorGUILayout.Separator ();

			// Inspector sections start
			if (sectionHeaderStyle==null) {
				sectionHeaderStyle = new GUIStyle(EditorStyles.foldout);
			}
			sectionHeaderStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color (0.52f, 0.66f, 0.9f) : new Color(0.12f, 0.16f, 0.4f);
			sectionHeaderStyle.margin = new RectOffset(12,0,0,0);
			sectionHeaderStyle.fontStyle = FontStyle.Bold;

			EditorGUILayout.BeginVertical (blackStyle);

			EditorGUILayout.BeginHorizontal ();
			expandFogGeometrySection = EditorGUILayout.Foldout(expandFogGeometrySection, "Fog Geometry", sectionHeaderStyle);
			EditorGUILayout.EndHorizontal ();

			if (expandFogGeometrySection) {
				EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Density", "General density of the fog. Higher density fog means darker fog as well."), GUILayout.Width(120));
			_fog.density = EditorGUILayout.Slider(_fog.density, 0f, 1f);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Noise Strength", "Randomness of the fog formation. 0 means uniform fog whereas a value towards 1 will make areas of different densities and heights."), GUILayout.Width(120));
			_fog.noiseStrength = EditorGUILayout.Slider(_fog.noiseStrength, 0f, 1f);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Scale", "Increasing this value will expand the size of the noise."), GUILayout.Width(120));
			_fog.noiseScale = EditorGUILayout.Slider(_fog.noiseScale, 0.2f, 10f);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Distance", "Distance in meters from the camera at which the fog starts. It works with Distance FallOff."), GUILayout.Width(120));
			_fog.distance = EditorGUILayout.Slider(_fog.distance, 0f, 1000f);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Distance FallOff", "When you set a value to Distance > 0, this parameter defines the gradient of the fog to the camera. A value of 0 means no gradient and the fog will start abruptly at the specified distance."), GUILayout.Width(120));
			_fog.distanceFallOff = EditorGUILayout.Slider(_fog.distanceFallOff, 0f, 1f);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Max. Distance", "Maximum distance from camera at which the fog is rendered. Decrease this value to improve performance."), GUILayout.Width(120));
			_fog.maxFogLength = EditorGUILayout.Slider(_fog.maxFogLength, 0f, 2000f);
			EditorGUILayout.EndHorizontal ();


			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Height", "Maximum height of the fog in meters."), GUILayout.Width(120));
			_fog.height = EditorGUILayout.Slider(_fog.height, 0f, 500f);
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Base Height", "Starting height of the fog in meters. You can set this value above Camera position. Try it!"), GUILayout.Width(120));
			_fog.baselineHeight = EditorGUILayout.Slider(_fog.baselineHeight, -500, 1000); // GUILayout.Width(80));
			if (GUILayout.Button(new GUIContent("Reset", "Automatically sets base height to water level or to zero height if no water found in the scene."))) {
				_fog.CheckWaterLevel(true);
				_fog.preset = FOG_PRESET.Custom;
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Relative To Camera", "If set to true, the base height will be added to the camera height. This is useful for cloud styles so they always stay over your head!"), GUILayout.Width(120));
			_fog.baselineRelativeToCamera = EditorGUILayout.Toggle(_fog.baselineRelativeToCamera);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Separator ();
			}

			EditorGUILayout.EndVertical ();
			EditorGUILayout.Separator ();
			EditorGUILayout.BeginVertical (blackStyle);

			EditorGUILayout.BeginHorizontal ();
			expandFogStyleSection = EditorGUILayout.Foldout(expandFogStyleSection, "Fog Style", sectionHeaderStyle);
			EditorGUILayout.EndHorizontal ();
			
			if (expandFogStyleSection) {

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Alpha", "Transparency for the fog. You may want to reduce this value if you experiment issues with billboards."),  GUILayout.Width(120));
			_fog.alpha = EditorGUILayout.Slider(_fog.alpha, 0, 1);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Albedo", "Base color of the fog."), GUILayout.Width(120));
			_fog.color = EditorGUILayout.ColorField(_fog.color);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Specular Color", "This is the color reflected by the fog under direct light exposure (see Light parameters)"), GUILayout.Width(120));
			_fog.specularColor = EditorGUILayout.ColorField(_fog.specularColor);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Specular Threshold", "Area of the fog subject to light reflectancy"), GUILayout.Width(120));
			_fog.specularThreshold = EditorGUILayout.Slider(_fog.specularThreshold, 0, 1);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Specular Intensity", "The intensity of the reflected light."), GUILayout.Width(120));
			_fog.specularIntensity = EditorGUILayout.Slider(_fog.specularIntensity, 0, 1);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Light Direction", "The normalized direction of a simulated directional light."), GUILayout.Width(120));
			_fog.lightDirection = EditorGUILayout.Vector3Field("", _fog.lightDirection);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Light Intensity", "Intensity of the simulated directional light."), GUILayout.Width(120));
			_fog.lightIntensity = EditorGUILayout.Slider(_fog.lightIntensity, -1, 1);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Light Color", "Color of the simulated direcional light."), GUILayout.Width(120));
			_fog.lightColor = EditorGUILayout.ColorField(_fog.lightColor);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Wind Speed", "Speed factor for the simulated wind effect over the fog."), GUILayout.Width(120));
			_fog.speed = EditorGUILayout.Slider(_fog.speed, 0, 1);
			if (GUILayout.Button("Stop")) {
					_fog.speed = 0;
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Wind Direction", "Normalized direcional vector for the wind effect."), GUILayout.Width(120));
			_fog.windDirection = EditorGUILayout.Vector3Field("", _fog.windDirection);
			EditorGUILayout.EndHorizontal ();
			}

			EditorGUILayout.EndVertical ();
			EditorGUILayout.Separator ();
			EditorGUILayout.BeginVertical (blackStyle);
			
			EditorGUILayout.BeginHorizontal ();
			expandSkySection = EditorGUILayout.Foldout(expandSkySection, (_fog.skyHaze>0 && _fog.skyAlpha>0) ? FOG_SKY_HAZE_ON: FOG_SKY_HAZE_OFF, sectionHeaderStyle);
			EditorGUILayout.EndHorizontal ();
			
			if (expandSkySection) {
				
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Haze", "Height of the sky haze in meters. Reduce this or alpha to 0 to disable sky haze."), GUILayout.Width(120));
				_fog.skyHaze = EditorGUILayout.Slider(_fog.skyHaze, 0, 1000);
				EditorGUILayout.EndHorizontal ();
				
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Color", "Sky haze color."), GUILayout.Width(120));
				_fog.skyColor = EditorGUILayout.ColorField(_fog.skyColor);
				EditorGUILayout.EndHorizontal ();
				
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Speed", "Speed of the haze animation."), GUILayout.Width(120));
				_fog.skySpeed = EditorGUILayout.Slider(_fog.skySpeed, 0, 1);
				EditorGUILayout.EndHorizontal ();
				
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Noise Strength", "Amount of noise for the sky haze."), GUILayout.Width(120));
				_fog.skyNoiseStrength = EditorGUILayout.Slider(_fog.skyNoiseStrength, 0, 1);
				EditorGUILayout.EndHorizontal ();
				
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Alpha", "Transparency of the sky haze. Reduce this or Haze above to 0 to disable sky haze. Important: if you experiment issues with tree billboards you need to lower this value."), GUILayout.Width(120));
				_fog.skyAlpha = EditorGUILayout.Slider(_fog.skyAlpha, 0, 1);
				EditorGUILayout.EndHorizontal ();
				
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Depth", "Any pixel beyond this depth is assumed to be part of the skybox. For standard skybox, leave this value at 0.999. If you're using Time of Day or other skybox dome, you need to reduce this value so it fits inside the sky dome."), GUILayout.Width(120));
				_fog.skyDepth = EditorGUILayout.Slider(_fog.skyDepth, 0, 0.999f);
				EditorGUILayout.EndHorizontal ();

			}
			
			EditorGUILayout.EndVertical ();
			EditorGUILayout.Separator ();

			EditorGUILayout.BeginVertical (blackStyle);
			
			EditorGUILayout.BeginHorizontal ();
			expandSunShaftsSection = EditorGUILayout.Foldout(expandSunShaftsSection, _fog.lightScatteringEnabled ? SCATTERING_ON : SCATTERING_OFF, sectionHeaderStyle);
			EditorGUILayout.EndHorizontal ();
			
			if (expandSunShaftsSection) {

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Enable", "Enables screen space light scattering. Simulates scattering of Sun light through atmosphere.."), GUILayout.Width(120));
				_fog.lightScatteringEnabled = EditorGUILayout.Toggle(_fog.lightScatteringEnabled);
				EditorGUILayout.EndHorizontal ();

				if (_fog.lightScatteringEnabled && _fog.sun==null) {
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Box("Warning: Light Scattering requires a Sun reference. Assign a game object (representing Sun or a light) in General Settings section.");
					EditorGUILayout.EndHorizontal ();
				}

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Spread", "Length of the Sun rays. "), GUILayout.Width(120));
				_fog.lightScatteringSpread = EditorGUILayout.Slider(_fog.lightScatteringSpread, 0, 1);
				EditorGUILayout.EndHorizontal ();
				
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Sample Weight", "Strength of Sun rays."), GUILayout.Width(120));
				_fog.lightScatteringWeight = EditorGUILayout.Slider(_fog.lightScatteringWeight, 0, 50f);
				EditorGUILayout.EndHorizontal ();
				
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Start Illumination", "Initial strength of each ray."), GUILayout.Width(120));
				_fog.lightScatteringIllumination = EditorGUILayout.Slider(_fog.lightScatteringIllumination, 0, 50f);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Decay", "Decay multiplier applied on each step."), GUILayout.Width(120));
				_fog.lightScatteringDecay = EditorGUILayout.Slider(_fog.lightScatteringDecay, 0.9f, 1.1f);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Exposure", "Final exposure adjustement for light scattering."), GUILayout.Width(120));
				_fog.lightScatteringExposure = EditorGUILayout.Slider(_fog.lightScatteringExposure, 0, 1f);
				EditorGUILayout.EndHorizontal ();
			}
			
			EditorGUILayout.EndVertical ();
			EditorGUILayout.Separator ();
			EditorGUILayout.BeginVertical (blackStyle);

			EditorGUILayout.BeginHorizontal ();
			expandOptionalPointLightSection = EditorGUILayout.Foldout(expandOptionalPointLightSection, "Point Lights", sectionHeaderStyle);
			EditorGUILayout.EndHorizontal ();

			if (expandOptionalPointLightSection) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Track Point Lights", "Check this option to automatically select the nearest point lights."), GUILayout.Width(120));
				_fog.pointLightTrackAuto = EditorGUILayout.Toggle(_fog.pointLightTrackAuto);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("  Max Point Lights", "Specify the maximum number of point lights that will be tracked. Nearest point light will be assigned to slot 1, next one to slot 2, and so on, up to the number specified here. This option is very useful for choosing the proper lights as the camera moves through the scene."), GUILayout.Width(120));
				_fog.pointLightTrackingCount = EditorGUILayout.IntSlider(_fog.pointLightTrackingCount, 0, 6);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Separator ();

				for (int k=0;k<VolumetricFog.MAX_POINT_LIGHTS;k++) {
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label (new GUIContent("Point Light " + (k+1).ToString(), "Assign a Point Light Game Object to automatically synchronize the position, radius and color parameters below."), GUILayout.Width (120));
					GameObject pointLight = (GameObject)EditorGUILayout.ObjectField (_fog.GetPointLight(k), typeof(GameObject), true );
					_fog.SetPointLight(k, pointLight);
					if (pointLight!=null && GUILayout.Button(new GUIContent("Unassign", "Removes link with point light to allow customize light color, direction and direction settings."))) {
						_fog.SetPointLight(k, null);
						_fog.SetPointLightRange(k, 0);
						GUIUtility.ExitGUI();
					}
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label(new GUIContent("  Range", "Range of the light in meters. Set this to zero to disable the point light effect."), GUILayout.Width(120));
					_fog.SetPointLightRange(k, EditorGUILayout.Slider(_fog.GetPointLightRange(k), 0, 1000));
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label(new GUIContent("  Intensity", "Intensity of the light. This would be the intensity of the point light."), GUILayout.Width(120));
					_fog.SetPointLightIntensity(k, EditorGUILayout.Slider(_fog.GetPointLightIntensity(k), 0, 10));
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label(new GUIContent("  Intensity Multiplier", "Optional additional multiplier for the intensity. This parameters can be used to reduce the intensity of the light in the fog without changing the intensity of the point light."), GUILayout.Width(120));
					_fog.SetPointLightIntensityMultiplier(k, EditorGUILayout.Slider(_fog.GetPointLightIntensityMultiplier(k), 0, 2));
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label(new GUIContent("  Position", "Location of the source of light in world space coordinates."), GUILayout.Width(120));
					_fog.SetPointLightPosition(k, EditorGUILayout.Vector3Field("", _fog.GetPointLightPosition(k)));
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label(new GUIContent("  Color", "Point light color."), GUILayout.Width(120));
					_fog.SetPointLightColor(k, EditorGUILayout.ColorField(_fog.GetPointLightColor(k)));
					EditorGUILayout.EndHorizontal ();
				}
			}

			EditorGUILayout.EndVertical ();
			EditorGUILayout.Separator ();

			EditorGUILayout.BeginVertical (blackStyle);

			
			EditorGUILayout.BeginHorizontal ();
			expandFogOfWarSection = EditorGUILayout.Foldout(expandFogOfWarSection,_fog.fogOfWarEnabled ? FOW_ON: FOW_OFF, sectionHeaderStyle);
			EditorGUILayout.EndHorizontal ();
			
			if (expandFogOfWarSection) {
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Enable", "Enables fog of war feature. When enabled, you can call SetFogOfWarAlpha(worldPosition, alpha) to define the transparency of the fog at certain positions in world space."), GUILayout.Width(120));
			_fog.fogOfWarEnabled = EditorGUILayout.Toggle(_fog.fogOfWarEnabled);
			if (GUILayout.Button("Reset")) _fog.ResetFogOfWar();
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Center", "Location of the center of the fog war area in world space. Only X and Z coordinates are considered."), GUILayout.Width(120));
			_fog.fogOfWarCenter = EditorGUILayout.Vector3Field("", _fog.fogOfWarCenter);
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Size", "Size of the fog of war. This is the bounds in world units of the fog of war (only X and Z coordinates are considered). Outside of these bounds, fog will appear as normal (so you can only set different alpha values inside these bounds)."), GUILayout.Width(120));
			_fog.fogOfWarSize= EditorGUILayout.Vector3Field("", _fog.fogOfWarSize);
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Texture Size", "A square texture is used to hold the transparency bits mapped to each world position. This parameter defines the size of the texture. The greater the size the more granularity for the fog of war effect. The optimal value depends on the size parameter. For instance, a fog of war size of 1024 units with a texture size of 1024 will result in one pixel of texture for each meter in world space."), GUILayout.Width(120));
			_fog.fogOfWarTextureSize= EditorGUILayout.IntSlider(_fog.fogOfWarTextureSize, 32, 2048);
			EditorGUILayout.EndHorizontal ();

				
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Restore Delay", "Delay after the fog has been cleared to restore the fog to original alpha. You clear or set the alpha of the fog at any position calling FogOfWarSetAlpha() method. Set this value to 0 to leave the cleared fog unrestored."), GUILayout.Width(120));
				_fog.fogOfWarRestoreDelay= EditorGUILayout.Slider(_fog.fogOfWarRestoreDelay, 0, 100);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Restore Duration", "Once the fog has started to be restored at any single location, the time in seconds for the fade in effect."), GUILayout.Width(120));
				_fog.fogOfWarRestoreDuration= EditorGUILayout.Slider(_fog.fogOfWarRestoreDuration, 0, 25);
				EditorGUILayout.EndHorizontal ();

			}

			EditorGUILayout.EndVertical ();
			EditorGUILayout.Separator ();
			EditorGUILayout.BeginVertical (blackStyle);
			
				EditorGUILayout.BeginHorizontal ();
			expandFogVoidSection = EditorGUILayout.Foldout(expandFogVoidSection, _fog.fogVoidRadius>0 ? FOG_VOID_ON: FOG_VOID_OFF, sectionHeaderStyle);
				EditorGUILayout.EndHorizontal ();
				
				if (expandFogVoidSection) {

			EditorGUILayout.BeginHorizontal ();
			DrawTitleLabel("Fog Void");
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Center", "Location of the center of the fog void in world space (area where the fog disappear).\nThis option is very useful if you want a clear area around your character in 3rd person view."), GUILayout.Width(120));
			_fog.fogVoidPosition = EditorGUILayout.Vector3Field("", _fog.fogVoidPosition);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label (new GUIContent("Character To Follow", "Assign a Game Object to follow its position automatically (usually the player character which will be at the center of the void area)."), GUILayout.Width (130));
			_fog.character = (GameObject)EditorGUILayout.ObjectField (_fog.character, typeof(GameObject), true );
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Inverted", "Check this option to make fog only appear inside the void area defined by the position and radius. Area fog can be spherical (radius is used) or boxed (radius and depth are used, falloff is ignored and box height is controlled by main baseline and height parameters)."), GUILayout.Width(120));
			_fog.fogVoidInverted= EditorGUILayout.Toggle(_fog.fogVoidInverted);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Radius", "Radius of the void area. If height and depth params are zero, void area will adopt a spherical shape."), GUILayout.Width(120));
			_fog.fogVoidRadius= EditorGUILayout.Slider(_fog.fogVoidRadius, 0f, 2000f);
			EditorGUILayout.EndHorizontal ();
			
			if (!_fog.fogVoidInverted) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Height", "Height of the void area."), GUILayout.Width(120));
				_fog.fogVoidHeight= EditorGUILayout.Slider(_fog.fogVoidHeight, 0f, 2000f);
				EditorGUILayout.EndHorizontal ();
			}
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Depth", "Depth of the void area."), GUILayout.Width(120));
			_fog.fogVoidDepth = EditorGUILayout.Slider(_fog.fogVoidDepth, 0f, 2000f);
			EditorGUILayout.EndHorizontal ();

			if (!_fog.fogVoidInverted || _fog.fogVoidDepth <= 0) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("FallOff", "Gradient of the void area effect."), GUILayout.Width(120));
				_fog.fogVoidFallOff= EditorGUILayout.Slider(_fog.fogVoidFallOff, 0f, 10f);
				EditorGUILayout.EndHorizontal ();
				}
			}

			EditorGUILayout.EndVertical ();
			EditorGUILayout.Separator ();
			EditorGUILayout.BeginVertical (blackStyle);

			
			EditorGUILayout.BeginHorizontal ();
			expandOptimizationSettingsSection = EditorGUILayout.Foldout(expandOptimizationSettingsSection, "Optimization Settings", sectionHeaderStyle);
			EditorGUILayout.EndHorizontal ();
			
			if (expandOptimizationSettingsSection) {
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Downsampling", "Reduces the size of the depth texture to improve performance."), GUILayout.Width(120));
			_fog.downsampling = EditorGUILayout.IntSlider(_fog.downsampling, 1, 4);
			EditorGUILayout.EndHorizontal ();
			
			if (_fog.downsampling>1) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label(new GUIContent("Improve Edges", "Check this option to reduce artifacts and halos around geometry edges when downsampling is applied. This is an option because it's faster to not take care or geometry edges, which is probably unnecesary if you use fog as elevated clouds."), GUILayout.Width(120));
				_fog.edgeImprove= EditorGUILayout.Toggle(_fog.edgeImprove, GUILayout.Width(20));
				EditorGUILayout.EndHorizontal ();
				if (_fog.edgeImprove) {
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label(new GUIContent("   Threshold", "Depth threshold used to detected edges."),  GUILayout.Width(120));
					_fog.edgeThreshold = EditorGUILayout.Slider(_fog.edgeThreshold, 0.00001f, 0.005f);
					EditorGUILayout.EndHorizontal ();
					}
			}

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Stepping", "Multiplier to the ray-marching algorithm. Values between 8-12 are good. Increasing the stepping will produce more accurate and better quality fog but performance will be reduced. The less the density of the fog the lower you can set this value."),  GUILayout.Width(120));
			_fog.stepping = EditorGUILayout.Slider(_fog.stepping, 1f, 20f);
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Stepping Near", "Works with Stepping parameter but applies only to short distances from camera. Lowering this value can help to reduce banding effect (performance can be reduced as well)."), GUILayout.Width(120));
			_fog.steppingNear = EditorGUILayout.Slider(_fog.steppingNear, 0f, 50f);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Dithering", "Check this option to reduce banding."), GUILayout.Width(120));
				_fog.dithering= EditorGUILayout.Toggle(_fog.dithering, GUILayout.Width(20));
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(new GUIContent("Scattering Samples", "Number of light samples used when Light Scattering is enabled. Reduce to increse performance."), GUILayout.Width(120));
			_fog.lightScatteringSamples = EditorGUILayout.IntSlider(_fog.lightScatteringSamples, 8, 64);
			EditorGUILayout.EndHorizontal ();

			}
			
			EditorGUILayout.EndVertical ();
			EditorGUILayout.Separator ();

			if (_fog.isDirty) {
				EditorUtility.SetDirty (target);
			}


		}

		Texture2D MakeTex (int width, int height, Color col)
		{
			Color[] pix = new Color[width * height];
			
			for (int i = 0; i < pix.Length; i++)
				pix [i] = col;
			
			Texture2D result = new Texture2D (width, height);
			result.SetPixels (pix);
			result.Apply ();
			
			return result;
		}

		GUIStyle titleLabelStyle;
		void DrawTitleLabel (string s) {
			if (titleLabelStyle==null) {
				titleLabelStyle = new GUIStyle (GUI.skin.label);
			}
			titleLabelStyle.normal.textColor = titleColor;
			titleLabelStyle.fontStyle = FontStyle.Bold;
			GUILayout.Label (s, titleLabelStyle);
		}


	}

}
