using IllusionPlugin;
using IllusionInjector;
using UnityEngine;
using System.Reflection;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Utility.Xml;
using System;
using Harmony;
using PHIBL;
using PHIBL.PostProcessing;

namespace LightSave
{
    public class LightSavePlugin : IPlugin, IEnhancedPlugin
    {
        public string Name => "PlayHome Light Save";
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public string[] Filter => new string[]
        {
            "PlayHome64bit",
            "PlayHomeStudio64bit"
        };

        public void OnLevelWasLoaded(int level)
        {
            if (!GameObject.Find("LightSave") && (
                SceneManager.GetActiveScene().name == "SelectScene" ||
                SceneManager.GetActiveScene().name == "EditScene" ||
                SceneManager.GetActiveScene().name == "H" ||
                SceneManager.GetActiveScene().name == "ADVScene" ||
                SceneManager.GetActiveScene().name == "Studio"))
            {
                var lightSave = new GameObject("LightSave");
                lightSave.AddComponent<LightSave>();
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
