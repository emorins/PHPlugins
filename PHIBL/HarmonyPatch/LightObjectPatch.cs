using System;
using Harmony;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Studio;
using IllusionPlugin;

namespace PHIBL.Patch
{
    [HarmonyPatch(typeof(OCILight), "SetColor")]
    static class LightColorPatch
    {
        static bool Prefix(OCILight __instance, Color _color)
        {
            __instance.lightInfo.color = _color;
            __instance.light.GetComponent<AlloyAreaLight>().Color = __instance.lightInfo.color;
            if (__instance.lightColor)
            {
                __instance.lightColor.color = __instance.lightInfo.color;
            }
            return false;
        }
    }
    [HarmonyPatch(typeof(OCILight), "SetIntensity")]
    static class LightIntensityPatch
    {
        static bool Prefix(OCILight __instance, ref bool __result, float _value, bool _force = false)
        {
            if (!Utility.SetStruct(ref __instance.lightInfo.intensity, _value) && !_force)
            {
                __result = false;
                return false;
            }
            if (__instance.light)
            {
                __instance.light.GetComponent<AlloyAreaLight>().Intensity = __instance.lightInfo.intensity;
            }
            __result = true;
            return false;
        }
    }
    [HarmonyPatch(typeof(MPLightCtrl), "Awake")]
    static class LightCtrlPatch
    {
        static void Postfix(MPLightCtrl __instance)
        {
            var instance = Traverse.Create(__instance);
            var viIntensity = instance.Field("viIntensity").GetValue();
            var ValueInfo = Traverse.Create(viIntensity);
            var slider = ValueInfo.Field("slider").GetValue<Slider>();
            slider.minValue = 0f;
            slider.maxValue = ModPrefs.GetFloat("PHIBL", "Light.maxIntensity", 10f, true);
        }
    }

    [HarmonyPatch(typeof(MPLightCtrl), "OnValueChangeIntensity")]
    static class LightOVCIPatch
    {
        static bool Prefix(MPLightCtrl __instance, float _value)
        {
            var instance = Traverse.Create(__instance);
            var m_OCILight = instance.Field("m_OCILight").GetValue<OCILight>();
            var viIntensity = instance.Field("viIntensity").GetValue();
            var ValueInfo = Traverse.Create(viIntensity);
            var inputField = ValueInfo.Field("inputField").GetValue<InputField>();
            if (instance.Field("isUpdateInfo").GetValue<bool>())
            {
                return false;
            }
            if (m_OCILight.SetIntensity(_value, false))
            {
                inputField.text = m_OCILight.lightInfo.intensity.ToString("0.000");
            }
            return false;
        }
    }
    [HarmonyPatch(typeof(MPLightCtrl), "OnEndEditIntensity")]
    static class LightOEEIPatch
    {
        static bool Prefix(MPLightCtrl __instance, string _text)
        {
            var instance = Traverse.Create(__instance);
            var m_OCILight = instance.Field("m_OCILight").GetValue<OCILight>();
            var viIntensity = instance.Field("viIntensity").GetValue();
            var ValueInfo = Traverse.Create(viIntensity);
            var inputField = ValueInfo.Field("inputField").GetValue<InputField>();
            var slider = ValueInfo.Field("slider").GetValue<Slider>();
            float value = Mathf.Clamp(__instance.StringToFloat(_text), 0f, ModPrefs.GetFloat("PHIBL", "Light.maxIntensity", 10f, true));
            m_OCILight.SetIntensity(value, false);
            inputField.text = m_OCILight.lightInfo.intensity.ToString("0.00");
            slider.value = m_OCILight.lightInfo.intensity;
            return false;
        }
    }

    abstract class Utility
    {
        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (currentValue.Equals(newValue))
            {
                return false;
            }
            currentValue = newValue;
            return true;
        }
    }
}
