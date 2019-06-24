using UnityEngine;
using IllusionPlugin;
using System;
using static PHIBL.UIUtils;
using static PHIBL.DeferredShadingUtils;
namespace PHIBL
{
    partial class PHIBL : MonoBehaviour
    {
        private void TessellationModule()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIStrings.Tessellation, titlestyle2);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(GUIStrings.Save, buttonstyleNoStretch))
            {
                ModPrefs.SetFloat("PHIBL", "Tessellation.Phong", phong);
                ModPrefs.SetFloat("PHIBL", "Tessellation.EdgeLength", edgelength);
            }
            GUILayout.EndHorizontal();
            SliderExGUI(value: phong, guiContent: GUIStrings.Tessellation_phong,
                reset: () => ModPrefs.GetFloat("PHIBL", "Tessellation.Phong", 0.5f),
                SetOutput: x => { phong = x; Shader.SetGlobalFloat(_Phong, x); });
            SliderGUI(value: edgelength, min: 2f, max: 50f, label: GUIStrings.Tessellation_edgelength,
                reset: () => ModPrefs.GetFloat("PHIBL", "Tessellation.EdgeLength", 15f),  
                SetOutput: x => { edgelength = x; Shader.SetGlobalFloat(_EdgeLength, x); });
        }

        private float phong;
        private float edgelength;
       
    }
}