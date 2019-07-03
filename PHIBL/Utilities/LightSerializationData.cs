using Harmony;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;

namespace PHIBL.Utilities
{
    [Serializable]
    [MessagePackObject(keyAsPropertyName: true)]
    public class LightsSerializationData
    {
        [SerializeField]
        public List<LightSerializationData> directionalLights;
        [SerializeField]
        public List<LightSerializationData> pointLights;
        [SerializeField]
        public List<LightSerializationData> spotLights;
        [SerializeField]
        public List<LightSerializationData> areaLights;

        public LightsSerializationData()
        {
            directionalLights = new List<LightSerializationData>();
            pointLights = new List<LightSerializationData>();
            spotLights = new List<LightSerializationData>();
            areaLights = new List<LightSerializationData>();
        }
        public static void Save(string path)
        {
            /*
            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
            byte[] bin = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(phibl.LightsSerializ()));
            File.WriteAllBytes(path, bin);
            */
            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
            var bin = LZ4MessagePackSerializer.Serialize(phibl.LightsSerializ(), CustomCompositeResolver.Instance);
            File.WriteAllBytes(path, bin);
        }

        public static void Load(string path)
        {
            /*
            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
            var bin = File.ReadAllBytes(path);
            string json = System.Text.Encoding.UTF8.GetString(bin);
            phibl.StartCoroutine(phibl.LightsDeserializ(json));
            */

            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
            var bin = File.ReadAllBytes(path);
            var lightsSerializationData = LZ4MessagePackSerializer.Deserialize<LightsSerializationData>(bin, CustomCompositeResolver.Instance);
            phibl.StartCoroutine(phibl.LightsDeserializ(lightsSerializationData));
        }
    }

    [Serializable]
    [MessagePackObject(keyAsPropertyName: true)]
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
            lightData.Add("name", light.name);
            lightData.Add("range", light.range.ToString());
            lightData.Add("spotAngle", light.spotAngle.ToString());
            lightData.Add("cookieSize", light.cookieSize.ToString());
            lightData.Add("renderMode", ((int)(light.renderMode)).ToString());
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
            lightData.Add("type", ((int)(light.type)).ToString());
            lightData.Add("shadowStrength", light.shadowStrength.ToString());
            lightData.Add("shadowResolution", ((int)(light.shadowResolution)).ToString());
            lightData.Add("shadowCustomResolution", light.shadowCustomResolution.ToString());
            lightData.Add("shadows", ((int)(light.shadows)).ToString());
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
