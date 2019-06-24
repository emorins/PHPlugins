using UnityEngine;
using IllusionPlugin;
using static PHIBL.UIUtils;

namespace PHIBL
{
    partial class PHIBL : MonoBehaviour
    {
        private void UserCustomModule()
        {
            GUILayout.Label(GUIStrings.Global_Settings, titlestyle2);
            SliderGUI(QualitySettings.shadowDistance, 20f, 150f, 30f, x => QualitySettings.shadowDistance = x, " Shadow Distance ");
            SliderGUI(Camera.main.nearClipPlane, 0.01f, 1f, 0.06f, x => Camera.main.nearClipPlane = x, new GUIContent(" Camra near clip plane "));
            ToggleGUI(Camera.main.useOcclusionCulling, new GUIContent(" Occlusion Culling "), x => Camera.main.useOcclusionCulling = x);
            //ToggleGUI(Camera.main.useJitteredProjectionMatrixForTransparentRendering, new GUIContent(" Use Jittered Projection Matrix For Transparent Rendering "), x => Camera.main.useJitteredProjectionMatrixForTransparentRendering = x);
            SelectGUI(ref vSyncCount, GUIStrings.Vsync, GUIStrings.Disable_vs_Enable, count => ModPrefs.SetInt("PHIBL", "VSync", count));
            ToggleGUI(ref asyncLoad, GUIStrings.Async_Load, enable => ModPrefs.SetBool("PHIBL", "AsyncLoad", enable));
            //ToggleGUI(ref forceDeferred, GUIStrings.Force_Deferred, enable => ModPrefs.SetBool("PHIBL", "ForceDeferred", enable));
            ToggleGUI(ref autoSetting, GUIStrings.Auto_Setting, enable => ModPrefs.SetBool("PHIBL", "AutoSetting", enable));
            ToggleGUI(ref disableLightMap, GUIStrings.DisableLightMap, disablelightmap  => ModPrefs.SetBool("PHIBL", "DisableLightMap", disablelightmap));
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIStrings.Custom_Window, labelstyle);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(GUIStrings.Custom_Window_Remember, buttonstyleNoStretch, GUILayout.ExpandWidth(false)))
            {
                ModPrefs.SetFloat("PHIBL", "Window.width", windowRect.width);
                ModPrefs.SetFloat("PHIBL", "Window.height", windowRect.height);
                ModPrefs.SetFloat("PHIBL", "Window.x", windowRect.x);
                ModPrefs.SetFloat("PHIBL", "Window.y", windowRect.y);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(space);
            float widthtemp;
            widthtemp = SliderGUI(
                value: windowRect.width, 
                min: minwidth, 
                max: UIUtils.Screen.width * 0.5f, 
                reset: () => ModPrefs.GetFloat("PHIBL", "Window.width"), 
                labeltext: GUIStrings.Window_Width, 
                valuedecimals: "N0");
            if (widthtemp != windowRect.width)
            {
                windowRect.x += (windowRect.width - widthtemp) * (windowRect.x) / (UIUtils.Screen.width - windowRect.width);
                windowRect.width = widthtemp;
            }
            windowRect.height = SliderGUI(
                value: windowRect.height, 
                min: UIUtils.Screen.height * 0.2f, 
                max: UIUtils.Screen.height - 10f, 
                reset: () => ModPrefs.GetFloat("PHIBL", "Window.height"), 
                labeltext: GUIStrings.Window_Height, 
                valuedecimals: "N0");
            SelectGUI(ref screenShotSize, new GUIContent(" Screen Shot Size: "), 0);
        }

        private int vSyncCount;
        private bool disableLightMap;
    }
}