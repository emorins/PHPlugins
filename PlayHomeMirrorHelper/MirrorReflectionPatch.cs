using System;
using Harmony;
using UnityEngine;
using IllusionPlugin;
using System.Collections;
using System.Reflection;

namespace PlayHomeMirrorHelper
{
    [HarmonyPatch(typeof(MirrorReflection), "OnWillRenderObject")]
    static class MirrorReflectionPatch
    {
        static bool Prefix(MirrorReflection __instance)
        {
            Map Map;
            if (null != (Map = UnityEngine.Object.FindObjectOfType<Map>()))
            {
                Map.GetType().GetField("mirrors", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Map, null);
            }
            MirrorReflectionPlus MirrorPlus;
            if (null == (MirrorPlus = __instance.GetComponent<MirrorReflectionPlus>()))
            {
                MirrorPlus = __instance.gameObject.AddComponent<MirrorReflectionPlus>();
            }
            int texturesize = ModPrefs.GetInt("MirrorHelper", "Resolution", 2048, true);
            if (texturesize <= 1024)
                texturesize = 1024;
            else if (texturesize <= 2048)
                texturesize = 2048;
            else
                texturesize = 4096;
            MirrorPlus.m_ClipPlaneOffset = ModPrefs.GetFloat("MirrorHelper", "ClipPlaneOffset", 0, true);
            MirrorPlus.m_ReflectLayers = __instance.m_ReflectLayers;
            MirrorPlus.m_TextureSize = texturesize;
            UnityEngine.Object.Destroy(__instance);
            return false;
        }
    }

}
