using System;
using Harmony;
using UnityEngine;
using UnityEngine.Rendering;
using Studio;
using System.IO;
using PHIBL.Utilities;

namespace PHIBL.Patch
{
    [HarmonyPatch(typeof(Studio.Studio), "LoadScene", new Type[] { typeof(string) })]
    static class SceneLoadPatch
    {
        static void Postfix(string _path)
        {
            var newpath = _path + ".extdata";
            if (File.Exists(newpath))
            {
                Profile.Load(newpath);
            }

            newpath = _path + "_lights.extdata";
            if (File.Exists(newpath))
            {
                LightsSerializationData.Load(newpath);
            }
        }
    }

    [HarmonyPatch(typeof(SceneInfo), "Save", new Type[] { typeof(string) })]
    static class SceneSavePatch
    {
        static bool Prefix(string _path, SceneInfo __instance, ref bool __result)
        {
            Profile.Save(_path + ".extdata");
            LightsSerializationData.Save(_path + "_lights.extdata");

            using (FileStream fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    byte[] buffer = __instance.CreatePngScreen(320, 180);
                    binaryWriter.Write(buffer);
                    binaryWriter.Write(__instance.version.ToString());
                    __instance.Save(binaryWriter, __instance.dicObject);
                    binaryWriter.Write(__instance.map);
                    __instance.caMap.Save(binaryWriter);
                    binaryWriter.Write(__instance.atmosphere);
                    binaryWriter.Write(__instance.enableSSAO);
                    binaryWriter.Write(__instance.ssaoIntensity);
                    binaryWriter.Write(__instance.ssaoRadius);
                    binaryWriter.Write(JsonUtility.ToJson(__instance.ssaoColor));
                    binaryWriter.Write(__instance.enableBloom);
                    binaryWriter.Write(__instance.bloomIntensity);
                    binaryWriter.Write(__instance.bloomDirt);
                    binaryWriter.Write(__instance.enableDepth);
                    binaryWriter.Write(__instance.depthFocalSize);
                    binaryWriter.Write(__instance.depthAperture);
                    binaryWriter.Write(__instance.enableVignette);
                    binaryWriter.Write(__instance.vignetteVignetting);
                    binaryWriter.Write(__instance.enableEyeAdaptation);
                    binaryWriter.Write(__instance.eyeAdaptationIntensity);
                    binaryWriter.Write(__instance.enableNoise);
                    binaryWriter.Write(__instance.noiseIntensity);
                    __instance.cameraSaveData.Save(binaryWriter);
                    for (int i = 0; i < 10; i++)
                    {
                        __instance.cameraData[i].Save(binaryWriter);
                    }
                    binaryWriter.Write(JsonUtility.ToJson(__instance.cameraLightColor));
                    binaryWriter.Write(__instance.cameraLightIntensity);
                    binaryWriter.Write(__instance.cameraLightRot[0]);
                    binaryWriter.Write(__instance.cameraLightRot[1]);
                    binaryWriter.Write(__instance.cameraLightShadow);
                    binaryWriter.Write(__instance.cameraMethod);
                    __instance.bgmCtrl.Save(binaryWriter, __instance.version);
                    __instance.envCtrl.Save(binaryWriter, __instance.version);
                    __instance.outsideSoundCtrl.Save(binaryWriter, __instance.version);
                    binaryWriter.Write(__instance.background);
                    binaryWriter.Write(__instance.skybox);

                    binaryWriter.Write("【PHStudio】");
                }
            }
            __result = true;
            return false;
        }
    }

    //[HarmonyPatch(typeof(Studio.Studio), "SaveScene")]
    //static class SceneSavePatch
    //{
    //    static bool Prefix(Studio.Studio __instance)
    //    {

    //        foreach (KeyValuePair<int, ObjectCtrlInfo> keyValuePair in __instance.dicObjectCtrl)
    //        {
    //            keyValuePair.Value.OnSavePreprocessing();
    //        }
    //        __instance.sceneInfo.cameraSaveData = __instance.cameraCtrl.Export();
    //        DateTime now = DateTime.Now;
    //        string str = string.Format("{0}_{1:00}{2:00}_{3:00}{4:00}_{5:00}_{6:000}.png", new object[]
    //        {
    //            now.Year,
    //            now.Month,
    //            now.Day,
    //            now.Hour,
    //            now.Minute,
    //            now.Second,
    //            now.Millisecond
    //        });
    //        string path = UserData.Create("studio/scene") + str;
    //        Profile.Save(path + ".extdata");
    //        __instance.sceneInfo.Save(path);
    //        return false;
    //    }
    //}
}
