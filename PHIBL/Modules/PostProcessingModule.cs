using UnityEngine;
using IllusionPlugin;
using System;
using PHIBL.PostProcessing;
using static PHIBL.UIUtils;

namespace PHIBL
{
    partial class PHIBL : MonoBehaviour
    {
        void TonemappingModule()
        {
            GUILayout.Label(GUIStrings.Tonemapping, titlestyle2);
            GUILayout.Space(space);
            var SelectedToneMapper = (int)PPCtrl.colorGrading.tonemapping.tonemapper;
            SelectedToneMapper = GUILayout.SelectionGrid(SelectedToneMapper, new GUIContent[]
            {
                GUIStrings.Tonemapping_none,
                GUIStrings.Tonemapping_ACES,
                GUIStrings.Tonemapping_neutral
            }, 3, selectstyle);
            PPCtrl.colorGrading.tonemapping.tonemapper = (ColorGradingModel.Tonemapper)SelectedToneMapper;
            GUILayout.Space(space);
            if (SelectedToneMapper == 2)
            {
                SliderGUI(ref PPCtrl.colorGrading.tonemapping.neutralBlackIn, -0.1f, 0.1f, GUIStrings.Tonemapping_neutralBlackIn);
                SliderGUI(ref PPCtrl.colorGrading.tonemapping.neutralWhiteIn, 1f, 20f, GUIStrings.Tonemapping_neutralWhiteIn);
                SliderGUI(ref PPCtrl.colorGrading.tonemapping.neutralBlackOut, -0.09f, 0.1f, GUIStrings.Tonemapping_neutralBlackOut);
                SliderGUI(ref PPCtrl.colorGrading.tonemapping.neutralWhiteOut, 1f, 19f, GUIStrings.Tonemapping_neutralWhiteOut);
                SliderGUI(ref PPCtrl.colorGrading.tonemapping.neutralWhiteLevel, 0.1f, 20f, GUIStrings.Tonemapping_neutralWhiteLevel);
                SliderGUI(ref PPCtrl.colorGrading.tonemapping.neutralWhiteClip, 1f, 10f, GUIStrings.Tonemapping_neutralWhiteClip);
            }
            SliderGUI(PPCtrl.colorGrading.basic.postExposure, min: -5f, max: 5f, reset: 0f, guiContent: GUIStrings.Exposure_Value, 
                SetOutput: x => { PPCtrl.colorGrading.basic.postExposure = x; ModPrefs.SetFloat("PHIBL","EV", x); });
            GUILayout.Space(space);
        }

