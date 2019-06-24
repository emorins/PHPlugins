using System;
using Harmony;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Reflection;
using IllusionPlugin;
namespace PHIBL.Patch
{
    [HarmonyPatch(typeof(MapShadowControl), "Start")]
    static class MapShadowPatch
    {
        static bool Prefix(MapShadowControl __instance)
        {
            return !ModPrefs.GetBool("PHIBL", "DisableLightMap", true, true);
        }
    }

    [HarmonyPatch(typeof(LightMapControl), "Apply")]
    static class LightMapPatch
    {
        static bool Prefix(LightMapControl __instance)
        {
            return !ModPrefs.GetBool("PHIBL", "DisableLightMap", true, true);
        }
    }
}
