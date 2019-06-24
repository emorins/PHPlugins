using IllusionPlugin;
using System.Reflection;
using UnityEngine;
using Harmony;
using Character;

namespace NipParadox
{
    public class NipPlugin : IPlugin
    {
        public string Name => "NipParadox";
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public void OnApplicationStart()
        {
            HarmonyInstance.Create(Name).PatchAll(Assembly.GetExecutingAssembly());
        }
        public void OnApplicationQuit() { }
        public void OnFixedUpdate() { }
        public void OnLevelWasInitialized(int level) { }
        public void OnLevelWasLoaded(int level) { }
        public void OnUpdate() { }
    }

    [HarmonyPatch(typeof(Wears), "CheckBodyShow")]
    static class NipPatch
    {
        static bool Prefix(bool show, Wears __instance)
        {
            var instance= Traverse.Create(__instance);
            var wearParam = instance.Field("wearParam").GetValue<WearParameter>();
            var wearObjs = instance.Field("wearObjs").GetValue<WearObj[]>();
            var skinObj = instance.Field("skinObj").GetValue<GameObject>();
            var sex = instance.Field("sex").GetValue<SEX>();
            var human = instance.Field("human").GetValue<Human>();
            var nipShow = true;
            var underHairShow = true;
            var barelyNude = false;
            if (show)
            {
                barelyNude = wearParam.isSwimwear | wearObjs[0] == null | __instance.GetShow(WEAR_SHOW_TYPE.TOPUPPER, true) == WEAR_SHOW.HIDE;
            }
            skinObj.SetActive(barelyNude);
            if (sex == SEX.FEMALE)
            {
                if (show)
                {
                    if (!wearParam.isSwimwear)
                    {
                        var topData = CustomDataManager.GetWearData_Female(WEAR_TYPE.TOP, wearParam.GetWearID(WEAR_TYPE.TOP));
                        nipShow = topData == null | topData.nip | __instance.GetShow(WEAR_SHOW_TYPE.TOPUPPER, true) != WEAR_SHOW.ALL;
                        var braDisableFlag = topData != null & topData.braDisable & __instance.GetShow(WEAR_SHOW_TYPE.TOPUPPER, true) != WEAR_SHOW.ALL;
                        var braData = CustomDataManager.GetWearData_Female(WEAR_TYPE.BRA, wearParam.GetWearID(WEAR_TYPE.BRA));
                        nipShow &= braDisableFlag | braData == null | braData.nip | __instance.GetShow(WEAR_SHOW_TYPE.BRA, true) != WEAR_SHOW.ALL;
                        var shortsID = wearParam.GetWearID(WEAR_TYPE.SHORTS);
                        var shortsData = CustomDataManager.GetWearData_Female(WEAR_TYPE.SHORTS, shortsID);
                        underHairShow = shortsID == 0 | shortsData == null | __instance.GetShow(WEAR_SHOW_TYPE.SHORTS, true) != WEAR_SHOW.ALL;
                    }
                    else
                    {
                        var swimID = wearParam.GetWearID(WEAR_TYPE.SWIM);
                        var swimData = CustomDataManager.GetWearData_Female(WEAR_TYPE.SWIM, swimID);
                        if (swimID == 50 | swimData == null)
                        {
                            nipShow = true;
                            underHairShow = true;
                        }
                        else
                        {
                            underHairShow = swimData.underhair | __instance.GetShow(WEAR_SHOW_TYPE.SWIMLOWER, true) != WEAR_SHOW.ALL;
                            nipShow = swimData.nip | __instance.GetShow(WEAR_SHOW_TYPE.SWIMUPPER, true) != WEAR_SHOW.ALL;
                        }
                    }
                    var panstData = CustomDataManager.GetWearData_Female(WEAR_TYPE.PANST, wearParam.GetWearID(WEAR_TYPE.PANST));
                    underHairShow &= panstData == null | __instance.GetShow(WEAR_SHOW_TYPE.PANST, true) != WEAR_SHOW.ALL | panstData.underhair;
                }            
                else
                {
                    nipShow = false;
                    underHairShow = false;
                }
                if (human != null)
                {
                    human.body.ShowUnderHair3D(underHairShow);
                    human.body.ShowNip(nipShow);
                    return false;
                }
            }
            else if (human != null)
            {
                human.body.ShowUnderHair3D(barelyNude);
            }
            return false;
        }
    }

}
