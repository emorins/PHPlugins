using Harmony;
using UnityEngine;
using System;
using System.Reflection;

namespace PHIBL.Patch
{
    [HarmonyPatch(typeof(Body), "ChangeNip")]
    static class NipsPatch
    {
        static void Postfix(Body __instance)
        {
            DeferredShadingUtils.FixNips(__instance.Obj.GetComponentsInChildren<SkinnedMeshRenderer>(true));
        }
    }

    [HarmonyPatch(typeof(Body), "ChangeSkinMaterial")]
    static class BodySkinPatch
    {
        static void Postfix()
        {
            if (Camera.main.actualRenderingPath == RenderingPath.DeferredShading)
            {
                DeferredShadingUtils.Refresh();
            }
        }
    }
    [HarmonyPatch(typeof(Head), "ChangeSkinMaterial")]
    static class HeadSkinPatch
    {
        static void Postfix()
        {
            if (Camera.main.actualRenderingPath == RenderingPath.DeferredShading)
            {
                DeferredShadingUtils.Refresh();
            }
        }
    }
    [HarmonyPatch(typeof(Human), "ChangeHead", new Type[] { typeof(string)})]
    static class HeadProbePatch
    {
        static void Postfix(Human __instance)
        {
            RendererSetup.SetupProbes(__instance);
        }
    }
    [HarmonyPatch(typeof(Female), "Apply")]
    static class FemaleProbePatch
    {
        static void Postfix(Female __instance)
        {
            RendererSetup.SetupProbes(__instance);
        }
    }

    [HarmonyPatch(typeof(Male), "Apply")]
    static class MaleProbePatch
    {
        static void Postfix(Male __instance)
        {
            RendererSetup.SetupProbes(__instance);
        }
    }
    [HarmonyPatch(typeof(Female), "ApplyCoordinate")]
    static class FeMaleCoordinateProbePatch
    {
        static void Postfix(Female __instance)
        {
            RendererSetup.SetupProbes(__instance);
        }
    }
    [HarmonyPatch(typeof(Male), "ApplyCoordinate")]
    static class MaleCoordinateProbePatch
    {
        static void Postfix(Male __instance)
        {
            RendererSetup.SetupProbes(__instance);
        }
    }
    [HarmonyPatch(typeof(Female), "ApplyHair")]
    static class FeMaleHairProbePatch
    {
        static void Postfix(Female __instance)
        {
            RendererSetup.SetupProbes(__instance);
        }
    }
    [HarmonyPatch(typeof(Male), "ApplyHair")]
    static class MaleHairProbePatch
    {
        static void Postfix(Male __instance)
        {
            RendererSetup.SetupProbes(__instance);
        }
    }
    [HarmonyPatch(typeof(AcceCopyHelperUI), "Button_CopySlot")]
    static class AcceCopyHelperProbePatch
    {
        static void Postfix()
        {
            RendererSetup.SetupProbes();
        }
    }
    [HarmonyPatch(typeof(AccessoryCustomEdit), "OnChangeAcceItem")]
    static class AccessoryCustomProbePatch
    {
        static void Postfix()
        {
            RendererSetup.SetupProbes();
        }
    }
    //[HarmonyPatch(typeof(Wears), "WearInstantiate")]
    //static class WearsProbePatch
    //{
    //    static void Postfix(Character.WEAR_TYPE type, Material skinMaterial, Material customHighlightMat_Skin, Wears __instance)
    //    {
    //        if (__instance.objWear[(int)type] == null)
    //            return;
    //        RendererSetup.SetupProbes(__instance.objWear[(int)type]);
    //    }
    //}
}
