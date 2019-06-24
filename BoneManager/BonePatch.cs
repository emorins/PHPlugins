using System;
using Harmony;
using UnityEngine;
using Studio;
using MessagePack;
using System.IO;
namespace BoneModHarmony.Patch
{
    [HarmonyPatch(typeof(Female), "Start")]
    static class FemaleBonePatch
    {
        static void Postfix(Female __instance)
        {
            if (Application.productName == "PlayHomeStudio")
                return;
            __instance.GetOrAddComponent<BMMHuman>().Name = Female.HeroineName(__instance.HeroineID);
        }
    }
    [HarmonyPatch(typeof(Male), "Start")]
    static class MaleBonePatch
    {
        static void Postfix(Male __instance)
        {
            if (Application.productName == "PlayHomeStudio")
                return;
            __instance.GetOrAddComponent<BMMHuman>().Name = Male.MaleName(__instance.MaleID);
        }
    }
    [HarmonyPatch(typeof(AddObjectFemale), "Add", new Type[] { typeof(ChaControl), typeof(OICharInfo), typeof(ObjectCtrlInfo), typeof(TreeNodeObject), typeof(bool), typeof(int) })]
    static class StudioFemaleBonePatch
    {
        static void Postfix(ChaControl _female, OICharInfo _info)
        {
            var bmm = _female.human.GetOrAddComponent<BMMHuman>();
            bmm.Name = _info.fileStatus.name;
            //bmm.reset = true;
            Console.WriteLine("\nBone modified on new female {0}. ", _info.fileStatus.name);
        }
    }
    [HarmonyPatch(typeof(AddObjectMale), "Add", new Type[] { typeof(ChaControl), typeof(OICharInfo), typeof(ObjectCtrlInfo), typeof(TreeNodeObject), typeof(bool), typeof(int) })]
    static class StudioMaleBonePatch
    {
        static void Postfix(ChaControl _male, OICharInfo _info)
        {
            var bmm = _male.human.GetOrAddComponent<BMMHuman>();
            bmm.Name = _info.fileStatus.name;
            //bmm.reset = true;
            Console.WriteLine("\nBone modified on new male {0}. ", _info.fileStatus.name);
        }
    }
    [HarmonyPatch(typeof(Body), "ShapeApply")]
    static class BodyShapePatch
    {
        static void Postfix(Body __instance)
        {
            var bmm = __instance.Obj.GetComponentInParent<BMMHuman>();
            if (bmm != null)
                bmm.OnShapeApplied();
        }
    }
    [HarmonyPatch(typeof(Head), "ShapeApply")]
    static class HeadShapePatch
    {
        static void Postfix(Head __instance)
        {
            var bmm = __instance.Obj.GetComponentInParent<BMMHuman>();
            if (bmm != null)
                bmm.OnShapeApplied();
        }
    }
    [HarmonyPatch(typeof(OCIChar), "ChangeChara")]
    static class ChangeCharaPatch
    {
        static void Postfix(OCIChar __instance)
        {
            var bmm = __instance.charInfo.human.GetComponent<BMMHuman>();
            bmm.Name = __instance.charStatus.name;
            bmm.reset = true;
        }
    }
    [HarmonyPatch(typeof(Human), "ToCharaPNG", new Type[] {typeof(string) , typeof(string) , typeof(Texture2D)})]
    static class HumanSavePatch
    {
        static void Postfix(Human __instance, string file)
        {
            var bmm = __instance.GetComponent<BMMHuman>();
            var name = Path.GetFileNameWithoutExtension(file);
            Console.WriteLine(name + "'s bonemod profile saved");
            bmm.SaveProfile();
            //File.WriteAllBytes(Prefs.GetCharaPathMsgPack(name, __instance.sex), LZ4MessagePackSerializer.Serialize(bmm.boneModifiers));
        }
    }
    [HarmonyPatch(typeof(Human), "ToCharaPNG", new Type[] { typeof(string), typeof(string)})]
    static class HumanSavePatch2
    {
        static void Postfix(Human __instance, string file)
        {
            var bmm = __instance.GetComponent<BMMHuman>();
            var name = Path.GetFileNameWithoutExtension(file);
            Console.WriteLine(name + "'s bonemod profile saved");
            bmm.SaveProfile();
            //File.WriteAllBytes(Prefs.GetCharaPathMsgPack(name, __instance.sex), LZ4MessagePackSerializer.Serialize(bmm.boneModifiers));
        }
    }
    [HarmonyPatch(typeof(DataCustomEdit), "Chara_Load")]
    static class CharaLoadPatch
    {
        static void Postfix(DataCustomEdit __instance, string file)
        {
            var human = Traverse.Create(__instance).Field("human").GetValue<Human>();
            var bmm = human.GetComponent<BMMHuman>();
            var name = Path.GetFileNameWithoutExtension(file);
            bmm.Name = name;
            bmm.reset = true;
            Console.WriteLine(name + "'s bonemod profile loaded");
        }
    }
}
