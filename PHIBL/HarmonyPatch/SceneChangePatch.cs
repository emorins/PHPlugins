using System;
using Harmony;
using UnityEngine.SceneManagement;

namespace PHIBL.Patch
{

    [HarmonyPatch(typeof(SceneControl), "Change", new Type[] { typeof(string)})]
    static class SceneChangePatch
    {
        static void Prefix()
        {
            if (SceneManager.GetActiveScene().buildIndex == -1)
            {                
                var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
                var scenebundle = phibl.Scenebundle;

                foreach (var GO in phibl.RootGOs)
                {
                    if (GO != null)
                    {
                        Console.WriteLine("Destroy Root Gameobject: " + GO.name);
                        UnityEngine.Object.DestroyImmediate(GO);
                    }
                }
                scenebundle.Unload(true);
            }
        }
    }
    
}
