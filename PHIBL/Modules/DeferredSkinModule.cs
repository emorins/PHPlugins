using UnityEngine;
using static PHIBL.DeferredShadingUtils;
using IllusionPlugin;
using static PHIBL.UIUtils;

namespace PHIBL
{
    partial class PHIBL : MonoBehaviour
    {
        void DeferredSkinModule()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIStrings.Skin_Scattering, titlestyle2);
            GUILayout.FlexibleSpace();
            if(GUILayout.Button(GUIStrings.Save, buttonstyleNoStretch, GUILayout.ExpandWidth(false)))
            {
                JSON.Utilities.SaveSettings(deferredShading.SSSSS.SkinSettings, JSON.Utilities.SkinSettings);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(space);
            SliderGUI(deferredShading.SSSSS.SkinSettings.Weight, min: 0f, max: 1f, reset: 1f,
                SetOutput: x => { deferredShading.SSSSS.SkinSettings.Weight = x; deferredShading.SSSSS.Refresh(); },
                labeltext: GUIStrings.Skin_Scattering_Weight, tooltip: GUIStrings.Skin_Scattering_Weight_Tooltips);
            SliderGUI(deferredShading.SSSSS.SkinSettings.MaskCutoff, min: 0.01f, max: 1f, reset: 0.1f, 
                SetOutput: x => { deferredShading.SSSSS.SkinSettings.MaskCutoff = x; deferredShading.SSSSS.Refresh(); },
                labeltext: GUIStrings.Skin_Scattering_Mask_Cutoff, tooltip: GUIStrings.Skin_Scattering_Mask_Cutoff_Tooltips);
            SliderExGUI(deferredShading.SSSSS.SkinSettings.Bias, reset: 0.01f, power: 0.4f, 
                SetOutput: x => {deferredShading.SSSSS.SkinSettings.Bias = x; deferredShading.SSSSS.Refresh(); }, 
               labeltext: GUIStrings.Skin_scattering_bias, tooltip: GUIStrings.Skin_scattering_bias_tooltips);
            SliderExGUI(deferredShading.SSSSS.SkinSettings.Scale, reset: 1f, power: 0.4f, 
                SetOutput: x => { deferredShading.SSSSS.SkinSettings.Scale = x; deferredShading.SSSSS.Refresh(); },
                labeltext: GUIStrings.Skin_Scattering_Scale, tooltip: GUIStrings.Skin_Scattering_Scale_Tooltips);
            SliderGUI(deferredShading.SSSSS.SkinSettings.BumpBlur, min: 0f, max: 1f, reset: 0.7f, 
                SetOutput: x => { deferredShading.SSSSS.SkinSettings.BumpBlur = x; deferredShading.SSSSS.Refresh(); },
                labeltext: GUIStrings.Skin_bump_blur, tooltip: GUIStrings.Skin_bump_blur_tooltips);
            GUILayout.Space(space);
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIStrings.Skin_Transmission, titlestyle2);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(GUIStrings.Save, buttonstyleNoStretch, GUILayout.ExpandWidth(false)))
            {
                JSON.Utilities.SaveSettings(deferredShading.SSSSS.TransmissionSettings, JSON.Utilities.TransmissionSettings);
            }
            GUILayout.EndHorizontal();
            SliderGUI(deferredShading.SSSSS.TransmissionSettings.Weight, min: 0f, max: 1f, reset: 1f, 
                SetOutput: x => { deferredShading.SSSSS.TransmissionSettings.Weight = x; deferredShading.SSSSS.Refresh(); },
                labeltext: GUIStrings.Skin_transmission_weight, tooltip: GUIStrings.Skin_transmission_weight_tooltips);
            SliderGUI(deferredShading.SSSSS.TransmissionSettings.ShadowWeight, min: 0f, max: 1f, reset: 0.5f,
                SetOutput: x => { deferredShading.SSSSS.TransmissionSettings.ShadowWeight = x; deferredShading.SSSSS.Refresh(); },
                labeltext: GUIStrings.Skin_transmission_shadow_weight, tooltip: GUIStrings.Skin_transmission_shadow_weight_tooltips);
            SliderGUI(deferredShading.SSSSS.TransmissionSettings.BumpDistortion, min: 0f, max: 1f, reset: 0.05f,
                SetOutput: x => { deferredShading.SSSSS.TransmissionSettings.BumpDistortion = x; deferredShading.SSSSS.Refresh(); },
                labeltext: GUIStrings.Skin_bump_distortion, tooltip: GUIStrings.Skin_bump_distortion_tooltips);
            SliderGUI(deferredShading.SSSSS.TransmissionSettings.Falloff, min: 1f, max: 10f, reset: 1f,
                SetOutput: x => { deferredShading.SSSSS.TransmissionSettings.Falloff = x; deferredShading.SSSSS.Refresh(); },
                labeltext: GUIStrings.Skin_transmission_falloff, tooltip: GUIStrings.Skin_transmission_falloff_tooltips);
            GUILayout.Space(space);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Nipples", titlestyle2);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(GUIStrings.Save, buttonstyleNoStretch, GUILayout.ExpandWidth(false)))
            {
                ModPrefs.SetFloat("PHIBL", "NippleSSS", nippleSSS);
            }
            GUILayout.EndHorizontal();
            SliderGUI(nippleSSS, min: 0f, max: 1f, 
                reset: () => ModPrefs.GetFloat("PHIBL", "NippleSSS", 0.5f),
                SetOutput: x => { nippleSSS = x; Shader.SetGlobalFloat(_AlphaSSS, x); },
                label: new GUIContent(" Scattering and Transmission Weight: "));
        }
        private float nippleSSS;
    }
}