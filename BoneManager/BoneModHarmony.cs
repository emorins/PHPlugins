using IllusionPlugin;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Harmony;
using System.IO;
namespace BoneModHarmony
{
    public class BoneModHarmony : IPlugin
    {
        public string Name => "BoneModHarmony";
        public string Version => "2.0";

        public void OnApplicationQuit() { }
        public void OnApplicationStart()
        {
            HarmonyInstance.Create(Name).PatchAll(Assembly.GetExecutingAssembly());
            Prefs.Init();
            if(!File.Exists(Prefs.defaultBMPath))
                File.Create(Prefs.defaultBMPath);
            if (!File.Exists(Prefs.defaultMaleBMPath))
                File.Create(Prefs.defaultMaleBMPath);
        }
        public void OnFixedUpdate() { }
        public void OnLevelWasInitialized(int level)
        {
            if (!BoneManager.IsInstance())
            {
                var BoneManager = new GameObject("BoneManager");
                BoneManager.AddComponent<BoneManager>();
                if (SceneManager.GetActiveScene().name == "Studio")
                {
                    BoneManager.AddComponent<CamCtrlStudio>();
                }
                else
                {
                    BoneManager.AddComponent<CamCtrl>();
                }
            }
        }
        public void OnLevelWasLoaded(int level) { }
        public void OnUpdate() { }
    }
}