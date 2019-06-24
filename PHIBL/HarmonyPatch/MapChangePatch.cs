using Harmony;
using UnityEngine;

namespace PHIBL.Patch
{
    [HarmonyPatch(typeof(Map), "ChangeShow")]
    static class MapChangePatch
    {
        static void Postfix(Map __instance)
        {
            var colliders = __instance.GetComponentsInChildren<Collider>();
            foreach(var c in colliders)
            {
                c.enabled = GlobalData.showMap;
            }
        }
    }
}
