using System;
using Harmony;
using UnityEngine;
using UnityEngine.Rendering;
using Studio;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PHIBL;

namespace LightSave.Patch
{
    [Harmony.HarmonyPatch(typeof(Studio.Studio), "LoadScene", new Type[] { typeof(string) })]
    static class SceneLoadPatch
    {
        [Harmony.HarmonyAfter(new string[] { "PlayHome Image Based Lighting" })]
        //[HarmonyPriority(999)]
        static void Postfix(string _path)
        {
            var newpath = _path + "_lights.extdata";
            if (File.Exists(newpath))
            {
                LightsSerializationData.Load(newpath);
            }
        }
    }

    [Harmony.HarmonyPatch(typeof(SceneInfo), "Save", new Type[] { typeof(string) })]
    static class SceneSavePatch
    {
        [Harmony.HarmonyBefore(new string[] { "PlayHome Image Based Lighting" })]
        static bool Prefix(string _path, SceneInfo __instance, ref bool __result)
        {
            LightsSerializationData.Save(_path + "_lights.extdata");
            __result = true;
            return true;
        }
    }
}
