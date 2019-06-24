using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PHIBL
{
    static class RendererSetup
    {
        static public void SetupProbes()
        {
            var humans = UnityEngine.Object.FindObjectsOfType<Human>();
            foreach (var human in humans)
            {
                SetupProbes(human);
            }
        }

        static public void SetupProbes(Human human)
        {
            Transform t;
            if (human.sex == Character.SEX.MALE)
                t = Transform_Utility.FindTransform(human.body.AnimatedBoneRoot, "cm_J_Spine01");
            else
                t = Transform_Utility.FindTransform(human.body.AnimatedBoneRoot, "cf_J_Spine01");
            Console.WriteLine("Setup probes on " + human.name + ", anchor name: " + t.name);
            foreach (var renderer in human.GetComponentsInChildren<Renderer>(true))
            {
                renderer.reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
                renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
                renderer.probeAnchor = t;
            }
        }
        
    }


}
