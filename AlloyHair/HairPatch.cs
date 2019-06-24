using IllusionPlugin;
using System.Reflection;
using UnityEngine;
using Harmony;
using Character;

namespace AlloyHair
{
    public class AlloyHairPatch : IPlugin
    {
        public string Name => "Alloy Hair Patch";
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public void OnApplicationQuit() {}
        public void OnApplicationStart()
        {
            HarmonyInstance.Create(Name).PatchAll(Assembly.GetExecutingAssembly());
        }
        public void OnFixedUpdate() {}
        public void OnLevelWasInitialized(int level) {}
        public void OnLevelWasLoaded(int level) {}
        public void OnUpdate() {}
    }

    [HarmonyPatch(typeof(ColorParameter_Hair), "SetToMaterial")]
    static class HairColorPatch
    {
        static bool Prefix(ColorParameter_Hair __instance, Material material)
        {
            if (material.shader.name.Contains("Alloy") && material.shader.name.Contains("Hair"))
            {
                material.color = __instance.mainColor;
                material.SetColor("_HighlightTint0", __instance.cuticleColor);
                material.SetFloat("_HighlightWidth0", __instance.cuticleExp / 20f);
                material.SetColor("_HighlightTint1", __instance.cuticleColor * 0.82f);
                material.SetFloat("_HighlightWidth1",Mathf.Clamp01(__instance.cuticleExp / 15f + 0.2f));
                material.SetColor("_DecalColor", __instance.fresnelColor);
                material.SetFloat("_DecalWeight", Mathf.Clamp01(__instance.fresnelExp / 8f));
                return false;
            }
            return true;
        }
    }
}
