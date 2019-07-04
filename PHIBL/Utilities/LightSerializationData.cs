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
        public List<string> transform_localPosition = new List<string>();
        [SerializeField]
        public List<string> transform_eulerAngles = new List<string>();
        [SerializeField]
        public List<string> transform_localEulerAngles = new List<string>();
        [SerializeField]
        public List<string> transform_right = new List<string>();
        [SerializeField]
        public List<string> transform_up = new List<string>();
        [SerializeField]
        public List<string> transform_forward = new List<string>();
        [SerializeField]
        public List<string> transform_rotation = new List<string>();
        [SerializeField]
        public List<string> transform_position = new List<string>();
        [SerializeField]
        public List<string> transform_localRotation = new List<string>();
        [SerializeField]
        public List<string> transform_localScale = new List<string>();
        [SerializeField]
        public List<string> enabled = new List<string>();
        [SerializeField]
        public List<string> instanceId = new List<string>();
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
        public List<string> alloy_transform_localPosition = new List<string>();
        [SerializeField]
        public List<string> alloy_transform_eulerAngles = new List<string>();
        [SerializeField]
        public List<string> alloy_transform_localEulerAngles = new List<string>();
        [SerializeField]
        public List<string> alloy_transform_right = new List<string>();
        [SerializeField]
        public List<string> alloy_transform_up = new List<string>();
        [SerializeField]
        public List<string> alloy_transform_forward = new List<string>();
        [SerializeField]
        public List<string> alloy_transform_rotation = new List<string>();
        [SerializeField]
        public List<string> alloy_transform_position = new List<string>();
        [SerializeField]
        public List<string> alloy_transform_localRotation = new List<string>();
        [SerializeField]
        public List<string> alloy_transform_localScale = new List<string>();
        [SerializeField]
        public List<string> alloy_enabled = new List<string>();
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

        public static string ToString(Vector3 vector)
        {
            return vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString();
        }

        public static string ToString(Quaternion quaternion)
        {
            return quaternion.x.ToString() + "," + quaternion.y.ToString() + "," + quaternion.z.ToString() + "," + quaternion.w.ToString();
        }

        public static Vector3 ToVector3(string str)
        {
            string[] arr = str.Split(',');
            return new Vector3(x: float.Parse(arr[0]), y: float.Parse(arr[1]), z: float.Parse(arr[2]));
        }

        public static Quaternion ToQuaternion(string str)
        {
            string[] arr = str.Split(',');
            return new Quaternion(x: float.Parse(arr[0]), y: float.Parse(arr[1]), z: float.Parse(arr[2]), w: float.Parse(arr[3]));
        }

        public void Serializ(Light light)
        {
            transform_localPosition.Add(ToString(light.transform.localPosition));
            transform_eulerAngles.Add(ToString(light.transform.eulerAngles));
            transform_localEulerAngles.Add(ToString(light.transform.localEulerAngles));
            transform_right.Add(ToString(light.transform.right));
            transform_up.Add(ToString(light.transform.up));
            transform_forward.Add(ToString(light.transform.forward));
            transform_rotation.Add(ToString(light.transform.rotation));
            transform_position.Add(ToString(light.transform.position));
            transform_localRotation.Add(ToString(light.transform.localRotation));
            transform_localScale.Add(ToString(light.transform.localScale));
            enabled.Add(Convert.ToInt32(light.enabled).ToString());
            instanceId.Add(light.GetInstanceID().ToString());
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

            AlloyAreaLight alloyAreaLight = light.GetComponent<AlloyAreaLight>();
            alloy_transform_localPosition.Add(ToString(alloyAreaLight.transform.localPosition));
            alloy_transform_eulerAngles.Add(ToString(alloyAreaLight.transform.eulerAngles));
            alloy_transform_localEulerAngles.Add(ToString(alloyAreaLight.transform.localEulerAngles));
            alloy_transform_right.Add(ToString(alloyAreaLight.transform.right));
            alloy_transform_up.Add(ToString(alloyAreaLight.transform.up));
            alloy_transform_forward.Add(ToString(alloyAreaLight.transform.forward));
            alloy_transform_rotation.Add(ToString(alloyAreaLight.transform.rotation));
            alloy_transform_position.Add(ToString(alloyAreaLight.transform.position));
            alloy_transform_localRotation.Add(ToString(alloyAreaLight.transform.localRotation));
            alloy_transform_localScale.Add(ToString(alloyAreaLight.transform.localScale));
            alloy_enabled.Add(Convert.ToInt32(alloyAreaLight.enabled).ToString());
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
            light.transform.localPosition = ToVector3(transform_localPosition[index]);
            light.transform.eulerAngles = ToVector3(transform_eulerAngles[index]);
            light.transform.localEulerAngles = ToVector3(transform_localEulerAngles[index]);
            light.transform.right = ToVector3(transform_right[index]);
            light.transform.up = ToVector3(transform_up[index]);
            light.transform.forward = ToVector3(transform_forward[index]);
            light.transform.rotation = ToQuaternion(transform_rotation[index]);
            light.transform.position = ToVector3(transform_position[index]);
            light.transform.localRotation = ToQuaternion(transform_localRotation[index]);
            light.transform.localScale = ToVector3(transform_localScale[index]);
            light.enabled = (int.Parse(enabled[index]) == 1);
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

            AlloyAreaLight alloyAreaLight = light.GetComponent<AlloyAreaLight>();
            alloyAreaLight.transform.localPosition = ToVector3(alloy_transform_localPosition[index]);
            alloyAreaLight.transform.eulerAngles = ToVector3(alloy_transform_eulerAngles[index]);
            alloyAreaLight.transform.localEulerAngles = ToVector3(alloy_transform_localEulerAngles[index]);
            alloyAreaLight.transform.right = ToVector3(alloy_transform_right[index]);
            alloyAreaLight.transform.up = ToVector3(alloy_transform_up[index]);
            alloyAreaLight.transform.forward = ToVector3(alloy_transform_forward[index]);
            alloyAreaLight.transform.rotation = ToQuaternion(alloy_transform_rotation[index]);
            alloyAreaLight.transform.position = ToVector3(alloy_transform_position[index]);
            alloyAreaLight.transform.localRotation = ToQuaternion(alloy_transform_localRotation[index]);
            alloyAreaLight.transform.localScale = ToVector3(alloy_transform_localScale[index]);
            alloyAreaLight.enabled = (int.Parse(alloy_enabled[index]) == 1);
            alloyAreaLight.Radius = float.Parse(alloy_Radius[index]);
            alloyAreaLight.Length = float.Parse(alloy_Length[index]);
            alloyAreaLight.Intensity = float.Parse(alloy_Intensity[index]);
            alloyAreaLight.Color = new Color(float.Parse(alloy_Color_r[index]), float.Parse(alloy_Color_g[index]), float.Parse(alloy_Color_b[index]));
            alloyAreaLight.HasSpecularHighlight = (int.Parse(alloy_HasSpecularHighlight[index]) == 1);
            alloyAreaLight.IsAnimatedByClip = (int.Parse(alloy_IsAnimatedByClip[index]) == 1);
        }
    }
}
