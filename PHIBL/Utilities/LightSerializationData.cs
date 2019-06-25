using Harmony;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace PHIBL.Utilities
{
    [Serializable]
    public class LightsSerializationData
    {
        public List<LightSerializationData> directionalLights;
        public List<LightSerializationData> pointLights;
        public List<LightSerializationData> spotLights;

        public LightsSerializationData()
        {
            directionalLights = new List<LightSerializationData>();
            pointLights = new List<LightSerializationData>();
            spotLights = new List<LightSerializationData>();
        }
    }

    [Serializable]
    public class LightSerializationData : ISerializationCallbackReceiver
    {
        public Dictionary<string, string> lightData;

        [SerializeField]
        private List<string> _keyList;
        [SerializeField]
        private List<string> _valueList;

        public LightSerializationData()
        {
        }

        public LightSerializationData(Light light, AlloyAreaLight alloyAreaLight)
        {
            lightData = new Dictionary<string, string>();
            lightData.Add("range", light.range.ToString());
            lightData.Add("spotAngle", light.spotAngle.ToString());
            lightData.Add("cookieSize", light.cookieSize.ToString());
            lightData.Add("renderMode", light.renderMode.ToString());
            lightData.Add("bakedIndex", light.bakedIndex.ToString());
            lightData.Add("cullingMask", light.cullingMask.ToString());
            lightData.Add("shadowNearPlane", light.shadowNearPlane.ToString());
            lightData.Add("shadowBias", light.shadowBias.ToString());
            lightData.Add("shadowNormalBias", light.shadowNormalBias.ToString());
            lightData.Add("color_r", light.color.r.ToString());
            lightData.Add("color_g", light.color.g.ToString());
            lightData.Add("color_b", light.color.b.ToString());
            lightData.Add("color_a", light.color.a.ToString());
            lightData.Add("intensity", light.intensity.ToString());
            lightData.Add("bounceIntensity", light.bounceIntensity.ToString());
            lightData.Add("shadowStrength", light.shadowStrength.ToString());
            lightData.Add("shadowResolution", light.shadowResolution.ToString());
            lightData.Add("shadowCustomResolution", light.shadowCustomResolution.ToString());
            lightData.Add("shadows", light.shadows.ToString());
            lightData.Add("alloy_Radius", alloyAreaLight.Radius.ToString());
            lightData.Add("alloy_Length", alloyAreaLight.Length.ToString());
            lightData.Add("alloy_Intensity", alloyAreaLight.Intensity.ToString());
            lightData.Add("alloy_Color_r", alloyAreaLight.Color.r.ToString());
            lightData.Add("alloy_Color_g", alloyAreaLight.Color.g.ToString());
            lightData.Add("alloy_Color_b", alloyAreaLight.Color.b.ToString());
            lightData.Add("alloy_Color_a", alloyAreaLight.Color.a.ToString());
            lightData.Add("alloy_HasSpecularHighlight", alloyAreaLight.HasSpecularHighlight.ToString());
            lightData.Add("alloy_IsAnimatedByClip", alloyAreaLight.IsAnimatedByClip.ToString());
        }

        public void OnBeforeSerialize()
        {
            _keyList = lightData.Keys.ToList();
            _valueList = lightData.Values.ToList();
        }

        public void OnAfterDeserialize()
        {
            lightData = _keyList.Select((id, index) =>
            {
                var value = _valueList[index];
                return new { id, value };
            }).ToDictionary(x => x.id, x => x.value);

            _keyList.Clear();
            _valueList.Clear();
        }
    }
}
