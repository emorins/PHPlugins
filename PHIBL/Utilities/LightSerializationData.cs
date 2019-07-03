using Harmony;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Formatters;

namespace PHIBL.Utilities
{
    public class LightsSerializationCompositeResolver : IFormatterResolver
    {
        public static IFormatterResolver Instance = new LightsSerializationCompositeResolver();

        static readonly IFormatterResolver[] resolvers = new[]
        {
        // resolver custom types first
        MessagePack.Unity.UnityResolver2.Instance,
        BuiltinResolver.Instance, // Try Builtin
        DynamicEnumResolver.Instance, // Try Enum
        DynamicGenericResolver.Instance, // Try Array, Tuple, Collection
        DynamicUnionResolver.Instance, // Try Union(Interface)
        DynamicObjectResolver.Instance, // Try Object
        PrimitiveObjectResolver.Instance, // finally, try primitive resolver
        // finaly use standard resolver
        MessagePackSerializer.DefaultResolver
        };

        LightsSerializationCompositeResolver() { }

        public IMessagePackFormatter<T> GetFormatter<T>() => FormatterCache<T>.formatter;

        static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                foreach (var item in resolvers)
                {
                    var f = item.GetFormatter<T>();
                    if (f != null)
                    {
                        formatter = f;
                        return;
                    }
                }
            }
        }
    }

    [Serializable]
    [MessagePackObject(keyAsPropertyName: true)]
    public class LightsSerializationData
    {
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
    }
}
