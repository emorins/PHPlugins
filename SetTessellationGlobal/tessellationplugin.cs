using System;
using IllusionPlugin;
using System.Reflection;
using UnityEngine;
using IllusionInjector;

namespace TessellationGlobal
{
    public class TessellationGlobalPatch : IPlugin
    {
        public string Name => "Tessellation Global Plugin";
        public string Version => "1.0";
        public void OnFixedUpdate() { }
        public void OnLevelWasInitialized(int level) { }
        public void OnLevelWasLoaded(int level) { }
        public void OnUpdate() { }
        public void OnApplicationQuit() { }
        bool CheckPHIBL()
        {
            foreach (IPlugin plugin in PluginManager.Plugins)
            {
                if (plugin.Name == "PlayHome Image Based Lighting")
                {
                    return true;
                }                
            }
            return false;
        }
        public void OnApplicationStart()
        {

            if (!CheckPHIBL())
            {
                float phong = ModPrefs.GetFloat("PHIBL", "Tessellation.Phong", 0, true);
                float edgelength = ModPrefs.GetFloat("PHIBL", "Tessellation.EdgeLength", 15, true);
                if (phong < 0f)
                    phong = 0f;
                else if (phong > 1f)
                    phong = 1f;
                if (edgelength < 2f)
                    edgelength = 2f;
                else if (edgelength > 50f)
                    edgelength = 50f;
                Shader.SetGlobalFloat("_Phong", phong);
                Shader.SetGlobalFloat("_EdgeLength", edgelength);
            }
        }


    }
}
