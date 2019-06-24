using IllusionPlugin;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using Harmony;

namespace PlayHomeMirrorHelper
{
    public class MirrorPlugin : IPlugin, IEnhancedPlugin
    {
        public string Name => GetType().Name;
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string[] Filter => new string[]
        {
            "PlayHome64bit",
            "PlayHome32bit",
            "PlayHomeStudio64bit",
            "PlayHomeStudio32bit"
        };
        public void OnLevelWasLoaded(int level)
        {
            if (!GameObject.Find("MirrorHelper") && (
                SceneManager.GetActiveScene().name == "SelectScene" ||
                SceneManager.GetActiveScene().name == "EditScene" ||
                SceneManager.GetActiveScene().name == "H" ||
                SceneManager.GetActiveScene().name == "ADVScene" ||
                SceneManager.GetActiveScene().name == "Studio"))
            {
                var MirrorHelper = new GameObject("MirrorHelper");
                MirrorHelper.AddComponent<MirrorHelper>();
                if (!GameObject.Find("FixHDR"))
                {
                    new GameObject("FixHDR").AddComponent<fixHDR>();
                }
                if(SceneManager.GetActiveScene().name == "Studio")
                {
                    MirrorHelper.AddComponent<CamCtrlStudio>();
                }
                else
                {
                    MirrorHelper.AddComponent<CamCtrl>();
                }
            }
        }
        public void OnUpdate() { }
        public void OnLateUpdate() { }
        public void OnApplicationStart()
        {
            HarmonyInstance.Create(Name).PatchAll(Assembly.GetExecutingAssembly());
        }
        public void OnApplicationQuit() { }
        public void OnLevelWasInitialized(int level) { }
        public void OnFixedUpdate() { }        
    }
}

