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
        public List<LightSerializationData> lights;

        public LightsSerializationData()
        {
            lights = new List<LightSerializationData>();
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
            lightData.Add("shadowSoftnessFade", light.range.ToString());
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
