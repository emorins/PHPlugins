using System;
using Harmony;
using UnityEngine;
using Studio;
using System.Text;
using System.IO;

namespace PHIBL.Patch
{
    [HarmonyPatch(typeof(SceneInfo), "CreatePngScreen")]
    static class SceneScreenShotPatch
    {
        static bool Prefix(ref byte[] __result,int _width, int _height)
        {
            _width *= 2;
            _height *= 2;
            var tempTex = new Texture2D(_width, _height, TextureFormat.RGB24, false);
            RenderTexture rt = RenderTexture.GetTemporary(_width, _height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Camera.main.targetTexture = rt;
            Camera.main.Render();
            RenderTexture.active = rt;
            tempTex.ReadPixels(new Rect(0f, 0f, _width, _height), 0, 0);
            tempTex.Apply();
            RenderTexture.active = null;
            Camera.main.targetTexture = null;
            __result = tempTex.EncodeToPNG();
            RenderTexture.ReleaseTemporary(rt);
            return false;
        }
    }
    [HarmonyPatch(typeof(GameScreenShotAssist), "Start")]
    static class StudioScreenShotAssistPatch
    {
        static bool Prefix(GameScreenShotAssist __instance)
        {
            UnityEngine.Object.DestroyImmediate(__instance);
            return false;
        }
    }
    [HarmonyPatch(typeof(GameScreenShot), "Capture")]
    static class StudioScreenShotPatch
    {
        static bool Prefix()
        {
            ScreenShot.Capture = true;
            return false;
        }
    }
    [HarmonyPatch(typeof(EndFrameToScreenShot), "LateUpdate")]
    static class GameScreenShotPatch
    {
        static bool Prefix(EndFrameToScreenShot __instance)
        {
            if (Input.GetKeyDown(__instance.ssKey))
            {
                ScreenShot.Capture = true;                
            }
            return false;
        }
    }
}