        void EyeAdaptationModule()
        {
            ToggleGUITitle(ref PPCtrl.enableEyeAdaptation, GUIStrings.Eye_Adaptation);
            if (PPCtrl.enableEyeAdaptation)
            {
                GUILayout.Space(space);
                ToggleGUI(ref PPCtrl.eyeAdaptation.dynamicKeyValue, GUIStrings.DynamicKeyValue);
                if (!PPCtrl.eyeAdaptation.dynamicKeyValue)
                {
                    SliderGUI(ref PPCtrl.eyeAdaptation.keyValue, 0f, 2f, 0.25f, GUIStrings.KeyValue);
                }
                GUILayout.Label(new GUIContent(GUIStrings.Histogram_filter + PPCtrl.eyeAdaptation.lowPercent.ToString("N1") + " - " + PPCtrl.eyeAdaptation.highPercent.ToString("N1"), GUIStrings.Histogram_filter_tooltips), labelstyle);
                PPCtrl.eyeAdaptation.lowPercent = GUILayout.HorizontalSlider(PPCtrl.eyeAdaptation.lowPercent, 1f, 99f, sliderstyle, thumbstyle);
                GUILayout.Space(space);
                if (PPCtrl.eyeAdaptation.highPercent < PPCtrl.eyeAdaptation.lowPercent)
                {
                    PPCtrl.eyeAdaptation.highPercent = PPCtrl.eyeAdaptation.lowPercent;
                }
                PPCtrl.eyeAdaptation.highPercent = GUILayout.HorizontalSlider(PPCtrl.eyeAdaptation.highPercent, 1f, 99f, sliderstyle, thumbstyle);
                //GUILayout.Space(UIUtils.space);
                if (PPCtrl.eyeAdaptation.highPercent < PPCtrl.eyeAdaptation.lowPercent)
                {
                    PPCtrl.eyeAdaptation.lowPercent = PPCtrl.eyeAdaptation.highPercent;
                }

                GUILayout.Label(new GUIContent(GUIStrings.Histogram_bound + PPCtrl.eyeAdaptation.logMin.ToString() + " - " + PPCtrl.eyeAdaptation.logMax.ToString(), GUIStrings.Histogram_bound_tooltips), labelstyle);
                PPCtrl.eyeAdaptation.logMin = (int)GUILayout.HorizontalSlider(PPCtrl.eyeAdaptation.logMin, -16f, -1f, sliderstyle, thumbstyle);
                GUILayout.Space(space);
                PPCtrl.eyeAdaptation.logMax = (int)GUILayout.HorizontalSlider(PPCtrl.eyeAdaptation.logMax, 1f, 16f, sliderstyle, thumbstyle);
                //GUILayout.Space(UIUtils.space);
                GUILayout.Label(new GUIContent(GUIStrings.Lumination_range + PPCtrl.eyeAdaptation.minLuminance.ToString("N3") + " - " + PPCtrl.eyeAdaptation.maxLuminance.ToString("N3"), GUIStrings.Lumination_range_tooltips), labelstyle);
                PPCtrl.eyeAdaptation.minLuminance = GUILayout.HorizontalSlider(PPCtrl.eyeAdaptation.minLuminance, -10f, 10f, sliderstyle, thumbstyle);
                GUILayout.Space(space);
                if (PPCtrl.eyeAdaptation.maxLuminance < PPCtrl.eyeAdaptation.minLuminance)
                {
                    PPCtrl.eyeAdaptation.maxLuminance = PPCtrl.eyeAdaptation.minLuminance;
                }
                PPCtrl.eyeAdaptation.maxLuminance = GUILayout.HorizontalSlider(PPCtrl.eyeAdaptation.maxLuminance, -10f, 10f, sliderstyle, thumbstyle);
                GUILayout.Space(space);
                if (PPCtrl.eyeAdaptation.maxLuminance < PPCtrl.eyeAdaptation.minLuminance)
                {
                    PPCtrl.eyeAdaptation.minLuminance = PPCtrl.eyeAdaptation.maxLuminance;
                }
                SliderGUI(ref PPCtrl.eyeAdaptation.speedUp, 0f, 10f, GUIStrings.Light_adaptation_speed);
                SliderGUI(ref PPCtrl.eyeAdaptation.speedDown, 0f, 10f, GUIStrings.Dark_adaptation_speed);
            }
            GUILayout.Space(space);
        }
        void AAModule()
        {
            ToggleGUITitle(ref PPCtrl.enableAntialiasing, new GUIContent("Anti-Aliasing"));
            if(PPCtrl.enableAntialiasing)
            {
                GUILayout.Space(space);
                SelectGUI(ref PPCtrl.antialiasing.method, new GUIContent(" method "), method => ModPrefs.SetString("PHIBL", "AntiAliasing", method.ToString()));
                if(PPCtrl.antialiasing.method == AntialiasingModel.Method.Fxaa)
                {
                    SelectGUI(ref PPCtrl.antialiasing.fxaaSettings.preset, new GUIContent(" preset "), 3);
                }
                else
                {
                    SliderGUI(ref PPCtrl.antialiasing.taaSettings.jitterSpread, 0.1f, 1f, new GUIContent(" Jitter Spread "));
                    SliderGUI(ref PPCtrl.antialiasing.taaSettings.stationaryBlending, 0f, 0.99f, new GUIContent(" Stationary Blending "));
                    SliderGUI(ref PPCtrl.antialiasing.taaSettings.motionBlending, 0f, 0.99f, new GUIContent(" Motion Blending "));
                    SliderGUI(ref PPCtrl.antialiasing.taaSettings.sharpen, 0f, 3f, new GUIContent(" Sharpen "));
                }
            }
            GUILayout.Space(space);
        }
        void MotionBlurModule()
        {
            ToggleGUITitle(ref PPCtrl.enableMotionBlur, new GUIContent("Motion Blur"), enable => ModPrefs.SetBool("PHIBL", "MotionBlur", enable));
            if(PPCtrl.enableMotionBlur)
            {
                GUILayout.Space(space);
                SliderGUI(ref PPCtrl.motionBlur.shutterAngle, 0f, 360f, 270f, " Shutter Angle ", valuedecimals: "N1");
                PPCtrl.motionBlur.sampleCount = (int)SliderGUI(PPCtrl.motionBlur.sampleCount, 4f, 32f, 10f, " Sample Count ", valuedecimals: "N0");
                SliderGUI(ref PPCtrl.motionBlur.frameBlending, min: 0f, max: 1f, reset: 0f, labeltext: " Frame Blend ");
            }
        }
        void NoiseModule()
        {
            ToggleGUITitle(ref PPCtrl.enableDither, GUIStrings.Dithering);
            ToggleGUITitle(ref PPCtrl.enableGrain, new GUIContent("Grain"), enable => ModPrefs.SetBool("PHIBL", "Grain", enable));
            if(PPCtrl.enableGrain)
            {
                GUILayout.Space(space);
                SliderGUI(ref PPCtrl.grain.intensity, 0f, 1f, 0.5f, " Intensity ");
                SliderGUI(ref PPCtrl.grain.size, 0.3f, 3f, 1f, " Size ");
                SliderGUI(ref PPCtrl.grain.luminanceContribution, 0f, 1f, 0.8f, " Luminance Contribution ");
                ToggleGUI(ref PPCtrl.grain.colored, new GUIContent(" Colored "));
            }
        }

