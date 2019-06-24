using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
namespace LightsSwitch
{
    public class LightOnOff : MonoBehaviour
    {
        public List<GameObject> Lights;
        public bool On;
        public List<Material> Emissive_Mats;
        public List<Renderer> Emissive_Objs;
        public Color Color;
        public float Intensity;
        void Awake()
        {
            if (On)
            {
                foreach(var l in Lights)
                {
                    l.SetActive(true);
                }
                EmissiveON();
            }
            else
            {
                foreach (var l in Lights)
                {
                    l.SetActive(false);
                }
                EmissiveOFF();
            }
        }

        void OnMouseDown()
        {
            StartCoroutine(RefreshProbes());
            if (Lights[0].activeInHierarchy == false)
            {
                //if ( On == true ){
                foreach (var l in Lights)
                {
                    l.SetActive(true);
                }
                EmissiveON();
                On = !On;
            }
            else
            {
                foreach (var l in Lights)
                {
                    l.SetActive(false);
                }
                EmissiveOFF();
                On = !On;
            }            
        }

        IEnumerator RefreshProbes()
        {
            yield return new WaitForEndOfFrame();
            var rps = FindObjectsOfType<ReflectionProbe>();
            var newrps = rps.Where(x => x.refreshMode == UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting).ToArray();
            if(newrps == null || newrps.Length == 0)
                yield break;            
            foreach (var rp in newrps)
            {
                rp.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
                rp.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
            }
            yield return new WaitForSeconds(2f);
            foreach (var rp in newrps)
            {
                rp.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
            }
        }


        void EmissiveON()
        {
            foreach (var obj in Emissive_Objs)
            {
                DynamicGI.SetEmissive(obj, Color * Intensity);
            }
            foreach(var mat in Emissive_Mats)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color * Intensity);
            }
        }

        void EmissiveOFF()
        {
            foreach (var obj in Emissive_Objs)
            {
                DynamicGI.SetEmissive(obj, Color.black);
            }
            foreach (var mat in Emissive_Mats)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}
