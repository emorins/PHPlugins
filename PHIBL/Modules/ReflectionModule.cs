using UnityEngine;
using static PHIBL.UIUtils;
using System;
namespace PHIBL
{
    partial class PHIBL : MonoBehaviour
    {
        void ReflectionProbeRefreshModule()
        {
            SelectGUI(ref rpRate, new GUIContent(GUIStrings.Reflection_probe_refresh_rate), 0, new Action<ReflectionProbeRefreshRate>(ReflectionProbeChangeMode));
        }
        void ReflectionProbeChangeMode(ReflectionProbeRefreshRate rate)
        {
            var reflectionProbes = FindObjectsOfType<ReflectionProbe>();
            foreach (var rp in reflectionProbes)
            {
                rp.hdr = true;
                rp.clearFlags = UnityEngine.Rendering.ReflectionProbeClearFlags.Skybox;
                rp.cullingMask = 1 | ~Camera.main.cullingMask;
                switch (rate)
                {
                    default:
                    case ReflectionProbeRefreshRate.OnDemand:
                        rp.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
                        rp.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
                        break;
                    case ReflectionProbeRefreshRate.Low:
                        rp.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
                        rp.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.IndividualFaces;
                        break;
                    case ReflectionProbeRefreshRate.High:
                        rp.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
                        rp.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
                        break;
                    case ReflectionProbeRefreshRate.Extreme:
                        rp.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
                        rp.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
                        break;
                }
            }
        }
        void ReflectionProbeModule()
        {
            GUILayout.Label(GUIStrings.Reflection, titlestyle2);
            GUILayout.Space(space);
            RenderSettings.reflectionBounces = (int)SliderGUI(RenderSettings.reflectionBounces, 1, 5, 1, GUIStrings.Reflection_Bounces, "N0");
            ReflectionProbeRefreshModule();
            if (selectedScene == -1)
            {       
                GUILayout.Label(GUIStrings.Reflection_probe_resolution, labelstyle);

                switch (probeComponent.resolution)
                {
                    case 128:
                        ReflectionProbeResolution = 0;
                        break;
                    default:
                    case 256:
                        ReflectionProbeResolution = 1;
                        break;
                    case 512:
                        ReflectionProbeResolution = 2;
                        break;
                }

                ReflectionProbeResolution = GUILayout.SelectionGrid(ReflectionProbeResolution, new string[]
                {
                    "128",
                    "256",
                    "512"
                }, 3, selectstyle);

                switch (ReflectionProbeResolution)
                {
                    case 0:
                        probeComponent.resolution = 128;
                        break;
                    default:
                    case 1:
                        probeComponent.resolution = 256;
                        break;
                    case 2:
                        probeComponent.resolution = 512;
                        break;
                }
                GUILayout.BeginHorizontal();
                GUILayout.Label(GUIStrings.Reflection_Intensity, labelstyle);
                GUILayout.Space(space);
                if (GUILayout.Button(GUIStrings.Reset, buttonstyleNoStretch))
                {
                    probeComponent.intensity = 1f;
                }
                GUILayout.EndHorizontal();
                GUILayout.Label(probeComponent.intensity.ToString("N3"), labelstyle2);
                probeComponent.intensity = GUILayout.HorizontalSlider(probeComponent.intensity, 0f, 2f, sliderstyle, thumbstyle);
                GUILayout.Space(space);
            }
        }

    }
}