        void AmbientOcclusionAction(bool enable)
        {
            ModPrefs.SetBool("PHIBL", "AmbientOcclusion", enable);
            if (enable)
            {
                Camera.main.GetComponent<SSAOPro>().enabled = false;
                if (StudioMode)
                {                    
                    Singleton<Studio.Studio>.Instance.sceneInfo.enableSSAO = false;
                }
                else
                {
                    ConfigData.ssaoEnable = false;
                }
            }
        }
        void DoFAction(bool enable)
        {
            ModPrefs.SetBool("PHIBL", "DepthOfField", enable);
            FocusPuller.enabled = autoFocus && enable;
        }
        

        void AutoFocusAction(bool enable)
        {
            FocusPuller.enabled = false;
            ModPrefs.SetBool("PHIBL", "AutoFocus", enable);
            FocusPuller.enabled = enable && PPCtrl.enableDepthOfField;
        }

        void AmbientOcclusionModule()
        {
            ToggleGUITitle(ref PPCtrl.enableAmbientOcclusion, GUIStrings.SSAO, new Action<bool>(AmbientOcclusionAction));
            if (PPCtrl.enableAmbientOcclusion)
            {
                SliderGUI(ref PPCtrl.ambientOcclusion.intensity, 0f, 4f, 1f, GUIStrings.SSAO_intensity);
                SliderGUI(ref PPCtrl.ambientOcclusion.radius, 1e-4f, 10f, 1f, GUIStrings.SSAO_radius);
                ToggleGUI(ref PPCtrl.ambientOcclusion.downsampling, GUIStrings.SSAO_Downsampling);
                SelectGUI(ref PPCtrl.ambientOcclusion.sampleCount, GUIStrings.SSAO_SampleCount, 0);

                if (Camera.main.actualRenderingPath == RenderingPath.DeferredShading)
                {
                    ToggleGUI(ref PPCtrl.ambientOcclusion.forceForwardCompatibility, GUIStrings.SSAO_ForceForwardCompatibility);
                    if(!PPCtrl.ambientOcclusion.forceForwardCompatibility)
                        ToggleGUI(ref PPCtrl.ambientOcclusion.ambientOnly, GUIStrings.SSAO_AmbientOnly);
                }
                ToggleGUI(ref PPCtrl.ambientOcclusion.highPrecision, GUIStrings.SSAO_HighPrecision);

            }
        }
        void SreenSpaceReflectionModule()
        {
            PPCtrl.enableScreenSpaceReflection = ToggleGUITitle(PPCtrl.enableScreenSpaceReflection, GUIStrings.SSR, enable => ModPrefs.SetBool("PHIBL", "ScreenSpaceReflection", enable));
            if(PPCtrl.enableScreenSpaceReflection)
            {
                SelectGUI(ref PPCtrl.screenSpaceReflection.reflection.blendType, new GUIContent(" Blend Type "));
                SelectGUI(ref PPCtrl.screenSpaceReflection.reflection.reflectionQuality, GUIStrings.SSRResolution);
                ToggleGUI(ref PPCtrl.screenSpaceReflection.reflection.reflectBackfaces, GUIStrings.SSR_reflectBackfaces);
                SliderGUI(ref PPCtrl.screenSpaceReflection.intensity.fadeDistance, 0f, 1000f, 100f, " " + nameof(PPCtrl.screenSpaceReflection.intensity.fadeDistance) + ": ");
                SliderGUI(ref PPCtrl.screenSpaceReflection.intensity.fresnelFade, 0f, 1f, 0.5f, " " + nameof(PPCtrl.screenSpaceReflection.intensity.fresnelFade) + ": ");
                SliderGUI(ref PPCtrl.screenSpaceReflection.intensity.fresnelFadePower, 0.1f, 10f, 0.8f, " " + nameof(PPCtrl.screenSpaceReflection.intensity.fresnelFadePower) + ": ");
                SliderGUI(ref PPCtrl.screenSpaceReflection.intensity.reflectionMultiplier, 0f, 2f, 1f, " " + nameof(PPCtrl.screenSpaceReflection.intensity.reflectionMultiplier) + ": ");
            }
        }
        void BloomModule()
        {
            ToggleGUITitle(ref PPCtrl.enableBloom, GUIStrings.Bloom);
            if (PPCtrl.enableBloom)
            {
                SliderGUI(ref PPCtrl.bloom.bloom.intensity, 0f, 1f, GUIStrings.Bloom_intensity, GUIStrings.Bloom_intensity_tooltips);
                SliderGUI(ref PPCtrl.bloom.bloom.threshold, 0f, 8f, GUIStrings.Bloom_threshold, GUIStrings.Bloom_threshold_tooltips);
                SliderGUI(ref PPCtrl.bloom.bloom.softKnee, 0f, 1f, GUIStrings.Bloom_softknee, GUIStrings.Bloom_softknee_tooltips);
                SliderGUI(ref PPCtrl.bloom.bloom.radius, 1f, 7f, GUIStrings.Bloom_radius, GUIStrings.Bloom_radius_tooltips);
                ToggleGUI(ref PPCtrl.bloom.bloom.antiFlicker, GUIStrings.Bloom_antiflicker);
            }
        }

