using Harmony;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace PHIBL.Utilities
{
    [Serializable]
    public class LightsSerializationData
    {
        [SerializeField]
        public List<string> name = new List<string>();
        [SerializeField]
        public List<string> range = new List<string>();
        [SerializeField]
        public List<string> spotAngle = new List<string>();
        [SerializeField]
        public List<string> cookieSize = new List<string>();
        [SerializeField]
        public List<string> renderMode = new List<string>();
        [SerializeField]
        public List<string> bakedIndex = new List<string>();
        [SerializeField]
        public List<string> cullingMask = new List<string>();
        [SerializeField]
        public List<string> shadowNearPlane = new List<string>();
        [SerializeField]
        public List<string> shadowBias = new List<string>();
        [SerializeField]
        public List<string> shadowNormalBias = new List<string>();
        [SerializeField]
        public List<string> color_r = new List<string>();
        [SerializeField]
        public List<string> color_g = new List<string>();
        [SerializeField]
        public List<string> color_b = new List<string>();
        [SerializeField]
        public List<string> intensity = new List<string>();
        [SerializeField]
        public List<string> bounceIntensity = new List<string>();
        [SerializeField]
        public List<string> type = new List<string>();
        [SerializeField]
        public List<string> shadowStrength = new List<string>();
        [SerializeField]
        public List<string> shadowResolution = new List<string>();
        [SerializeField]
        public List<string> shadowCustomResolution = new List<string>();
        [SerializeField]
        public List<string> shadows = new List<string>();
        [SerializeField]
        public List<string> alloy_Radius = new List<string>();
        [SerializeField]
        public List<string> alloy_Length = new List<string>();
        [SerializeField]
        public List<string> alloy_Intensity = new List<string>();
        [SerializeField]
        public List<string> alloy_Color_r = new List<string>();
        [SerializeField]
        public List<string> alloy_Color_g = new List<string>();
        [SerializeField]
        public List<string> alloy_Color_b = new List<string>();
        [SerializeField]
        public List<string> alloy_HasSpecularHighlight = new List<string>();
        [SerializeField]
        public List<string> alloy_IsAnimatedByClip = new List<string>();

        public LightsSerializationData()
        {
        }

        public static void Save(string path)
        {
            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
            byte[] bin = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(phibl.LightsSerializ()));
            File.WriteAllBytes(path, bin);
        }

        public static void Load(string path)
        {
            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
            var bin = File.ReadAllBytes(path);
            string json = System.Text.Encoding.UTF8.GetString(bin);
            phibl.StartCoroutine(phibl.LightsDeserializ(json));
        }

        public void Serializ(Light light, AlloyAreaLight alloyAreaLight)
        {
            name.Add(light.name);
            range.Add(light.range.ToString());
            spotAngle.Add(light.spotAngle.ToString());
            cookieSize.Add(light.cookieSize.ToString());
            renderMode.Add(((int)(light.renderMode)).ToString());
            bakedIndex.Add(light.bakedIndex.ToString());
            cullingMask.Add(light.cullingMask.ToString());
            shadowNearPlane.Add(light.shadowNearPlane.ToString());
            shadowBias.Add(light.shadowBias.ToString());
            shadowNormalBias.Add(light.shadowNormalBias.ToString());
            color_r.Add(light.color.r.ToString());
            color_g.Add(light.color.g.ToString());
            color_b.Add(light.color.b.ToString());
            intensity.Add(light.intensity.ToString());
            bounceIntensity.Add(light.bounceIntensity.ToString());
            type.Add(((int)(light.type)).ToString());
            shadowStrength.Add(light.shadowStrength.ToString());
            shadowResolution.Add(((int)(light.shadowResolution)).ToString());
            shadowCustomResolution.Add(light.shadowCustomResolution.ToString());
            shadows.Add(((int)(light.shadows)).ToString());
            alloy_Radius.Add(alloyAreaLight.Radius.ToString());
            alloy_Length.Add(alloyAreaLight.Length.ToString());
            alloy_Intensity.Add(alloyAreaLight.Intensity.ToString());
            alloy_Color_r.Add(alloyAreaLight.Color.r.ToString());
            alloy_Color_g.Add(alloyAreaLight.Color.g.ToString());
            alloy_Color_b.Add(alloyAreaLight.Color.b.ToString());
            alloy_HasSpecularHighlight.Add(Convert.ToInt32(alloyAreaLight.HasSpecularHighlight).ToString());
            alloy_IsAnimatedByClip.Add(Convert.ToInt32(alloyAreaLight.IsAnimatedByClip).ToString());
        }

        public void Deserializ(ref Light light, int index)
        {
            AlloyAreaLight alloyAreaLight = light.GetComponent<AlloyAreaLight>();

            light.name = name[index];
            light.range = float.Parse(range[index]);
            light.spotAngle = float.Parse(spotAngle[index]);
            light.cookieSize = float.Parse(cookieSize[index]);
            light.renderMode = (LightRenderMode)(int.Parse(renderMode[index]));
            light.bakedIndex = int.Parse(bakedIndex[index]);
            light.cullingMask = int.Parse(cullingMask[index]);
            light.shadowNearPlane = float.Parse(shadowNearPlane[index]);
            light.shadowBias = float.Parse(shadowBias[index]);
            light.shadowNormalBias = float.Parse(shadowNormalBias[index]);
            light.color = new Color(float.Parse(color_r[index]), float.Parse(color_g[index]), float.Parse(color_b[index]));
            light.intensity = float.Parse(intensity[index]);
            light.bounceIntensity = float.Parse(bounceIntensity[index]);
            light.type = (LightType)(int.Parse(type[index]));
            light.shadowStrength = float.Parse(shadowStrength[index]);
            light.shadowResolution = (LightShadowResolution)(int.Parse(shadowResolution[index]));
            light.shadowCustomResolution = int.Parse(shadowCustomResolution[index]);
            light.shadows = (LightShadows)(int.Parse(shadows[index]));

            alloyAreaLight.Radius = float.Parse(alloy_Radius[index]);
            alloyAreaLight.Length = float.Parse(alloy_Length[index]);
            alloyAreaLight.Intensity = float.Parse(alloy_Intensity[index]);
            alloyAreaLight.Color = new Color(float.Parse(alloy_Color_r[index]), float.Parse(alloy_Color_g[index]), float.Parse(alloy_Color_b[index]));
            alloyAreaLight.HasSpecularHighlight = (int.Parse(alloy_HasSpecularHighlight[index]) == 1);
            alloyAreaLight.IsAnimatedByClip = (int.Parse(alloy_IsAnimatedByClip[index]) == 1);
        }
    }
}
