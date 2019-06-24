using UnityEngine;
using static PHIBL.UIUtils;
using Studio;
namespace PHIBL
{
    partial class PHIBL : MonoBehaviour
    {
        private Light seletedLight;
        void LightModule()
        {
            scrollPosition[2] = GUILayout.BeginScrollView(scrollPosition[2]);
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIStrings.Directional_Light, titlestyle2);
            GUILayout.FlexibleSpace();
            if (StudioMode)
            {
                if (GUILayout.Button(" + ", buttonstyleNoStretch))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(0);
                    LightsInit();
                }
            }
            GUILayout.EndHorizontal();
            directionalLights.ForEach(l => LightOverviewModule(l));
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIStrings.Point_Light, titlestyle2);
            GUILayout.FlexibleSpace();
            if (StudioMode)
            {
                if (GUILayout.Button(" + ", buttonstyleNoStretch))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(1);
                    LightsInit();
                }
            }
            GUILayout.EndHorizontal();
            pointLights.ForEach(l => LightOverviewModule(l));
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIStrings.Spot_light, titlestyle2);
            GUILayout.FlexibleSpace();
            if (StudioMode)
            {
                if (GUILayout.Button(" + ", buttonstyleNoStretch))
                {
                    Singleton<Studio.Studio>.Instance.AddLight(2);
                    LightsInit();
                }
            }
            GUILayout.EndHorizontal();
            spotLights.ForEach(l => LightOverviewModule(l));
            GUILayout.EndScrollView();
        }
        void LightOverviewModule(Light l)
        {
            if (l == null)
            {
                LightsInit();
                return;
            }
            GUILayout.BeginHorizontal();            
            var selected = ToggleButton(ReferenceEquals(l, seletedLight), new GUIContent(l.name));
            if (selected)
                seletedLight = l;
            GUILayout.FlexibleSpace();
            ToggleButton(l.enabled, new GUIContent(" ON "), x => l.enabled = x);
            GUILayout.EndHorizontal();
        }
        void LightInspector(Light l)
        {
            if (l.enabled)
            {
                GUILayout.Label(GUIStrings.Light_Inspector, titlestyle2);
                LightModule(l);
            }
            else
            {
                EmptyPage(new GUIContent("Selected light is disabled. "));
            }
        }

        void LightModule(Light l)
        {
            var Alloylight = l.GetComponent<AlloyAreaLight>();
            SliderGUI(Alloylight.Intensity, 0f, IBLPlugin.lightIntensityMax, 1f, value => Alloylight.Intensity = value, GUIStrings.Light_Intensity);
            SliderGUI(Alloylight.Color, Color.white, value => Alloylight.Color = value, GUIStrings.Color);
            SliderGUI(l.bounceIntensity, 0f, 8f, 1f, value => l.bounceIntensity = value, GUIStrings.Light_Bounce);
            if (l.type == LightType.Directional)
            {
                var rot = l.transform.eulerAngles;
                rot.x = Mathf.DeltaAngle(0f, rot.x);
                if (rot.x > 180f)
                {
                    rot.x -= 360f;
                }
                rot.y = Mathf.DeltaAngle(0f, rot.y);
                if (rot.y > 180f)
                {
                    rot.y -= 360f;
                }
                rot.x = SliderGUI(rot.x, -90f, 90f, 0f, "Vertical rotation");
                rot.y = SliderGUI(rot.y, -179.999f, 180f, 0f, "Horizontal rotation");
                if (rot != l.transform.eulerAngles)
                    l.transform.eulerAngles = rot;
            }
            else
            {
                SliderGUI(l.range, 0.1f, 100f, 30f, x => l.range = x, GUIStrings.Light_Range);
                if(l.type == LightType.Spot)
                    SliderGUI(l.spotAngle, 1f, 179f, 60f, x => l.spotAngle = x, GUIStrings.Spot_angle);
            }
            l.shadows = (LightShadows)SelectGUI((int)l.shadows, new GUIContent(GUIStrings.Shadow), GUIStrings.Shadow_Selections);
            if (l.shadows != LightShadows.None)
            {
                SliderExGUI(value: l.shadowStrength, reset: 1f, SetOutput: x => l.shadowStrength = x, labeltext: GUIStrings.Shadow_strength);
                SliderExGUI(value: l.shadowBias, reset: 0.002f, SetOutput: x => l.shadowBias = x, labeltext: GUIStrings.Shadow_bias);
                SliderGUI(l.shadowNormalBias, 0f, 3f, 0.4f, x => l.shadowNormalBias = x, GUIStrings.Shadow_normal_bias);
            }
            if (ToggleGUITitle(Alloylight.HasSpecularHighlight, new GUIContent(GUIStrings.Specular_Highlight), x => Alloylight.HasSpecularHighlight = x))
            {
                SliderExGUI(value: Alloylight.Radius, reset: 0f, SetOutput: x => Alloylight.Radius = x, labeltext: GUIStrings.Radius);
                if(l.type == LightType.Point)
                    SliderGUI(Alloylight.Length, 0f, 1f, 0f, x => Alloylight.Length = x, GUIStrings.Light_point_length);
                GUILayout.Space(space);
            }
            var lightshaft = l.GetComponent<LightShafts.LightShafts>();
            if (lightshaft != null)
            {
                ToggleGUITitle(lightshaft.enabled, new GUIContent("Light Shafts"), x => lightshaft.enabled = x);
                SliderGUI(ref lightshaft.m_DepthThreshold, 0f, 5f, 0.5f, " Depth Threshold ");
            }
        }
    }
}