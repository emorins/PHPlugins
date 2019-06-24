using System;
using IllusionPlugin;
using System.Reflection;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NormalRecalculate
{
    public class NormalRecalculatePlugin : IPlugin, IEnhancedPlugin
    {
        public string Name => GetType().Name;
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string[] Filter => new string[]
        {
            "PlayHome64bit",
            "PlayHome32bit"
        };
        public void OnLevelWasLoaded(int level)
        {
            if (!UnityEngine.Object.FindObjectOfType<RuntimeNormalRecalculate>() && (
            SceneManager.GetActiveScene().name == "SelectScene" ||
            SceneManager.GetActiveScene().name == "EditScene" ||
            SceneManager.GetActiveScene().name == "H" ||
            SceneManager.GetActiveScene().name == "ADVScene"))
            {
                new GameObject("RuntimeNormalRecalculate").AddComponent<RuntimeNormalRecalculate>();
            }
        }
        public void OnUpdate() { }
        public void OnLateUpdate() { }
        public void OnApplicationStart() { }
        public void OnApplicationQuit() { }
        public void OnLevelWasInitialized(int level) { }
        public void OnFixedUpdate() { }
    }
}