        void LensModule()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIStrings.Field_Of_View, titlestyle2);
            GUILayout.FlexibleSpace();
            if (!StudioMode)
            {
                dollyZoom = UIUtils.ToggleButton(dollyZoom, new GUIContent(GUIStrings.Dolly_zoom), enable => ModPrefs.SetBool("PHIBL", "DollyZoom", enable));
            }
            else
            {
                if (GUILayout.Button(GUIStrings.Save, buttonstyleNoStretch))
                {
                    ModPrefs.SetFloat("PHIBL", "StudioFOV", Camera.main.fieldOfView);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(Camera.main.fieldOfView.ToString("N1"), labelstyle2);
            if (StudioMode)
            {
                GetComponent<CamCtrlStudio>().StudioCameraControl.fieldOfView = GUILayout.HorizontalSlider(GetComponent<CamCtrlStudio>().StudioCameraControl.fieldOfView, leftValue: 10f, rightValue: 120f, slider: sliderstyle, thumb: thumbstyle);
            }
            else
            {
                if (dollyZoom)
                {
                    GetComponent<CamCtrl>().Illcam.SetParse(GUILayout.HorizontalSlider(Camera.main.fieldOfView, leftValue: 10f, rightValue: 120f, slider: sliderstyle, thumb: thumbstyle), true);
                }
                else
                {
                    Camera.main.fieldOfView = GUILayout.HorizontalSlider(Camera.main.fieldOfView, leftValue: 10f, rightValue: 120f, slider: sliderstyle, thumb: thumbstyle);
                }
            }
            GUILayout.Space(space);

            ToggleGUITitle(ref PPCtrl.enableDepthOfField, new GUIContent(GUIStrings.Depth_Of_View), new Action<bool>(DoFAction));
            
            if (PPCtrl.enableDepthOfField)
            {
                if (StudioMode)
                    Singleton<Studio.Studio>.Instance.sceneInfo.enableDepth = false;
                GUILayout.Space(space);
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent(GUIStrings.Dof_focal_distance + PPCtrl.depthOfField.focusDistance.ToString("N3"), GUIStrings.Dof_focal_distance_tooltips), labelstyle);
                GUILayout.FlexibleSpace();
                autoFocus = UIUtils.ToggleButton(autoFocus, GUIStrings.Dof_auto_focus, new Action<bool>(AutoFocusAction));
                GUILayout.EndHorizontal();
                if (!autoFocus)
                {
                    GUILayout.Space(space);
                    PPCtrl.depthOfField.focusDistance = GUILayout.HorizontalSlider(PPCtrl.depthOfField.focusDistance,
                        leftValue: 0.1f,
                        rightValue: 20f,
                        slider: sliderstyle,
                        thumb: thumbstyle);
                }
                else
                {
                    FocusPuller.speed = SliderGUI(FocusPuller.speed, 1f, 20f, 6f, GUIStrings.Focus_Speed);
                }
                GUILayout.Space(space);
                GUILayout.Label(new GUIContent(GUIStrings.Dof_aperture + PPCtrl.depthOfField.aperture.ToString("N3"), GUIStrings.Dof_aperture_tooltips), labelstyle);
                PPCtrl.depthOfField.aperture = GUILayout.HorizontalSlider(PPCtrl.depthOfField.aperture, leftValue: 0.05f, rightValue: 32f, slider: sliderstyle, thumb: thumbstyle);
                //GUILayout.Space(UIUtils.space);
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent(GUIStrings.Dof_focallength, GUIStrings.Dof_focallength_tooltips), labelstyle);
                GUILayout.FlexibleSpace();
                PPCtrl.depthOfField.useCameraFov = UIUtils.ToggleButton(PPCtrl.depthOfField.useCameraFov, GUIStrings.Dof_auto_calc);
                GUILayout.EndHorizontal();

                if (!PPCtrl.depthOfField.useCameraFov)
                {
                    GUILayout.Label(PPCtrl.depthOfField.focalLength.ToString("N3"), labelstyle2);
                    PPCtrl.depthOfField.focalLength = GUILayout.HorizontalSlider(PPCtrl.depthOfField.focalLength, 0.1f, 100f, sliderstyle, thumbstyle);                    
                }                
                SelectGUI(ref PPCtrl.depthOfField.kernelSize, new GUIContent(GUIStrings.Dof_kernel, GUIStrings.Dof_kernel_tooltips), 0);
                GUILayout.Space(space);
            }
            ToggleGUITitle(ref PPCtrl.enableVignette, new GUIContent(GUIStrings.Vignette), enable => ModPrefs.SetBool("PHIBL", "Vignette", enable));

