using Harmony;
using IllusionPlugin;
using System.Reflection;
using UnityEngine;

namespace phTattooPatch
{
    public class CustomTexturePatch : IPlugin
    {
        public string Name => "Playhome Custom Texture Patch";
        public string Version => "1.0";
        public void OnFixedUpdate() { }
        public void OnLevelWasInitialized(int level) { }
        public void OnLevelWasLoaded(int level) { }
        public void OnUpdate() { }
        public void OnApplicationQuit() { }

        public void OnApplicationStart()
        {
            var hi = HarmonyInstance.Create(Name);

            var t_CharaShapeCustomBase = typeof(CharaShapeCustomBase);
            var mi_SetTattooOffsetAndTiling = t_CharaShapeCustomBase.GetMethod("SetTattooOffsetAndTiling", BindingFlags.NonPublic | BindingFlags.Instance);
            var h_SetTattooOffsetAndTiling = new HarmonyMethod(typeof(CustomTexturePatch).GetMethod(nameof(pre_SetTattooOffsetAndTiling)));
            hi.Patch(mi_SetTattooOffsetAndTiling, h_SetTattooOffsetAndTiling, null);
        }

        public static bool pre_SetTattooOffsetAndTiling(Material mat, string propertyName, int baseW, int baseH, int texW, int texH, float offsetPx, float offsetPy)
        {
            if (offsetPx >= 10000 && offsetPy >= 10000 && offsetPx < 20000 && offsetPy < 20000)
            {
                baseW = 2048;
                baseH = 2048;
                offsetPx -= 10000;
                offsetPy -= 10000;
            }
            else if (offsetPx >= 20000 && offsetPy >= 20000 && offsetPx < 30000 && offsetPy < 30000)
            {
                baseW = 4096;
                baseH = 4096;
                offsetPx -= 20000;
                offsetPy -= 20000;
            }
            else if (offsetPx >= 30000 && offsetPy >= 30000)
            {
                baseW = 8196;
                baseH = 8196;
                offsetPx -= 30000;
                offsetPy -= 30000;
            }
            float num = baseW / (float)texW;
            float num2 = baseH / (float)texH;
            float x = -(offsetPx / baseW) * num;
            float y = -((baseH - offsetPy - texH) / baseH) * num2;
            mat.SetTextureOffset(propertyName, new Vector2(x, y));
            mat.SetTextureScale(propertyName, new Vector2(num, num2));

            return false; //Don't execute original.
        }
    }
}
