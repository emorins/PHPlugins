using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.IO;
using IllusionPlugin;
using System.Collections.Generic;
using PHIBL;
using PHIBL.PostProcessing;
using UnityEngine.Rendering;
using Studio;
using System.Reflection;

namespace LightSave
{
    public partial class LightSave : MonoBehaviour
    {
        LightSave()
        {
        }

        void Update()
        {
            if (LightsSerializationData.loaded == false)
            {
                var scene = Singleton<Studio.Scene>.Instance;
                var phibl = UnityEngine.Object.FindObjectOfType<PHIBL.PHIBL>();
                if (LightsSerializationData.path != null && phibl != null && scene != null)
                {
                    Type type = phibl.GetType();
                    FieldInfo field = type.GetField("IsLoading", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    bool IsLoading = (bool)(field.GetValue(phibl));

                    if (field.GetValue(phibl) != null && scene.isNowLoading == false && IsLoading == false)
                    {
                        LightsSerializationData.Load(LightsSerializationData.path);
                        LightsSerializationData.loaded = true;

                        //MethodInfo method = phibl.GetType().GetMethod("LightsInit");
                        //method.Invoke(phibl, null);
                    }
                }
            }
        }

        public LightsSerializationData LightsSerializ()
        {
            LightsSerializationData lightsSerializationData = new LightsSerializationData();

            Light[] allLights = UnityEngine.Object.FindObjectsOfType<Light>();
            foreach (Light light in allLights)
            {
                lightsSerializationData.Serializ(light);
            }

            Dictionary<TreeNodeObject, ObjectCtrlInfo> dicInfo = Singleton<Studio.Studio>.Instance.dicInfo;
            foreach (KeyValuePair<TreeNodeObject, ObjectCtrlInfo> kvp in dicInfo)
            {
                if (kvp.Value != null && kvp.Key != null)
                {
                    if (kvp.Value is OCILight)
                    {
                        OCILight value = kvp.Value as OCILight;
                        lightsSerializationData.Serializ(value.light, value);
                    }
                }
            }

            return lightsSerializationData;
        }

        public void LightsDeserializ(LightsSerializationData lightsSerializationData)
        {
            Light[] allLights = UnityEngine.Object.FindObjectsOfType<Light>();
            Dictionary<TreeNodeObject, ObjectCtrlInfo> dicInfo = Singleton<Studio.Studio>.Instance.dicInfo;

            List<Light> deserialized = new List<Light>();
            for (int i = 0; i < lightsSerializationData.name.Count(); i++)
            {
                if (int.Parse(lightsSerializationData.hasStudio[i]) == 1)
                {
                    foreach (KeyValuePair<TreeNodeObject, ObjectCtrlInfo> kvp in dicInfo)
                    {
                        if (kvp.Value != null && kvp.Key != null)
                        {
                            if (kvp.Value is OCILight)
                            {
                                OCILight value = kvp.Value as OCILight;
                                if (deserialized.Contains(value.light) == false &&
                                    lightsSerializationData.name[i] == value.light.name &&
                                    (LightType)(int.Parse(lightsSerializationData.type[i])) == value.light.type &&
                                    lightsSerializationData.hierarchyPath[i] == LightsSerializationData.GetHierarchyPath(value.light) &&
                                    LightsSerializationData.ToVector3(lightsSerializationData.transform_position[i]) == value.light.transform.position &&
                                    LightsSerializationData.ToVector3(lightsSerializationData.transform_localPosition[i]) == value.light.transform.localPosition
                                    )
                                {
                                    lightsSerializationData.Deserializ(value.light, i, value);
                                    deserialized.Add(value.light);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < allLights.Length; j++)
                    {
                        if (deserialized.Contains(allLights[j]) == false &&
                            lightsSerializationData.name[i] == allLights[j].name &&
                            (LightType)(int.Parse(lightsSerializationData.type[i])) == allLights[j].type &&
                            lightsSerializationData.hierarchyPath[i] == LightsSerializationData.GetHierarchyPath(allLights[j]) &&
                            LightsSerializationData.ToVector3(lightsSerializationData.transform_position[i]) == allLights[j].transform.position &&
                            LightsSerializationData.ToVector3(lightsSerializationData.transform_localPosition[i]) == allLights[j].transform.localPosition
                            )
                        {
                            lightsSerializationData.Deserializ(allLights[j], i);
                            deserialized.Add(allLights[j]);
                            break;
                        }
                    }
                }
            }
        }
    }
}