            if (PPCtrl.enableVignette)
            {
                ToggleGUI(ref PPCtrl.vignette.rounded, GUIStrings.Vignette_mode, GUIStrings.Vignette_mode_selection);
                SliderGUI(ref PPCtrl.vignette.intensity, 0f, 1f, GUIStrings.Vignette_intensity);
                if (!PPCtrl.vignette.rounded)
                {
                    SliderGUI(ref PPCtrl.vignette.roundness, 0f, 1f, GUIStrings.Vignette_roundness);
                }
                SliderGUI(ref PPCtrl.vignette.smoothness, 0f, 1f, GUIStrings.Vignette_smoothness);

                PPCtrl.vignette.color = SliderGUI(PPCtrl.vignette.color, Color.black, GUIStrings.Vignette_color);
            }

            ToggleGUITitle(ref PPCtrl.enableChromaticAberration, new GUIContent(GUIStrings.Chromatic_Aberration), enable => ModPrefs.SetBool("PHIBL", "ChromaticAberration", enable));
            if (PPCtrl.enableChromaticAberration)
            {
                GUILayout.Label(GUIStrings.Chromatic_aberration_intensity + PPCtrl.chromaticAberration.intensity.ToString("N3"), labelstyle);
                PPCtrl.chromaticAberration.intensity = GUILayout.HorizontalSlider(PPCtrl.chromaticAberration.intensity, 0f, 1f, sliderstyle, thumbstyle);
            }
            GUILayout.Space(space);
            //GUILayout.Label(" Lens Dirt ", UIUtils.titlestyle2);
            //GUILayout.Label(" Intensity: " + postprofile.bloom.lensDirt.intensity.ToString("N3"), UIUtils.labelstyle);
            //postprofile.bloom.lensDirt.intensity = GUILayout.HorizontalSlider(postprofile.bloom.lensDirt.intensity, 0f, 10f, UIUtils.sliderstyle, UIUtils.thumbstyle);
            //GUILayout.Space(UIUtils.space);
        }

        private void FilterModule()
        {
            GUILayout.Label(GUIStrings.Filters, titlestyle2);
            GUILayout.Space(space);
            SliderGUI(ref PPCtrl.colorGrading.basic.temperature, -100f, 100f, 0f,
                guiContent: GUIStrings.Filter_temperature,
                valuedecimals: "N1");
            SliderGUI(ref PPCtrl.colorGrading.basic.tint, -100f, 100f, 0f,
                guiContent: GUIStrings.Filter_tint,
                valuedecimals: "N1");
            SliderGUI(ref PPCtrl.colorGrading.basic.hueShift, -180f, 180f, 0f,
                guiContent: GUIStrings.Filter_hue,
                valuedecimals: "N1");
            SliderGUI(ref PPCtrl.colorGrading.basic.saturation, 0f, 2f, 1f,
                guiContent: GUIStrings.Filter_saturation);
            SliderGUI(ref PPCtrl.colorGrading.basic.contrast, 0f, 2f, 1f,
                guiContent: GUIStrings.Filter_contrast);
            GUILayout.Space(space);
        }
    }
}