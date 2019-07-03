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
        public List<LightSerializationData> lights;

        public LightsSerializationData()
        {
            lights = new List<LightSerializationData>();
        }
        public static void Save(string path)
        {
            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
            var bin = LZ4MessagePackSerializer.Serialize(phibl.LightsSerializ(), LightsSerializationCompositeResolver.Instance);
            File.WriteAllBytes(path, bin);
        }

        public static void Load(string path)
        {
            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
            var bin = File.ReadAllBytes(path);
            var lightsSerializationData = LZ4MessagePackSerializer.Deserialize<LightsSerializationData>(bin, LightsSerializationCompositeResolver.Instance);
            phibl.StartCoroutine(phibl.LightsDeserializ(lightsSerializationData));
        }
    }

    [Serializable]
    [MessagePackObject(keyAsPropertyName: true)]
    public class LightSerializationData
    {
        [SerializeField]
        public Dictionary<string, string> lightData;

        public LightSerializationData()
        {
        }

        public LightSerializationData(Light light, AlloyAreaLight alloyAreaLight = null)
        {
            lightData = new Dictionary<string, string>();
            lightData.Add("instanceId", light.GetInstanceID().ToString());
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
            if (alloyAreaLight != null)
            {
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
        }
    }
}
