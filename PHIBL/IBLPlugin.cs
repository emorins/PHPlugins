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

namespace PHIBL
{
    public class IBLPlugin : IPlugin, IEnhancedPlugin
    {
        public static float lightIntensityMax=10f;

        public string Name => "PlayHome Image Based Lighting";
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public string[] Filter => new string[]
        {
            "PlayHome64bit",
            "PlayHomeStudio64bit"
        };

        public void OnLevelWasLoaded(int level)
        {
            Console.WriteLine("Scene loaded: " + SceneManager.GetActiveScene().name);
            if (!DeferredShadingUtils.IsInstance())
            {
                new GameObject("DeferredShading").AddComponent<DeferredShadingUtils>();
            }
            if (!GameObject.Find("PHIBL") && (
                SceneManager.GetActiveScene().name == "SelectScene" || 
                SceneManager.GetActiveScene().name == "EditScene" || 
                SceneManager.GetActiveScene().name == "H"  ||
                SceneManager.GetActiveScene().name == "ADVScene" ||
                SceneManager.GetActiveScene().name == "Studio"))
            {
                var PHIBL = new GameObject("PHIBL");
                PHIBL.AddComponent<PHIBL>();
                PHIBL.AddComponent<ScreenShot>();
                if(SceneManager.GetActiveScene().name == "EditScene")
                {
                    PHIBL.AddComponent<CamCtrl>();
                }
                else if(SceneManager.GetActiveScene().name == "Studio")
                {
                    PHIBL.AddComponent<CamCtrlStudio>();                    
                }
                else
                {
                    PHIBL.AddComponent<CheckLastMapAndTime>();
                    PHIBL.AddComponent<CamCtrl>();
                }
                foreach (IPlugin plugin in PluginManager.Plugins)
                {
                    if (plugin.Name == "Dynamic High Heel System (DHH)")
                    {
                        PHIBL.AddComponent<DHHCompatible>();
                        break;
                    }
                }
                if (!GameObject.Find("FixHDR"))
                {
                    new GameObject("FixHDR").AddComponent<fixHDR>();
                }

            }
        }
        public void OnUpdate() { }
        public void OnLateUpdate() { }
        public void OnApplicationStart()
        {
            lightIntensityMax = ModPrefs.GetFloat("PHIBL", "Light.maxIntensity", 10f, true);

            Console.WriteLine(Application.productName);
            Control xmlLocale = new Control("PHIBL/Localization", Application.systemLanguage.ToString() + ".xml", "GUIStrings", new List<Data>
            {
                new GUIStrings("Strings")
            });
            xmlLocale.Read();
            xmlLocale.Write();
            QualitySettings.SetQualityLevel(2, false);
            QualitySettings.masterTextureLimit = 0;
            QualitySettings.softVegetation = true;
            QualitySettings.softParticles = true;
            QualitySettings.pixelLightCount = 8;
            QualitySettings.realtimeReflectionProbes = true;
            QualitySettings.vSyncCount = ModPrefs.GetInt("PHIBL", "VSync", 1, true);
            QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
            QualitySettings.shadowProjection = ShadowProjection.CloseFit;
            QualitySettings.shadowCascades = 4;
            QualitySettings.shadowDistance = ModPrefs.GetFloat("PHIBL", "shadowDistance", 30f, true);
            QualitySettings.asyncUploadBufferSize = 128;
            Shader.SetGlobalFloat("_MinEdgeLength", 2f);
            HarmonyInstance.Create(Name).PatchAll(Assembly.GetExecutingAssembly());            
        }
        public void OnApplicationQuit() { }
        public void OnLevelWasInitialized(int level) { }
        public void OnFixedUpdate() { }
    }
}
